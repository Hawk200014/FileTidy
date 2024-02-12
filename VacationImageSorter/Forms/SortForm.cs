using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VacationImageSorter.Forms
{
    public partial class SortForm : Form
    {

        private readonly string _sourceFolder;
        private readonly string _destinationFolder;

        public SortForm(string sourceFolder, string destinationFolder)
        {
            InitializeComponent();
            _sourceFolder = sourceFolder;
            _destinationFolder = destinationFolder;
        }

        public async Task LoadImagesAsync()
        {
            Task sourceImageLoader = LoadSourceImagesAsync();
            Task destImageLoader = 
            
        }

        private void SortForm_Load(object sender, EventArgs e)
        {

        }

        private Task<string[]> LoadSourceImagesAsync()
        {

        }

        private Task<string[]> LoadDestinationImagesAsync()
        {

        }
    }
}
