using Microsoft.EntityFrameworkCore;
using Personal_Finance_Tracker.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Personal_Finance_Tracker
{
    enum GridEnum {
        IDX_SR = 0,
        IDX_RecID,
        IDX_Cat,
        IDX_Amt,
        IDX_Date,
        IDX_Type,
    };

    public partial class MainForm : Form
    {

        public void RefreshGrid()
        {
            dataGridViewMain.Refresh();
        }

        public void ClearGrid()
        {
            dataGridViewMain.DataSource = null;
            Program.TransactionData.Clear();
            dataGridViewMain.DataSource = Program.TransactionData;
        }
        public void ReassignDataSource()
        {
            //Why: DataGridView keeps a reference to the old list instance; assigning a new List<T> does NOT update the grid unless you reassign the DataSource.
            dataGridViewMain.DataSource = null;
            dataGridViewMain.DataSource = Program.TransactionData;
        }
        private void UpdateSerialNumbers()
        {
            for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
            {
                // skip new row if present
                if (dataGridViewMain.Rows[i].IsNewRow) continue;
                dataGridViewMain.Rows[i].Cells["DataGridColSRNo"].Value = (i + 1);
            }
        }
        public async Task<bool> ReadDBForRecord()
        {
            const int cancellationSeconds = 5;
            const int commandTimeoutSeconds = 10; // >= cancellationSeconds

            try
            {
                using var db = new EFContext();

                if (!db.Database.CanConnect())
                {
                    MessageBox.Show("Cannot connect to database.");
                    return false;
                }

                // Ensure server/client timeouts are sane
                db.Database.SetCommandTimeout(commandTimeoutSeconds);

                var query = db.TableTransactions
                              .AsNoTracking()
                              .OrderBy(t => t.Id)
                              .Take(1024);

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(cancellationSeconds));

                List<ClsTransaction> transData;
                try
                {
                    transData = await query.ToListAsync(cts.Token);
                }
                catch (OperationCanceledException oce)
                {
                    MessageBox.Show("Database read was cancelled (operation timeout).");
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception : " + ex.Message, "Read Database");
                    return false;
                }

                // update global lists safely
                Program.SourceTransactionData = transData ?? new List<ClsTransaction>();
                Program.TransactionData = new List<ClsTransaction>(Program.SourceTransactionData);
                ReassignDataSource();
                RefreshGrid();

                if (transData?.Count == 0)
                {
                    MessageBox.Show("No records found!", "Read Database");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
                return false;
            }

            return true;
        }


        public async Task<bool> ReadCSVFileAndUpdatGrid(string filename)
        {
            string[]? header = null;
            string? val;
            ClsTransaction? transaction = null;
            //Clear Grid, by resting data source as here we are using data source to maintain 
            // Grid 
            ClearGrid();
            Program.SourceTransactionData.Clear();
            RefreshGrid();
            try
            {

                using (var SR = new StreamReader(filename))
                {
                    val = await SR.ReadLineAsync();
                    if (val != null)
                        header = val.Split(',');

                    if (header == null || header.Length <= 0)
                    {
                        MessageBox.Show("Invalid header of given CSV file : " + filename);
                        return false;
                    }
                    if (dataGridViewMain.Columns.Count - 1 != header.Length)
                    {
                        MessageBox.Show("Invalid header of given CSV file : " + filename + "\n Count of columns not matched");
                        return false;
                    }
                    // match both header 
                    for (int i = 1; i < dataGridViewMain.Columns.Count - 1; i++)// skip SR no
                    {
                        if (string.Compare(header[i - 1], dataGridViewMain.Columns[i].HeaderText, true) != 0)
                        {
                            MessageBox.Show("Invalid header of given CSV file : " + filename +
                                "\n Grid and File header don't match");
                            return false;
                        }
                    }

                    //read data 
                    while (!SR.EndOfStream)
                    {
                        int nextSr = dataGridViewMain.Rows.Count + 1;
                        object[]? dataArr = null;
                        val = await SR.ReadLineAsync();
                        if (val == null) continue;
                        val = nextSr.ToString() + "," + val;
                        dataArr = val.Split(',');
                        transaction = new ClsTransaction
                        {
                            Id = (int)dataArr[(int)GridEnum.IDX_RecID],
                            Amount = (decimal)dataArr[(int)GridEnum.IDX_Amt],
                            Category = (string)dataArr[(int)GridEnum.IDX_Cat],
                            Date = (DateTime)dataArr[(int)GridEnum.IDX_Date],
                            Type = (string)dataArr[(int)GridEnum.IDX_Type]
                        };
                        // Could add data manually or can use Data source 
                        //dataGridViewMain.Rows.Add(dataArr);

                        // In case of data source, but update the datasource 
                        Program.TransactionData.Add(transaction);
                        Program.SourceTransactionData.Add(transaction);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadCSVFileAndUpdatGrid|Exception caught : " + ex.Message);
                return false;
            }
            return true;
        }
        public MainForm()
        {
            Program.SourceOfData = 0;
            InitializeComponent();
        }

        private void toolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            using (var OpenD = new OpenFileDialog())
            {
                OpenD.Title = "Select CSV File";
                OpenD.Filter = "CSV file (*.CSV)|*.CSV";
                OpenD.InitialDirectory = Directory.GetCurrentDirectory();
                OpenD.CheckFileExists = true;
                OpenD.Multiselect = false;

                var filename = OpenD.FileName;

                if (OpenD.ShowDialog() == DialogResult.OK)
                {
                    // Set Source of data as local file 
                    Program.SourceOfData = Program.SourceImportFile;
                    Program.MCurrentCSV = filename;
                    // Read line and print in Grid..
                    if (!ReadCSVFileAndUpdatGrid(filename).Result)
                    {
                        MessageBox.Show("Got error while reading data from given file");
                        return;
                    }
                }
            }
        }

        private void toolStripMenuItemExport_Click(object sender, EventArgs e)
        {
            using (var SaveD = new SaveFileDialog())
            {
                SaveD.Title = "Select CSV File";
                SaveD.Filter = "CSV file (*.CSV)|*.CSV";
                SaveD.InitialDirectory = Directory.GetCurrentDirectory();
                SaveD.CheckFileExists = false;
                SaveD.FileName = $"Transactions_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.csv";

                if (SaveD.ShowDialog() == DialogResult.OK)
                {

                    // Write lines in file
                    using (StreamWriter sw = new StreamWriter(SaveD.FileName))
                    {
                        // header 
                        var headerData = "";
                        foreach (DataGridViewColumn col in dataGridViewMain.Columns)
                        {
                            if (headerData.Length > 0)
                                headerData += ",";

                            headerData += col.HeaderText;
                        }
                        sw.WriteLine(headerData);
                        foreach (DataGridViewRow row in dataGridViewMain.Rows)
                        {
                            var rowdata = "";
                            if (row.IsNewRow) continue;
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (rowdata.Length > 0)
                                    rowdata += ",";
                                rowdata += cell.Value;
                            }
                            sw.WriteLine(rowdata);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Request cancelled by user", "Export CSV");
                }
            }
        }
        private void dataGridViewMain_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            toolStripStatusLabelNumRecVal.Text = dataGridViewMain.RowCount.ToString();
            UpdateSerialNumbers();
        }

        private void dataGridViewMain_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            toolStripStatusLabelNumRecVal.Text = dataGridViewMain.RowCount.ToString();
            UpdateSerialNumbers();
        }
        private void dataGridViewMain_Sorted(object sender, EventArgs e)
        {
            UpdateSerialNumbers();
        }

        private void viewSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // View Summary will represent whole data from given source
            ClearGrid();
            if (Program.SourceTransactionData.Count <= 0)
            {
                MessageBox.Show("No Transaction data \n Please read data from Database or by importing CSV");
                RefreshGrid();
                return;
            }
            foreach (var list in Program.SourceTransactionData)
            {
                Program.TransactionData.Add(list);
            }
            RefreshGrid();
        }

        private void IncomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This will filter Data by type : Income only 
            ClearGrid();
            if (Program.SourceTransactionData.Count <= 0)
            {
                MessageBox.Show("No Transaction data \n Please read data from Database or by importing CSV");
                RefreshGrid();
                return;
            }
            Program.TransactionData = (List<ClsTransaction>)Program.SourceTransactionData.Where(R => R?.Type?.ToUpper()?.Trim() == "INCOME").ToList();
            ReassignDataSource();
            RefreshGrid();
        }

        private void expenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This will filter Data by type : expense only 
            ClearGrid();
            if (Program.SourceTransactionData.Count <= 0)
            {
                MessageBox.Show("No Transaction data \n Please read data from Database or by importing CSV");
                RefreshGrid();
                return;
            }
            Program.TransactionData = (List<ClsTransaction>)Program.SourceTransactionData.Where(R => R?.Type?.ToUpper()?.Trim() == "EXPENSE").ToList();
            ReassignDataSource();
            RefreshGrid();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This will filter Data by date range : expense only 
            using (DateTimePicker DatePick = new DateTimePicker())
            {
                // DatePick.
            }
        }

        private async void BtnRefresh_Click(object sender, EventArgs e)
        {
            // If Import csv and have file name
            if (Program.SourceOfData == Program.SourceImportFile && File.Exists(Program.MCurrentCSV))
            {
                // refresh require
                var result = MessageBox.Show($"Want to get updated daat from given file \n File name : {Program.MCurrentCSV}");
                if (result == DialogResult.OK)
                {
                    // read csv 
                    if (!ReadCSVFileAndUpdatGrid(Program.MCurrentCSV).Result)
                    {
                        MessageBox.Show("Got error while reading data from given file");
                        return;
                    }
                }
                return;
            }
            // select source to read data..
            using (var SD = new ReadOption())
            {
                if (SD.ShowDialog() == DialogResult.OK)
                {
                    if (SD.RbtnReadOpsImpCSV.Checked)
                    {
                        toolStripMenuItemImport_Click(sender, e);
                        return;
                    }
                    if (SD.RbtnReadOpReadDB.Checked)
                    {
                        // read from db 
                        var result = await ReadDBForRecord();
                        if (!result)
                            MessageBox.Show("Error while data from Database");

                        return;
                    }
                }
            }
        }
        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var SF = new SelectData())
            {

                if (SF != null)
                {
                    const int cancellationSeconds = 5;
                    const int commandTimeoutSeconds = 10; // >= cancellationSeconds

                    if (SF.ShowDialog(this) == DialogResult.OK)
                    {
                        // Add Row..
                        decimal Amt;
                        string type = String.Empty, cat = String.Empty;
                        DateTime Date;

                        decimal.TryParse(SF.textBoxAmt.Text, out Amt);
                        type = SF.comboBoxType.Text;
                        cat = SF.comboBoxCategory.Text;
                        Date = SF.dateTimePicker1.Value;

                        ClsTransaction obj = new ClsTransaction
                        {
                            Amount = Amt,
                            Type = type,
                            Category = cat,
                            Date = Date
                        };
                        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(cancellationSeconds));
                        try
                        {

                            using (var DBConext = new EFContext())
                            {
                                if (!DBConext.Database.CanConnect())
                                {
                                    MessageBox.Show("Can't connect to Database");
                                    return;
                                }
                                DBConext.Database.SetCommandTimeout(commandTimeoutSeconds);

                                var result = await DBConext.TableTransactions.AddAsync(obj, cts.Token);
                                if (result.State != EntityState.Added)
                                {
                                    MessageBox.Show("Record not added", "Add record");
                                    return;
                                }

                                if (DBConext.SaveChanges() <= 0)
                                {
                                    MessageBox.Show("Record not Inserted in Database", "Add record");
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception : " + ex.Message, "Add record");
                        }

                    }
                }
            }
            return;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedRows.Count == 1)
            {
                using (var SF = new SelectData())
                {
                    try
                    {

                        if (SF != null)
                        {
                            var row = dataGridViewMain.SelectedRows[0];

                            SF.comboBoxType.Text = row.Cells["typeDataGridViewTextBoxColumn"]?.Value?.ToString();
                            SF.comboBoxCategory.Text = row.Cells["categoryDataGridViewTextBoxColumn"]?.Value?.ToString();
                            SF.dateTimePicker1.Value = row.Cells["dateDataGridViewTextBoxColumn"]?.Value != null ? (DateTime)row.Cells["dateDataGridViewTextBoxColumn"]?.Value : SF.dateTimePicker1.Value;
                            SF.textBoxAmt.Text = row.Cells["amountDataGridViewTextBoxColumn"]?.Value?.ToString();

                            var RecordId = (int)row.Cells["idDataGridViewTextBoxColumn"].Value;

                            if (SF.ShowDialog(this) == DialogResult.OK)
                            {
                                if (SF.DialogResult == DialogResult.OK)
                                {
                                    // Find record in collection 
                                    using (var EFContext = new EFContext())
                                    {
                                        decimal amt;
                                        var Record = EFContext.TableTransactions.Where(T => T.Id == RecordId).FirstOrDefault();

                                        if (Record != null)
                                        {
                                            Record.Date = SF.dateTimePicker1.Value;
                                            Record.Type = SF.comboBoxType.Text;

                                            decimal.TryParse(SF.textBoxAmt.Text, out amt);
                                            Record.Amount = amt;
                                            Record.Category = SF.comboBoxCategory.Text;

                                            //Update into DB 
                                            EFContext.TableTransactions.Update(Record);
                                            var result = EFContext.SaveChanges();
                                            if (result != 0)
                                                MessageBox.Show("Record Updated..", "Edit/Update");
                                            else
                                                MessageBox.Show("Record Updatation failed", "Edit/Update");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Unexpected error, record not found in collection", "Edit/Update");
                                        }
                                    }

                                }
                            }
                            else
                            {
                                MessageBox.Show("Request cancelled by user", "Edit/Update");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception Caught : " + ex.Message, "Edit/Update");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to do operation on that", "Edit/Update");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridViewMain.SelectedRows.Count == 1)
                {
                    var row = dataGridViewMain.SelectedRows[0];
                    var RecordID = (int)row.Cells["idDataGridViewTextBoxColumn"].Value;
                    if (row != null)
                    {
                        using (var EFcontext = new EFContext())
                        {

                            var Record = EFcontext.TableTransactions.Where(T => T.Id == RecordID).FirstOrDefault();
                            if (Record != null)
                            {
                                EFcontext.TableTransactions.Remove(Record);
                                if (EFcontext.SaveChanges() > 0)
                                    MessageBox.Show("Record deleted", "Delete/remove");
                                else
                                    MessageBox.Show("Record deletion failed", "Delete/remove");
                            }
                            else
                            {
                                MessageBox.Show("Unexpected result : Record not found in collection", "Delete/remove");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to do operation on that", "Delete/Remove");
                }
            }
            catch (Exception EX)
            {
                MessageBox.Show("Exception caught : " + EX.Message, "Delete/Remove");
            }
            return;
        }
    }
}
