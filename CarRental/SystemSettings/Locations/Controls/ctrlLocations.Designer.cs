namespace CarRental.SystemSettings.Locations.Controls
{
    partial class ctrlLocations
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlState = new System.Windows.Forms.Panel();
            this.lblDescriptionState = new System.Windows.Forms.Label();
            this.lblTitleState = new System.Windows.Forms.Label();
            this.dgvListLocations = new System.Windows.Forms.DataGridView();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlContainstxtAndcb = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cbYesOrNo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.lblTotalPages = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.cbPageNumber = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListLocations)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlContainstxtAndcb.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.btnAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnEdit);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Controls.Add(this.btnExport);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1125, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(123, 545);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::CarRental.Properties.Resources.add_32;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.Location = new System.Drawing.Point(5, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(113, 50);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnAdd, "إضافة موقع جديد إلى النظام");
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Image = global::CarRental.Properties.Resources.edit_32;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEdit.Location = new System.Drawing.Point(5, 59);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(113, 50);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "تعديل";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnEdit, "تعديل بيانات الموقع المحدد");
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::CarRental.Properties.Resources.delete_32;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.Location = new System.Drawing.Point(5, 115);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(113, 50);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "حذف";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnDelete, "حذف الموقع المحدد من النظام");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Image = global::CarRental.Properties.Resources.excelAll_32;
            this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.Location = new System.Drawing.Point(5, 171);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(113, 50);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "تصدير";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.btnExport, "تصدير البيانات الحالية إلى ملف Excel");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.btnRefresh);
            this.pnlMain.Controls.Add(this.pnlState);
            this.pnlMain.Controls.Add(this.dgvListLocations);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1125, 545);
            this.pnlMain.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Image = global::CarRental.Properties.Resources.Refresh;
            this.btnRefresh.Location = new System.Drawing.Point(1069, 438);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(50, 50);
            this.btnRefresh.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnRefresh, "إعادة تحميل البيانات");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlState
            // 
            this.pnlState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlState.Controls.Add(this.lblDescriptionState);
            this.pnlState.Controls.Add(this.lblTitleState);
            this.pnlState.Location = new System.Drawing.Point(230, 152);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(664, 168);
            this.pnlState.TabIndex = 1;
            this.pnlState.Visible = false;
            // 
            // lblDescriptionState
            // 
            this.lblDescriptionState.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescriptionState.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescriptionState.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescriptionState.Location = new System.Drawing.Point(0, 60);
            this.lblDescriptionState.Name = "lblDescriptionState";
            this.lblDescriptionState.Size = new System.Drawing.Size(664, 60);
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
            this.lblTitleState.Size = new System.Drawing.Size(664, 60);
            this.lblTitleState.TabIndex = 2;
            this.lblTitleState.Text = "لاتوجد بيانات";
            this.lblTitleState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvListLocations
            // 
            this.dgvListLocations.AllowUserToAddRows = false;
            this.dgvListLocations.AllowUserToDeleteRows = false;
            this.dgvListLocations.AllowUserToOrderColumns = true;
            this.dgvListLocations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListLocations.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvListLocations.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListLocations.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvListLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListLocations.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvListLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListLocations.EnableHeadersVisualStyles = false;
            this.dgvListLocations.Location = new System.Drawing.Point(0, 0);
            this.dgvListLocations.Name = "dgvListLocations";
            this.dgvListLocations.ReadOnly = true;
            this.dgvListLocations.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvListLocations.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListLocations.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvListLocations.RowHeadersWidth = 62;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListLocations.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvListLocations.RowTemplate.Height = 28;
            this.dgvListLocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListLocations.Size = new System.Drawing.Size(1125, 545);
            this.dgvListLocations.TabIndex = 0;
            this.toolTip1.SetToolTip(this.dgvListLocations, "اضغط بزر الفأرة الأيمن لمزيد من الخيارات");
            // 
            // pnlSearch
            // 
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.pnlContainstxtAndcb);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.cbFilter);
            this.pnlSearch.Controls.Add(this.lblTotalPages);
            this.pnlSearch.Controls.Add(this.btnPrevious);
            this.pnlSearch.Controls.Add(this.cbPageNumber);
            this.pnlSearch.Controls.Add(this.btnNext);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSearch.Location = new System.Drawing.Point(0, 493);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1125, 52);
            this.pnlSearch.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = global::CarRental.Properties.Resources.search_32;
            this.btnSearch.Location = new System.Drawing.Point(729, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 50);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "    ";
            this.toolTip1.SetToolTip(this.btnSearch, "تنفيذ البحث حسب الفلتر المحدد");
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlContainstxtAndcb
            // 
            this.pnlContainstxtAndcb.Controls.Add(this.lblSearch);
            this.pnlContainstxtAndcb.Controls.Add(this.txtSearch);
            this.pnlContainstxtAndcb.Controls.Add(this.cbYesOrNo);
            this.pnlContainstxtAndcb.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlContainstxtAndcb.Location = new System.Drawing.Point(779, 0);
            this.pnlContainstxtAndcb.Name = "pnlContainstxtAndcb";
            this.pnlContainstxtAndcb.Size = new System.Drawing.Size(344, 50);
            this.pnlContainstxtAndcb.TabIndex = 5;
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.BackColor = System.Drawing.Color.White;
            this.lblSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lblSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lblSearch.Location = new System.Drawing.Point(219, 4);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(110, 38);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "بحث . . .";
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(0, 0);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(5);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(344, 50);
            this.txtSearch.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtSearch, "اكتب قيمة البحث هنا");
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // cbYesOrNo
            // 
            this.cbYesOrNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbYesOrNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYesOrNo.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.cbYesOrNo.FormattingEnabled = true;
            this.cbYesOrNo.Items.AddRange(new object[] {
            "نشط",
            "غير نشط"});
            this.cbYesOrNo.Location = new System.Drawing.Point(0, 0);
            this.cbYesOrNo.Name = "cbYesOrNo";
            this.cbYesOrNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbYesOrNo.Size = new System.Drawing.Size(344, 49);
            this.cbYesOrNo.TabIndex = 5;
            this.toolTip1.SetToolTip(this.cbYesOrNo, "اختر نعم أو لا لتصفية البيانات");
            this.cbYesOrNo.SelectedIndexChanged += new System.EventHandler(this.cbYesOrNo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(507, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 50);
            this.label1.TabIndex = 3;
            this.label1.Text = "ابحث حسب: ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbFilter
            // 
            this.cbFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Items.AddRange(new object[] {
            "المعرف",
            "الاسم",
            "العنوان",
            "الهاتف",
            "نشط ؟ "});
            this.cbFilter.Location = new System.Drawing.Point(267, 0);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbFilter.Size = new System.Drawing.Size(240, 49);
            this.cbFilter.TabIndex = 7;
            this.toolTip1.SetToolTip(this.cbFilter, "اختر نوع الفلتر للبحث");
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // lblTotalPages
            // 
            this.lblTotalPages.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTotalPages.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lblTotalPages.ForeColor = System.Drawing.Color.Red;
            this.lblTotalPages.Location = new System.Drawing.Point(193, 0);
            this.lblTotalPages.Name = "lblTotalPages";
            this.lblTotalPages.Size = new System.Drawing.Size(74, 50);
            this.lblTotalPages.TabIndex = 1;
            this.lblTotalPages.Text = "??";
            this.lblTotalPages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTotalPages, "عدد الصفحات");
            // 
            // btnPrevious
            // 
            this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Image = global::CarRental.Properties.Resources.previous_32;
            this.btnPrevious.Location = new System.Drawing.Point(143, 0);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(50, 50);
            this.btnPrevious.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnPrevious, "الانتقال إلى الصفحة السابقة");
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // cbPageNumber
            // 
            this.cbPageNumber.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbPageNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPageNumber.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.cbPageNumber.FormattingEnabled = true;
            this.cbPageNumber.Location = new System.Drawing.Point(50, 0);
            this.cbPageNumber.MaxDropDownItems = 100;
            this.cbPageNumber.Name = "cbPageNumber";
            this.cbPageNumber.Size = new System.Drawing.Size(93, 53);
            this.cbPageNumber.TabIndex = 9;
            this.toolTip1.SetToolTip(this.cbPageNumber, "اختر رقم الصفحة");
            this.cbPageNumber.SelectedIndexChanged += new System.EventHandler(this.cbPageNumber_SelectedIndexChanged);
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Image = global::CarRental.Properties.Resources.next_32;
            this.btnNext.Location = new System.Drawing.Point(0, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(50, 50);
            this.btnNext.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnNext, "الانتقال إلى الصفحة التالية");
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 200;
            this.toolTip1.ShowAlways = true;
            // 
            // ctrlLocations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ctrlLocations";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1248, 545);
            this.Load += new System.EventHandler(this.ctrlLocations_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlState.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListLocations)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlContainstxtAndcb.ResumeLayout(false);
            this.pnlContainstxtAndcb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dgvListLocations;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbPageNumber;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblTotalPages;
        private System.Windows.Forms.Panel pnlState;
        private System.Windows.Forms.Label lblDescriptionState;
        private System.Windows.Forms.Label lblTitleState;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlContainstxtAndcb;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cbYesOrNo;
    }
}
