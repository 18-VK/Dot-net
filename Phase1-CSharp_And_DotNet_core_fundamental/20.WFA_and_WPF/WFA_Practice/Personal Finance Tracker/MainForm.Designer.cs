
using System.Windows.Forms;
namespace Personal_Finance_Tracker
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStripMain = new MenuStrip();
            toolStripMenuItemFIle = new ToolStripMenuItem();
            toolStripMenuItemImport = new ToolStripMenuItem();
            toolStripMenuItemExport = new ToolStripMenuItem();
            toolStripMenuItemFilter = new ToolStripMenuItem();
            viewSummaryToolStripMenuItem = new ToolStripMenuItem();
            IncomeToolStripMenuItem = new ToolStripMenuItem();
            expenseToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            statusStripMain = new StatusStrip();
            toolStripStatusLabelDateRange = new ToolStripStatusLabel();
            toolStripStatusLabelDateRVal = new ToolStripStatusLabel();
            toolStripStatusLabelSpacer = new ToolStripStatusLabel();
            toolStripStatusLabelNumSRec = new ToolStripStatusLabel();
            toolStripStatusLabelNumRecVal = new ToolStripStatusLabel();
            toolStripStatusLabelSpacer1 = new ToolStripStatusLabel();
            toolStripStatusLabelCurrDate = new ToolStripStatusLabel();
            toolStripStatusLblCurrVal = new ToolStripStatusLabel();
            tableLayoutOuter = new TableLayoutPanel();
            tableLayoutPanelInner = new TableLayoutPanel();
            dataGridViewMain = new DataGridView();
            DataGridColSRNo = new DataGridViewTextBoxColumn();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            categoryDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            amountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            typeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            transactionBindingSource = new BindingSource(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            button2 = new Button();
            BtnRefresh = new Button();
            BtnAdd = new Button();
            BtnUpdate = new Button();
            BtnDelete = new Button();
            transactionBindingSource1 = new BindingSource(components);
            menuStripMain.SuspendLayout();
            statusStripMain.SuspendLayout();
            tableLayoutOuter.SuspendLayout();
            tableLayoutPanelInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewMain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)transactionBindingSource).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)transactionBindingSource1).BeginInit();
            SuspendLayout();
            // 
            // menuStripMain
            // 
            menuStripMain.ImageScalingSize = new Size(20, 20);
            menuStripMain.Items.AddRange(new ToolStripItem[] { toolStripMenuItemFIle, toolStripMenuItemFilter });
            menuStripMain.Location = new Point(0, 0);
            menuStripMain.Name = "menuStripMain";
            menuStripMain.Size = new Size(1082, 29);
            menuStripMain.TabIndex = 0;
            menuStripMain.Text = "menuStripMain";
            // 
            // toolStripMenuItemFIle
            // 
            toolStripMenuItemFIle.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItemImport, toolStripMenuItemExport });
            toolStripMenuItemFIle.Name = "toolStripMenuItemFIle";
            toolStripMenuItemFIle.Size = new Size(48, 25);
            toolStripMenuItemFIle.Text = "File";
            // 
            // toolStripMenuItemImport
            // 
            toolStripMenuItemImport.Name = "toolStripMenuItemImport";
            toolStripMenuItemImport.Size = new Size(174, 26);
            toolStripMenuItemImport.Text = "Import CSV";
            toolStripMenuItemImport.Click += toolStripMenuItemImport_Click;
            // 
            // toolStripMenuItemExport
            // 
            toolStripMenuItemExport.Name = "toolStripMenuItemExport";
            toolStripMenuItemExport.Size = new Size(174, 26);
            toolStripMenuItemExport.Text = "Export CSV";
            toolStripMenuItemExport.Click += toolStripMenuItemExport_Click;
            // 
            // toolStripMenuItemFilter
            // 
            toolStripMenuItemFilter.DropDownItems.AddRange(new ToolStripItem[] { viewSummaryToolStripMenuItem, IncomeToolStripMenuItem, expenseToolStripMenuItem, searchToolStripMenuItem });
            toolStripMenuItemFilter.Name = "toolStripMenuItemFilter";
            toolStripMenuItemFilter.Size = new Size(59, 25);
            toolStripMenuItemFilter.Text = "Filter";
            // 
            // viewSummaryToolStripMenuItem
            // 
            viewSummaryToolStripMenuItem.Name = "viewSummaryToolStripMenuItem";
            viewSummaryToolStripMenuItem.Size = new Size(200, 26);
            viewSummaryToolStripMenuItem.Text = "View Summary";
            viewSummaryToolStripMenuItem.Click += viewSummaryToolStripMenuItem_Click;
            // 
            // IncomeToolStripMenuItem
            // 
            IncomeToolStripMenuItem.Name = "IncomeToolStripMenuItem";
            IncomeToolStripMenuItem.Size = new Size(200, 26);
            IncomeToolStripMenuItem.Text = "Income";
            IncomeToolStripMenuItem.Click += IncomeToolStripMenuItem_Click;
            // 
            // expenseToolStripMenuItem
            // 
            expenseToolStripMenuItem.Name = "expenseToolStripMenuItem";
            expenseToolStripMenuItem.Size = new Size(200, 26);
            expenseToolStripMenuItem.Text = "Expense";
            expenseToolStripMenuItem.Click += expenseToolStripMenuItem_Click;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(200, 26);
            searchToolStripMenuItem.Text = "Filter by Date";
            searchToolStripMenuItem.Click += searchToolStripMenuItem_Click;
            // 
            // statusStripMain
            // 
            statusStripMain.ImageScalingSize = new Size(20, 20);
            statusStripMain.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelDateRange, toolStripStatusLabelDateRVal, toolStripStatusLabelSpacer, toolStripStatusLabelNumSRec, toolStripStatusLabelNumRecVal, toolStripStatusLabelSpacer1, toolStripStatusLabelCurrDate, toolStripStatusLblCurrVal });
            statusStripMain.Location = new Point(0, 513);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Size = new Size(1082, 40);
            statusStripMain.TabIndex = 1;
            statusStripMain.Text = "statusStripMain";
            // 
            // toolStripStatusLabelDateRange
            // 
            toolStripStatusLabelDateRange.Name = "toolStripStatusLabelDateRange";
            toolStripStatusLabelDateRange.Size = new Size(133, 34);
            toolStripStatusLabelDateRange.Spring = true;
            toolStripStatusLabelDateRange.Text = "Date Range : ";
            // 
            // toolStripStatusLabelDateRVal
            // 
            toolStripStatusLabelDateRVal.AutoSize = false;
            toolStripStatusLabelDateRVal.Name = "toolStripStatusLabelDateRVal";
            toolStripStatusLabelDateRVal.Size = new Size(133, 34);
            toolStripStatusLabelDateRVal.Spring = true;
            // 
            // toolStripStatusLabelSpacer
            // 
            toolStripStatusLabelSpacer.AutoSize = false;
            toolStripStatusLabelSpacer.Name = "toolStripStatusLabelSpacer";
            toolStripStatusLabelSpacer.Size = new Size(133, 34);
            toolStripStatusLabelSpacer.Spring = true;
            // 
            // toolStripStatusLabelNumSRec
            // 
            toolStripStatusLabelNumSRec.Name = "toolStripStatusLabelNumSRec";
            toolStripStatusLabelNumSRec.Size = new Size(133, 34);
            toolStripStatusLabelNumSRec.Spring = true;
            toolStripStatusLabelNumSRec.Text = "Nums of records : ";
            // 
            // toolStripStatusLabelNumRecVal
            // 
            toolStripStatusLabelNumRecVal.AutoSize = false;
            toolStripStatusLabelNumRecVal.Name = "toolStripStatusLabelNumRecVal";
            toolStripStatusLabelNumRecVal.Size = new Size(133, 34);
            toolStripStatusLabelNumRecVal.Spring = true;
            // 
            // toolStripStatusLabelSpacer1
            // 
            toolStripStatusLabelSpacer1.AutoSize = false;
            toolStripStatusLabelSpacer1.Name = "toolStripStatusLabelSpacer1";
            toolStripStatusLabelSpacer1.Size = new Size(133, 34);
            toolStripStatusLabelSpacer1.Spring = true;
            // 
            // toolStripStatusLabelCurrDate
            // 
            toolStripStatusLabelCurrDate.Name = "toolStripStatusLabelCurrDate";
            toolStripStatusLabelCurrDate.Size = new Size(133, 34);
            toolStripStatusLabelCurrDate.Spring = true;
            toolStripStatusLabelCurrDate.Text = "Current Date : ";
            // 
            // toolStripStatusLblCurrVal
            // 
            toolStripStatusLblCurrVal.AutoSize = false;
            toolStripStatusLblCurrVal.Name = "toolStripStatusLblCurrVal";
            toolStripStatusLblCurrVal.RightToLeft = RightToLeft.No;
            toolStripStatusLblCurrVal.Size = new Size(133, 34);
            toolStripStatusLblCurrVal.Spring = true;
            // 
            // tableLayoutOuter
            // 
            tableLayoutOuter.ColumnCount = 1;
            tableLayoutOuter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutOuter.Controls.Add(tableLayoutPanelInner, 0, 0);
            tableLayoutOuter.Controls.Add(tableLayoutPanel1, 0, 1);
            tableLayoutOuter.Dock = DockStyle.Fill;
            tableLayoutOuter.Location = new Point(0, 29);
            tableLayoutOuter.Name = "tableLayoutOuter";
            tableLayoutOuter.RowCount = 3;
            tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            tableLayoutOuter.Size = new Size(1082, 484);
            tableLayoutOuter.TabIndex = 2;
            // 
            // tableLayoutPanelInner
            // 
            tableLayoutPanelInner.ColumnCount = 3;
            tableLayoutPanelInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            tableLayoutPanelInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));
            tableLayoutPanelInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            tableLayoutPanelInner.Controls.Add(dataGridViewMain, 1, 0);
            tableLayoutPanelInner.Dock = DockStyle.Fill;
            tableLayoutPanelInner.Location = new Point(3, 3);
            tableLayoutPanelInner.Name = "tableLayoutPanelInner";
            tableLayoutPanelInner.RowCount = 1;
            tableLayoutPanelInner.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelInner.Size = new Size(1076, 381);
            tableLayoutPanelInner.TabIndex = 0;
            // 
            // dataGridViewMain
            // 
            dataGridViewMain.AllowUserToAddRows = false;
            dataGridViewMain.AllowUserToDeleteRows = false;
            dataGridViewMain.AutoGenerateColumns = false;
            dataGridViewMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewMain.Columns.AddRange(new DataGridViewColumn[] { DataGridColSRNo, idDataGridViewTextBoxColumn, categoryDataGridViewTextBoxColumn, amountDataGridViewTextBoxColumn, dateDataGridViewTextBoxColumn, typeDataGridViewTextBoxColumn });
            dataGridViewMain.DataSource = transactionBindingSource;
            dataGridViewMain.Dock = DockStyle.Fill;
            dataGridViewMain.Location = new Point(56, 3);
            dataGridViewMain.Name = "dataGridViewMain";
            dataGridViewMain.ReadOnly = true;
            dataGridViewMain.RowHeadersVisible = false;
            dataGridViewMain.RowHeadersWidth = 51;
            dataGridViewMain.Size = new Size(962, 375);
            dataGridViewMain.TabIndex = 0;
            // 
            // DataGridColSRNo
            // 
            DataGridColSRNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DataGridColSRNo.HeaderText = "SR. No";
            DataGridColSRNo.MinimumWidth = 6;
            DataGridColSRNo.Name = "DataGridColSRNo";
            DataGridColSRNo.ReadOnly = true;
            DataGridColSRNo.Width = 125;
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Record ID";
            idDataGridViewTextBoxColumn.MinimumWidth = 6;
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // categoryDataGridViewTextBoxColumn
            // 
            categoryDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            categoryDataGridViewTextBoxColumn.DataPropertyName = "Category";
            categoryDataGridViewTextBoxColumn.HeaderText = "Category";
            categoryDataGridViewTextBoxColumn.MinimumWidth = 6;
            categoryDataGridViewTextBoxColumn.Name = "categoryDataGridViewTextBoxColumn";
            categoryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            amountDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            amountDataGridViewTextBoxColumn.MinimumWidth = 6;
            amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            amountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            dateDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            dateDataGridViewTextBoxColumn.HeaderText = "Date";
            dateDataGridViewTextBoxColumn.MinimumWidth = 6;
            dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            dateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            typeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            typeDataGridViewTextBoxColumn.HeaderText = "Type";
            typeDataGridViewTextBoxColumn.MinimumWidth = 6;
            typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            typeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // transactionBindingSource
            // 
            transactionBindingSource.DataSource = typeof(Model.ClsTransaction);
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 6;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.80769253F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.5961533F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.5961533F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.5961533F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.5961533F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.80769253F));
            tableLayoutPanel1.Controls.Add(button2, 0, 0);
            tableLayoutPanel1.Controls.Add(BtnRefresh, 1, 1);
            tableLayoutPanel1.Controls.Add(BtnAdd, 2, 1);
            tableLayoutPanel1.Controls.Add(BtnUpdate, 3, 1);
            tableLayoutPanel1.Controls.Add(BtnDelete, 4, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 390);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 95F));
            tableLayoutPanel1.Size = new Size(1076, 66);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Left;
            button2.Location = new Point(3, 3);
            button2.Name = "button2";
            button2.Size = new Size(45, 1);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // BtnRefresh
            // 
            BtnRefresh.Anchor = AnchorStyles.None;
            BtnRefresh.Location = new Point(97, 14);
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new Size(150, 40);
            BtnRefresh.TabIndex = 2;
            BtnRefresh.Text = "Refresh/Read";
            BtnRefresh.UseVisualStyleBackColor = true;
            BtnRefresh.Click += BtnRefresh_Click;
            // 
            // BtnAdd
            // 
            BtnAdd.Anchor = AnchorStyles.None;
            BtnAdd.Location = new Point(340, 14);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(150, 40);
            BtnAdd.TabIndex = 3;
            BtnAdd.Text = "Add";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // BtnUpdate
            // 
            BtnUpdate.Anchor = AnchorStyles.None;
            BtnUpdate.Location = new Point(583, 14);
            BtnUpdate.Name = "BtnUpdate";
            BtnUpdate.Size = new Size(150, 40);
            BtnUpdate.TabIndex = 4;
            BtnUpdate.Text = "Edit/Update";
            BtnUpdate.UseVisualStyleBackColor = true;
            // 
            // BtnDelete
            // 
            BtnDelete.Anchor = AnchorStyles.None;
            BtnDelete.Location = new Point(826, 14);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(150, 40);
            BtnDelete.TabIndex = 5;
            BtnDelete.Text = "Delete/Remove";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // transactionBindingSource1
            // 
            transactionBindingSource1.DataSource = typeof(Model.ClsTransaction);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1082, 553);
            Controls.Add(tableLayoutOuter);
            Controls.Add(statusStripMain);
            Controls.Add(menuStripMain);
            MainMenuStrip = menuStripMain;
            MinimumSize = new Size(1100, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Personal Finance Tracker";
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            tableLayoutOuter.ResumeLayout(false);
            tableLayoutPanelInner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewMain).EndInit();
            ((System.ComponentModel.ISupportInitialize)transactionBindingSource).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)transactionBindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var SF = new SelectData())
            {

                if (SF != null)
                {
                   
                    if (SF.ShowDialog(this) == DialogResult.OK)
                    {
                        // update 
                    
                    }
                }
            }
        }

        #endregion

        private MenuStrip menuStripMain;
        private ToolStripMenuItem toolStripMenuItemFIle;
        private ToolStripMenuItem toolStripMenuItemImport;
        private ToolStripMenuItem toolStripMenuItemExport;
        private ToolStripMenuItem toolStripMenuItemFilter;
        private ToolStripMenuItem IncomeToolStripMenuItem;
        private ToolStripMenuItem expenseToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel toolStripStatusLabelDateRange;
        private ToolStripStatusLabel toolStripStatusLabelNumSRec;
        private ToolStripStatusLabel toolStripStatusLabelCurrDate;
        private ToolStripStatusLabel toolStripStatusLblCurrVal;
        private ToolStripStatusLabel toolStripStatusLabelDateRVal;
        private ToolStripStatusLabel toolStripStatusLabelSpacer;
        private ToolStripStatusLabel toolStripStatusLabelNumRecVal;
        private ToolStripStatusLabel toolStripStatusLabelSpacer1;
        private TableLayoutPanel tableLayoutOuter;
        private TableLayoutPanel tableLayoutPanelInner;
        private DataGridView dataGridViewMain;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button2;
        private Button BtnRefresh;
        private Button BtnAdd;
        private Button BtnUpdate;
        private Button BtnDelete;
        private ToolStripMenuItem viewSummaryToolStripMenuItem;
        private BindingSource transactionBindingSource;
        private BindingSource transactionBindingSource1;
        private DataGridViewTextBoxColumn DataGridColSRNo;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn categoryDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
    }
}
