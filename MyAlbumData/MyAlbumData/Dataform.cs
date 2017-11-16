using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Manning.MyPhotoAlbum;
using Manning.MyPhotoControls;

namespace MyAlbumData
{
    public partial class Dataform : Form
    {
        private AlbumManager _manager;
        internal AlbumManager Manager
        {
            get { return _manager; }
        }

        public Dataform()
        {
            InitializeComponent();
            SetupGrid();
        }
        protected override void OnLoad(EventArgs e)
        {
            Version ver = new Version(Application.ProductVersion);
            Text = String.Format("MyAlbumData {0:#}.{1:#}", ver.Major, ver.Minor);

            _manager = new AlbumManager();

            cmbAlbum.DataSource = Directory.GetFiles(AlbumManager.DefaultPath, "*.abm");
            OpenAlbum();
            SetDataSources();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbAlbum_SelectionChangeCommitted(object sender, EventArgs e)
        {
            OpenAlbum();
            SetDataSources();
        }

        private void OpenAlbum()
        {
            string albumPath = cmbAlbum.SelectedItem.ToString();
            if (Manager.FullName == albumPath)
                return;
            if (CloseAlbum() == false)
                return;
            try
            {
                _manager = new AlbumManager(albumPath);
            }
            catch (AlbumStorageException aex)
            {
                _manager = new AlbumManager();
                MessageBox.Show("Unable to open album - " + aex.Message);
            }

        }
        private bool CloseAlbum()
        {
            if (Manager.Album != null)
                Manager.Album.Dispose();

            return true;
        }

        private void SetDataSources()
        {
            gridPhotoAlbum.DataSource = Manager.Album;
        }
        private void SetupGrid()
        {
            gridPhotoAlbum.SuspendLayout();
            gridPhotoAlbum.AutoGenerateColumns = false;
            gridPhotoAlbum.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            DataGridViewImageColumn thumbCol = new DataGridViewImageColumn();
            thumbCol.DataPropertyName = "Image";
            thumbCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            thumbCol.Name = "Image";
            thumbCol.Width = 50;

            DataGridViewColumn captionCol = new DataGridViewTextBoxColumn();
            captionCol.DataPropertyName = "Caption";
            captionCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            captionCol.Name = "Caption";

            DataGridViewColumn takenCol = new DataGridViewTextBoxColumn();
            takenCol.DataPropertyName = "DateTaken";
            takenCol.Name = "Date Taken";

            DataGridViewColumn pgCol = new DataGridViewTextBoxColumn();
            pgCol.DataPropertyName = "Photographer";
            pgCol.Name = "Date Taken";

            DataGridViewTextBoxColumn fileCol = new DataGridViewTextBoxColumn();
            fileCol.DataPropertyName = "FileName";
            fileCol.Name = "File Name";

            gridPhotoAlbum.Columns.AddRange(new DataGridViewColumn[] {
                thumbCol,
                captionCol,
                takenCol,
                pgCol,
                fileCol});

            gridPhotoAlbum.ResumeLayout();

        }
    }
}
