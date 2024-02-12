namespace VacationImageSorter
{
    partial class FolderSelecterForm
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
            srcLbl = new Label();
            destLbl = new Label();
            destSelectLbl = new Label();
            srcSelectLbl = new Label();
            sortBtn = new Button();
            SuspendLayout();
            // 
            // srcLbl
            // 
            srcLbl.AutoSize = true;
            srcLbl.Font = new Font("Segoe UI", 15F);
            srcLbl.Location = new Point(12, 9);
            srcLbl.Name = "srcLbl";
            srcLbl.Size = new Size(133, 28);
            srcLbl.TabIndex = 0;
            srcLbl.Text = "Source Folder";
            // 
            // destLbl
            // 
            destLbl.AutoSize = true;
            destLbl.Font = new Font("Segoe UI", 15F);
            destLbl.Location = new Point(12, 58);
            destLbl.Name = "destLbl";
            destLbl.Size = new Size(173, 28);
            destLbl.TabIndex = 1;
            destLbl.Text = "Destination Folder";
            // 
            // destSelectLbl
            // 
            destSelectLbl.BackColor = SystemColors.ControlDark;
            destSelectLbl.Font = new Font("Segoe UI", 15F);
            destSelectLbl.Location = new Point(191, 58);
            destSelectLbl.Name = "destSelectLbl";
            destSelectLbl.Size = new Size(400, 28);
            destSelectLbl.TabIndex = 2;
            destSelectLbl.Text = "Select";
            destSelectLbl.Click += destSelectLbl_Click;
            // 
            // srcSelectLbl
            // 
            srcSelectLbl.BackColor = SystemColors.ControlDark;
            srcSelectLbl.Font = new Font("Segoe UI", 15F);
            srcSelectLbl.Location = new Point(191, 9);
            srcSelectLbl.Name = "srcSelectLbl";
            srcSelectLbl.Size = new Size(400, 28);
            srcSelectLbl.TabIndex = 3;
            srcSelectLbl.Text = "Select";
            srcSelectLbl.Click += srcSelectLbl_Click;
            // 
            // sortBtn
            // 
            sortBtn.Location = new Point(601, 9);
            sortBtn.Name = "sortBtn";
            sortBtn.Size = new Size(77, 77);
            sortBtn.TabIndex = 4;
            sortBtn.Text = "Sort";
            sortBtn.UseVisualStyleBackColor = true;
            sortBtn.Click += sortBtn_Click;
            // 
            // FolderSelecterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 93);
            Controls.Add(sortBtn);
            Controls.Add(srcSelectLbl);
            Controls.Add(destSelectLbl);
            Controls.Add(destLbl);
            Controls.Add(srcLbl);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FolderSelecterForm";
            Text = "HolidaySnapSort";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label srcLbl;
        private Label destLbl;
        private Label destSelectLbl;
        private Label srcSelectLbl;
        private Button sortBtn;
    }
}