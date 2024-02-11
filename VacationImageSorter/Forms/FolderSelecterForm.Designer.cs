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
            srcTB = new TextBox();
            destTB = new TextBox();
            SuspendLayout();
            // 
            // srcLbl
            // 
            srcLbl.AutoSize = true;
            srcLbl.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            srcLbl.Location = new Point(12, 9);
            srcLbl.Name = "srcLbl";
            srcLbl.Size = new Size(133, 28);
            srcLbl.TabIndex = 0;
            srcLbl.Text = "Source Folder";
            // 
            // destLbl
            // 
            destLbl.AutoSize = true;
            destLbl.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            destLbl.Location = new Point(12, 58);
            destLbl.Name = "destLbl";
            destLbl.Size = new Size(173, 28);
            destLbl.TabIndex = 1;
            destLbl.Text = "Destination Folder";
            // 
            // srcTB
            // 
            srcTB.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            srcTB.Location = new Point(191, 6);
            srcTB.Name = "srcTB";
            srcTB.PlaceholderText = "Select";
            srcTB.ReadOnly = true;
            srcTB.Size = new Size(480, 34);
            srcTB.TabIndex = 1;
            srcTB.Click += srcTB_Click;
            // 
            // destTB
            // 
            destTB.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            destTB.Location = new Point(191, 55);
            destTB.Name = "destTB";
            destTB.PlaceholderText = "Select";
            destTB.ReadOnly = true;
            destTB.Size = new Size(480, 34);
            destTB.TabIndex = 2;
            // 
            // FolderSelecterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(688, 111);
            Controls.Add(destTB);
            Controls.Add(srcTB);
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
        private TextBox srcTB;
        private TextBox destTB;
    }
}