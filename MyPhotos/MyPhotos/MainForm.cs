﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manning.MyPhotoAlbum;
using Mainning.MyPhotoControls;

namespace MyPhotos
{
    public partial class MainForm : Form
    {
        private AlbumManager _manager;
        private AlbumManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                _manager = value;
            }
        }

        private PixelDialog _dlgPixel = null;
        private PixelDialog PixelForm
        {
            get { return _dlgPixel; }
            set { _dlgPixel = value; }
        }

        public MainForm()
        {

            InitializeComponent();
            NewAlbum();
        }
        private void NewAlbum()
        {
            if (Manager == null || SaveAndCloseAlbum())
            {
                Manager = new AlbumManager();
                DisplayAlbum();
            }
        }
        private void DisplayAlbum()
        {
            pbxPhoto.Image = Manager.CurrentImage;
            SetTitleBar();
            SetStatusStrip(null);

            Point p = pbxPhoto.PointToClient(Form.MousePosition);
            UpdatePixelDialog(p.X, p.Y);
        }

        private void SetTitleBar()
        {
             Version ver = new Version(Application.ProductVersion);
             string name = Manager.FullName;
            Text = String.Format("{2} - MyPhotos {0:0}.{1:0}",
                                   ver.Major, ver.Minor,
                                   String.IsNullOrEmpty(name) ? "untitled": name);

        }

        private void mnuFileLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Photo";
            dlg.Filter = "jpg files (*.jpg)|*.jpg"
                + "|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pbxPhoto.Image = new Bitmap(dlg.OpenFile());
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Unable to laod file:" + ex.Message);
                    pbxPhoto.Image = null;
                }
                SetStatusStrip(dlg.FileName);
            }
            dlg.Dispose();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuImage_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            processImageClick(e);
        }
        private void processImageClick(ToolStripItemClickedEventArgs e)

        {
            ToolStripItem item = e.ClickedItem;
            string enumVal = item.Tag as string;
            if (enumVal != null)
            {
                pbxPhoto.SizeMode = (PictureBoxSizeMode)Enum.Parse(typeof(PictureBoxSizeMode), enumVal);
            }
        }

        private void mnuImage_DropDownOpening(object sender, EventArgs e)
        {
            ProcessImageOpening(sender as ToolStripDropDownItem);
        }
        private void ProcessImageOpening(ToolStripDropDownItem parent)
        {
            if (parent != null)
            {
                string enumVal = pbxPhoto.SizeMode.ToString();
                foreach (ToolStripMenuItem item in parent.DropDownItems)
                {
                    item.Enabled = (pbxPhoto.Image != null);
                    item.Checked = item.Tag.Equals(enumVal);
                }
            }
        }
        private void SetStatusStrip(string path)
        {
            if (pbxPhoto.Image != null)
            {
                sttinfo.Text = Manager.Current.Caption;
                sttImageSize.Text = string.Format("{0:#}x{1:#}",
                                            pbxPhoto.Image.Width,
                                            pbxPhoto.Image.Height);

                sttAbumPos.Text = String.Format("{0:0}/{1:0}",
                                               Manager.Index + 1,
                                               Manager.Album.Count);
            }
            else
            {
                sttinfo.Text = null;
                sttImageSize.Text = null;
                sttAbumPos.Text = null;
            }
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            NewAlbum();
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Album";
            dlg.Filter = "Album files (*.abm)|*.abm"
                + "|All files (*.*)|*.*";
            dlg.InitialDirectory = AlbumManager.DefaultPath;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {

                string path = dlg.FileName;

                if (!SaveAndCloseAlbum())
                    return;


                try
                {
                    //Open the new album
                    //TODO: handle invalid album file
                    Manager = new AlbumManager(path);
                }
                catch (AlbumStorageException aex)
                {
                    string msg = String.Format("Unable to open album file {0}\n({1})",
                                                   path, aex.Message);
                    MessageBox.Show(msg, "Unable to open");
                    Manager = new AlbumManager();

                }
                DisplayAlbum();
            }
            dlg.Dispose();
        }

        private void SaveAlbum(string name)
        {
            try
            {
                Manager.Save(name, true);
            }
            catch (AlbumStorageException aex)
            {
                string msg = String.Format("Unable to save album {0} ({1})\n\n"
                                           + "Do you wish to save the album"
                                           + "under a alternate name?",
                                           name, aex.Message);
                DialogResult result = MessageBox.Show(msg, "Unable to Save", MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                    SaveAlbum();
            }

        }

        private void SaveAlbum()
        {
            if (String.IsNullOrEmpty(Manager.FullName))
                SaveAsAlbum();
            else
            {
                //Save the Album under the existing name
                SaveAlbum(Manager.FullName);
            }
        }
        private void SaveAsAlbum()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Album";
            dlg.DefaultExt = "abm";
            dlg.Filter = "Album files (*.abm)|*.abm"
                + "|All files (*.*)|*.*";
            dlg.InitialDirectory = AlbumManager.DefaultPath;
            dlg.RestoreDirectory = true;
            if(dlg.ShowDialog()== DialogResult.OK)
            {
                SaveAlbum(dlg.FileName);
                //Update title bar to include new name
                SetTitleBar();
            }
            dlg.Dispose();
        }
        private bool SaveAndCloseAlbum()
        {
            if (Manager.Album.HasChanged)
            {
                string msg;
                if (string.IsNullOrEmpty(Manager.FullName))
                    msg = "Do you wish to save your changes?";
                else
                    msg = String.Format("Do you wish to save your changes to \n{0}?", Manager.FullName);
                DialogResult result = MessageBox.Show(this, msg, "Save changes?",
                                       MessageBoxButtons.YesNoCancel,
                                       MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    SaveAlbum();
                else if (result == DialogResult.Cancel)
                    return false;
            }

            if (Manager.Album != null)
                Manager.Album.Dispose();

            Manager = new AlbumManager();
            SetTitleBar();
            return true;
        }


        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            SaveAlbum();
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsAlbum();
        }

        private void mnuEditAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Add Photos";
            dlg.Multiselect = true;
            dlg.Filter = "Image Files(JPEG, GIF, BMP, etc.)|*jpg;*.jpeg;*.gif;*.bmp;*.tiff;*.png|" +
                         "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg |" +
                         "GIF files (*.gif)|*.gif|" +
                         "BMP files (*.bmp)|*.bmp|" +
                         "TIFF files (*.tif;*.tiff) |*.tif;*.tiff|" +
                         "PNG files (*.png )|*.png|" +
                         "All files (*.*)|*.* ";
            dlg.InitialDirectory = Environment.CurrentDirectory;

            if(dlg.ShowDialog()== DialogResult.OK)
            {
                string[] files = dlg.FileNames;

                int index = 0;
                foreach (string s in files)
                {
                    Photograph photo = new Photograph(s);

                    index = Manager.Album.IndexOf(photo);
                    if (index < 0)
                        Manager.Album.Add(photo);
                    else
                        photo.Dispose();
                }
                Manager.Index = Manager.Album.Count - 1;
            }
            dlg.Dispose();
            DisplayAlbum();

        }

        private void mnuEditRemove_Click(object sender, EventArgs e)
        {
            if(Manager.Album.Count > 0)
            {
                Manager.Album.RemoveAt(Manager.Index);
                DisplayAlbum();
            }
        }

        private void mnuNext_Click(object sender, EventArgs e)
        {
            if(Manager.Index < Manager.Album.Count -1)
            {
                Manager.Index++;
                DisplayAlbum();
            }
        }

        private void mnuPrevious_Click(object sender, EventArgs e)
        {
            if(Manager.Index >0)
            {
                Manager.Index--;
                DisplayAlbum();
            }
        }

        private void ctxMenuPhoto_Opening(object sender, CancelEventArgs e)
        {
            mnuNext.Enabled = (Manager.Index < Manager.Album.Count - 1);
            mnuPrevious.Enabled = (Manager.Index > 0);
            mnuPhotoProps.Enabled = (Manager.Current != null);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (SaveAndCloseAlbum() == false)
                e.Cancel = true;
            else
                e.Cancel = false;
            base.OnFormClosing(e);
        }

        private void mnuPixelData_Click(object sender, EventArgs e)
        {
            if (PixelForm == null || PixelForm.IsDisposed)
            {
                PixelForm = new PixelDialog();
                PixelForm.Owner = this;
            }

            PixelForm.Show();

            Point p = pbxPhoto.PointToClient(Form.MousePosition);
            UpdatePixelDialog(p.X, p.Y);

        }
        private void UpdatePixelDialog(int X, int Y)
        {
            if (PixelForm != null && PixelForm.Visible)
            {
                Bitmap bmp = Manager.CurrentImage;
                PixelForm.Text = (Manager.Current == null) ? "Pixel Data" : Manager.Current.Caption;

                if (bmp == null || !pbxPhoto.DisplayRectangle.Contains(X, Y))
                    PixelForm.ClearPixelData();
                else
                    PixelForm.UpdatePixelData(X, Y, bmp, pbxPhoto.DisplayRectangle,
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        pbxPhoto.SizeMode);
            }
        }

        private void pbxPhoto_MouseMove(object sender, MouseEventArgs e)
        {
            UpdatePixelDialog(e.X, e.Y);
        }

        private void mnuPhotoProps_Click(object sender, EventArgs e)
        {
            if (Manager.Current == null)
                return;

            using (PhotoEditDialog dlg = new PhotoEditDialog(Manager))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    DisplayAlbum();
            }
        }
    }
}
