namespace CarRental.Attachments.Controls
{
    partial class ctrlRelatedAttachments
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAttachmentsOwner = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAttachmentsCount = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvListAttachments = new System.Windows.Forms.DataGridView();
            this.pnlState = new System.Windows.Forms.Panel();
            this.lblDescriptionState = new System.Windows.Forms.Label();
            this.lblTitleState = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListAttachments)).BeginInit();
            this.pnlState.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.lblAttachmentsOwner);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.lblAttachmentsCount);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1233, 53);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::CarRental.Properties.Resources.attach_32;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(900, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 45);
            this.label1.TabIndex = 4;
            this.label1.Text = "المرفقات الخاصة بي:    ";
            // 
            // lblAttachmentsOwner
            // 
            this.lblAttachmentsOwner.AutoSize = true;
            this.lblAttachmentsOwner.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttachmentsOwner.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAttachmentsOwner.Location = new System.Drawing.Point(711, 0);
            this.lblAttachmentsOwner.Margin = new System.Windows.Forms.Padding(40, 0, 3, 0);
            this.lblAttachmentsOwner.Name = "lblAttachmentsOwner";
            this.lblAttachmentsOwner.Size = new System.Drawing.Size(146, 38);
            this.lblAttachmentsOwner.TabIndex = 5;
            this.lblAttachmentsOwner.Text = "غير معروف";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::CarRental.Properties.Resources.attachCount_32;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(68, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(400, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 45);
            this.label2.TabIndex = 6;
            this.label2.Text = "عدد المرفقات:    ";
            // 
            // lblAttachmentsCount
            // 
            this.lblAttachmentsCount.AutoSize = true;
            this.lblAttachmentsCount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttachmentsCount.ForeColor = System.Drawing.Color.Red;
            this.lblAttachmentsCount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAttachmentsCount.Location = new System.Drawing.Point(1047, 45);
            this.lblAttachmentsCount.Margin = new System.Windows.Forms.Padding(40, 0, 3, 0);
            this.lblAttachmentsCount.Name = "lblAttachmentsCount";
            this.lblAttachmentsCount.Size = new System.Drawing.Size(146, 38);
            this.lblAttachmentsCount.TabIndex = 7;
            this.lblAttachmentsCount.Text = "غير معروف";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnAdd);
            this.flowLayoutPanel2.Controls.Add(this.btnEdit);
            this.flowLayoutPanel2.Controls.Add(this.btnDelete);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 590);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1233, 56);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::CarRental.Properties.Resources.add_32;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.Location = new System.Drawing.Point(1117, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(113, 50);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "إضافة";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Image = global::CarRental.Properties.Resources.edit_32;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEdit.Location = new System.Drawing.Point(998, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(113, 50);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "تعديل";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::CarRental.Properties.Resources.delete_32;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.Location = new System.Drawing.Point(879, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(113, 50);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "حذف";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvListAttachments
            // 
            this.dgvListAttachments.AllowUserToAddRows = false;
            this.dgvListAttachments.AllowUserToDeleteRows = false;
            this.dgvListAttachments.AllowUserToOrderColumns = true;
            this.dgvListAttachments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListAttachments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvListAttachments.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListAttachments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvListAttachments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListAttachments.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvListAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListAttachments.EnableHeadersVisualStyles = false;
            this.dgvListAttachments.Location = new System.Drawing.Point(0, 53);
            this.dgvListAttachments.Name = "dgvListAttachments";
            this.dgvListAttachments.ReadOnly = true;
            this.dgvListAttachments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvListAttachments.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListAttachments.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvListAttachments.RowHeadersWidth = 62;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListAttachments.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvListAttachments.RowTemplate.Height = 28;
            this.dgvListAttachments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListAttachments.Size = new System.Drawing.Size(1233, 537);
            this.dgvListAttachments.TabIndex = 2;
            // 
            // pnlState
            // 
            this.pnlState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlState.Controls.Add(this.lblDescriptionState);
            this.pnlState.Controls.Add(this.lblTitleState);
            this.pnlState.Location = new System.Drawing.Point(146, 239);
            this.pnlState.Name = "pnlState";
            this.pnlState.Size = new System.Drawing.Size(941, 168);
            this.pnlState.TabIndex = 3;
            this.pnlState.Visible = false;
            // 
            // lblDescriptionState
            // 
            this.lblDescriptionState.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescriptionState.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescriptionState.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDescriptionState.Location = new System.Drawing.Point(0, 60);
            this.lblDescriptionState.Name = "lblDescriptionState";
            this.lblDescriptionState.Size = new System.Drawing.Size(941, 60);
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
            this.lblTitleState.Size = new System.Drawing.Size(941, 60);
            this.lblTitleState.TabIndex = 2;
            this.lblTitleState.Text = "لاتوجد بيانات";
            this.lblTitleState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctrlRelatedAttachments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlState);
            this.Controls.Add(this.dgvListAttachments);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ctrlRelatedAttachments";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1233, 646);
            this.Load += new System.EventHandler(this.ctrlRelatedAttachments_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListAttachments)).EndInit();
            this.pnlState.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvListAttachments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAttachmentsOwner;
        private System.Windows.Forms.Panel pnlState;
        private System.Windows.Forms.Label lblDescriptionState;
        private System.Windows.Forms.Label lblTitleState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAttachmentsCount;
    }
}
