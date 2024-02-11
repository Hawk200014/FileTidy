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
            srcTB.Text = SelectFolder("Select Source Folder");
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
    }
}