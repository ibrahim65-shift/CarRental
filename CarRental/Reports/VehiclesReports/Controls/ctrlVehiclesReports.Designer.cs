namespace CarRental.Reports.VehiclesReports.Controls
{
    partial class ctrlVehiclesReports
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbReportType = new System.Windows.Forms.ComboBox();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.labelTop = new System.Windows.Forms.Label();
            this.numericUpDownTop = new System.Windows.Forms.NumericUpDown();
            this.labelFromDate = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.labelToDate = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.dgvReportViewer = new System.Windows.Forms.DataGridView();
            this.pnlState = new System.Windows.Forms.Panel();
            this.lblDescriptionState = new System.Windows.Forms.Label();
            this.lblTitleState = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportViewer)).BeginInit();
            this.pnlState.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.cbReportType);
            this.flowLayoutPanel1.Controls.Add(this.btnGenerateReport);
            this.flowLayoutPanel1.Controls.Add(this.labelTop);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDownTop);
            this.flowLayoutPanel1.Controls.Add(this.labelFromDate);
            this.flowLayoutPanel1.Controls.Add(this.dtpFrom);
            this.flowLayoutPanel1.Controls.Add(this.labelToDate);
            this.flowLayoutPanel1.Controls.Add(this.dtpTo);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1837, 61);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.reportType_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(1630, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "نوع التقرير:    ";
            // 
            // cbReportType
            // 
            this.cbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReportType.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReportType.FormattingEnabled = true;
            this.cbReportType.Items.AddRange(new object[] {
            "جميع المركبات",
            "المركبات الأكثر تأجيراً",
            "صيانة المركبات"});
            this.cbReportType.Location = new System.Drawing.Point(1343, 3);
            this.cbReportType.Name = "cbReportType";
            this.cbReportType.Size = new System.Drawing.Size(281, 46);
            this.cbReportType.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cbReportType, "اختر نوع تقرير المركبات الذي تريده");
            this.cbReportType.SelectedIndexChanged += new System.EventHandler(this.cbReportType_SelectedIndexChanged);
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateReport.Image = global::CarRental.Properties.Resources.report_32;
            this.btnGenerateReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGenerateReport.Location = new System.Drawing.Point(1150, 3);
            this.btnGenerateReport.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(175, 50);
            this.btnGenerateReport.TabIndex = 7;
            this.btnGenerateReport.Text = "توليد التقرير";
            this.btnGenerateReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnGenerateReport, "اضغط على الزر لتوليد التقرير الذي اخترته");
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTop.Image = global::CarRental.Properties.Resources.vehicleCategory_32;
            this.labelTop.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelTop.Location = new System.Drawing.Point(1024, 0);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(120, 45);
            this.labelTop.TabIndex = 4;
            this.labelTop.Text = "أكثر :    ";
            this.labelTop.Visible = false;
            // 
            // numericUpDownTop
            // 
            this.numericUpDownTop.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownTop.Location = new System.Drawing.Point(915, 3);
            this.numericUpDownTop.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new System.Drawing.Size(103, 45);
            this.numericUpDownTop.TabIndex = 8;
            this.numericUpDownTop.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTop.Visible = false;
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFromDate.Image = global::CarRental.Properties.Resources.startDate_32;
            this.labelFromDate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelFromDate.Location = new System.Drawing.Point(732, 0);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(177, 45);
            this.labelFromDate.TabIndex = 4;
            this.labelFromDate.Text = "من تاريخ:    ";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Location = new System.Drawing.Point(452, 3);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(274, 45);
            this.dtpFrom.TabIndex = 1;
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelToDate.Image = global::CarRental.Properties.Resources.expiryDate_32;
            this.labelToDate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelToDate.Location = new System.Drawing.Point(263, 0);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(183, 45);
            this.labelToDate.TabIndex = 6;
            this.labelToDate.Text = "إلى تاريخ:    ";
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Location = new System.Drawing.Point(1560, 59);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(274, 45);
            this.dtpTo.TabIndex = 5;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnExportExcel);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 541);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1837, 66);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportExcel.Image = global::CarRental.Properties.Resources.excel_32;
            this.btnExportExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportExcel.Location = new System.Drawing.Point(1630, 3);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(204, 60);
            this.btnExportExcel.TabIndex = 0;
            this.btnExportExcel.Text = "تصدير إكسل";
            this.btnExportExcel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnExportExcel, "اضغط على الزر لتصدير البيانات على شكل إكسل");
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // dgvReportViewer
            // 
            this.dgvReportViewer.AllowUserToAddRows = false;
            this.dgvReportViewer.AllowUserToDeleteRows = false;
            this.dgvReportViewer.AllowUserToOrderColumns = true;
            this.dgvReportViewer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReportViewer.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReportViewer.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReportViewer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReportViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReportViewer.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReportViewer.EnableHeadersVisualStyles = false;
            this.dgvReportViewer.Location = new System.Drawing.Point(0, 61);
            this.dgvReportViewer.Name = "dgvReportViewer";
            this.dgvReportViewer.ReadOnly = true;
            this.dgvReportViewer.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReportViewer.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReportViewer.RowHeadersWidth = 62;
            this.dgvReportViewer.RowTemplate.Height = 28;
            this.dgvReportViewer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReportViewer.Size = new System.Drawing.Size(1837, 480);
            this.dgvReportViewer.TabIndex = 0;
            // 
            // pnlState
            // 
            this.pnlState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlState.Controls.Add(this.lblDescriptionState);
            this.pnlState.Controls.Add(this.lblTitleState);
            this.pnlState.Location = new System.Drawing.Point(418, 219);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(1001, 202);
            this.pnlState.TabIndex = 3;
            // 
            // lblDescriptionState
            // 
            this.lblDescriptionState.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescriptionState.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescriptionState.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescriptionState.Location = new System.Drawing.Point(0, 81);
            this.lblDescriptionState.Name = "lblDescriptionState";
            this.lblDescriptionState.Size = new System.Drawing.Size(1001, 74);
            this.lblDescriptionState.TabIndex = 3;
            this.lblDescriptionState.Text = "لاتوجد بيانات";
            this.lblDescriptionState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitleState
            // 
            this.lblTitleState.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleState.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleState.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleState.Location = new System.Drawing.Point(0, 0);
            this.lblTitleState.Name = "lblTitleState";
            this.lblTitleState.Size = new System.Drawing.Size(1001, 81);
            this.lblTitleState.TabIndex = 2;
            this.lblTitleState.Text = "لاتوجد بيانات";
            this.lblTitleState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 200;
            this.toolTip1.ShowAlways = true;
            // 
            // ctrlVehiclesReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlState);
            this.Controls.Add(this.dgvReportViewer);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ctrlVehiclesReports";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1837, 607);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportViewer)).EndInit();
            this.pnlState.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbReportType;
        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.DataGridView dgvReportViewer;
        private System.Windows.Forms.Panel pnlState;
        private System.Windows.Forms.Label lblDescriptionState;
        private System.Windows.Forms.Label lblTitleState;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown numericUpDownTop;
        private System.Windows.Forms.Label labelTop;
    }
}
