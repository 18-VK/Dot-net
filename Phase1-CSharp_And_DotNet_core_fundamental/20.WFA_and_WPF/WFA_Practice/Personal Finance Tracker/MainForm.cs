using System;
using System.Diagnostics.Eventing.Reader;
using System.Transactions;
using System.Windows.Forms;
using Personal_Finance_Tracker.Model;
using System.Linq;

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
        private void UpdateSerialNumbers()
        {
            for (int i = 0; i < dataGridViewMain.Rows.Count; i++)
            {
                // skip new row if present
                if (dataGridViewMain.Rows[i].IsNewRow) continue;
                dataGridViewMain.Rows[i].Cells["SR No"].Value = (i + 1);
            }
        }

        public async Task<bool> ReadCSVFileAndUpdatGrid(string filename)
        {
            string[]? header = null;
            string? val;
            ClsTransaction? transaction = null;
            //Clear Grid, by resting data source as here we are using data source to maintain 
            // Grid 
            Program.TransactionData.Clear();
            Program.SourceTransactionData.Clear();
            dataGridViewMain.Refresh();
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
                SaveD.CheckFileExists = true;
                SaveD.FileName = $"Transactions_{DateTime.Now}.csv";

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
            }
        }
        private void dataGridViewMain_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateSerialNumbers();
        }

        private void dataGridViewMain_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateSerialNumbers();
        }
        private void dataGridViewMain_Sorted(object sender, EventArgs e)
        {
            UpdateSerialNumbers();
        }

        private void viewSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // View Summary will represent whole data from given source
            Program.TransactionData.Clear();
            if (Program.SourceTransactionData.Count <= 0)
            {
                MessageBox.Show("No Transaction data \n Please read data from Database or by importing CSV");
                dataGridViewMain.Refresh();
                return;
            }
            foreach (var list in Program.SourceTransactionData)
            {
                Program.TransactionData.Add(list);
            }
            dataGridViewMain.Refresh();
        }

        private void IncomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This will filter Data by type : Income only 
            Program.TransactionData.Clear();
            if (Program.SourceTransactionData.Count <= 0)
            {
                MessageBox.Show("No Transaction data \n Please read data from Database or by importing CSV");
                dataGridViewMain.Refresh();
                return;
            }
            Program.TransactionData = (List<ClsTransaction>)Program.SourceTransactionData.Where(R => R?.Type?.ToUpper()?.Trim() == "INCOME").ToList();
            dataGridViewMain.Refresh();
        }

        private void expenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This will filter Data by type : expense only 
            Program.TransactionData.Clear();
            if (Program.SourceTransactionData.Count <= 0)
            {
                MessageBox.Show("No Transaction data \n Please read data from Database or by importing CSV");
                dataGridViewMain.Refresh();
                return;
            }
            Program.TransactionData = (List<ClsTransaction>)Program.SourceTransactionData.Where(R => R?.Type?.ToUpper()?.Trim() == "EXPENSE").ToList();
            dataGridViewMain.Refresh();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This will filter Data by date range : expense only 
            using (DateTimePicker DatePick = new DateTimePicker())
            {
                // DatePick.
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // If Import csv and have file name
            if(Program.SourceOfData == Program.SourceImportFile && File.Exists(Program.MCurrentCSV))
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
                        return;
                    }
                }
            }
        }
    }
}
