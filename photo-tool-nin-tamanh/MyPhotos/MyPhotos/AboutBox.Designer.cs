namespace MyPhotos
{
    partial class AboutBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblAboutText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Book_Green.ico");
            this.imageList1.Images.SetKeyName(1, "camera.ico");
            // 
            // lblIcon
            // 
            this.lblIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblIcon.ImageIndex = 0;
            this.lblIcon.ImageList = this.imageList1;
            this.lblIcon.Location = new System.Drawing.Point(13, 13);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(45, 54);
            this.lblIcon.TabIndex = 0;
            // 
            // lblAboutText
            // 
            this.lblAboutText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAboutText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAboutText.Location = new System.Drawing.Point(64, 13);
            this.lblAboutText.Name = "lblAboutText";
            this.lblAboutText.Size = new System.Drawing.Size(208, 54);
            this.lblAboutText.TabIndex = 1;
            this.lblAboutText.Text = "About MyPhotos";
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblAboutText);
            this.Controls.Add(this.lblIcon);
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Text = "AboutBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.Label lblAboutText;
    }
}