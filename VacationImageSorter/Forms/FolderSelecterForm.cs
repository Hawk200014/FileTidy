namespace VacationImageSorter
{
    public partial class FolderSelecterForm : Form
    {
        public FolderSelecterForm()
        {
            InitializeComponent();
        }

        private void srcTB_Click(object sender, EventArgs e)
        {

        }

        private string SelectFolder(string description)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = description;
                // Set the root folder to start browsing from (optional)
                //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;

                // Show the dialog and check if the user selected a folder
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    return folderBrowserDialog.SelectedPath;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void srcSelectLbl_Click(object sender, EventArgs e)
        {
            srcSelectLbl.Text = SelectFolder("Select Source Folder");
        }

        private void destSelectLbl_Click(object sender, EventArgs e)
        {
            destSelectLbl.Text = SelectFolder("Select Destination Folder");
        }

        private void sortBtn_Click(object sender, EventArgs e)
        {
            string message = "";
            if (srcSelectLbl.Text != "Select" && destSelectLbl.Text != "Select")
            {

            }
            else if (srcSelectLbl.Text == "Select")
            {
                message = "Select a source folder.";
            }
            else if (destSelectLbl.Text == "Select")
            {
                message = "Select a destination folder";
            }
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Error in input detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}