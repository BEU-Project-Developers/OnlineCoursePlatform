using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Prabesh_Academy.Modules.Views
{
    partial class Course_Home
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.back_button = new System.Windows.Forms.Button();
            this.home_button = new System.Windows.Forms.Button();
            this.refresh_button = new System.Windows.Forms.Button();
            this.logout_button = new System.Windows.Forms.Button();
            this.user_button = new System.Windows.Forms.Button();


            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 100);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Padding(0,back_button.Height+10,0,0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 361);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = true;
            // 
            // back_button
            // 
            this.back_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.back_button.Location = new System.Drawing.Point(10, 10);
            this.back_button.Name = "back_button";
            this.back_button.Size = new System.Drawing.Size(30, 30);
            this.back_button.TabIndex = 1;
            this.back_button.UseVisualStyleBackColor = true;
            this.back_button.Click += new System.EventHandler(BackButton_Click);

            this.home_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.home_button.Location = new System.Drawing.Point(40, 10);
            this.home_button.Name = "home_button";
            this.home_button.Size = new System.Drawing.Size(30, 30);
            this.home_button.TabIndex = 1;
            this.home_button.UseVisualStyleBackColor = true;
            this.home_button.Click += home_button_Click;

            this.refresh_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.refresh_button.Location = new System.Drawing.Point(80, 10);
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(30, 30);
            this.refresh_button.TabIndex = 1;
            this.refresh_button.UseVisualStyleBackColor = true;
            this.refresh_button.Click+=refresh_button_Click;

            this.logout_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.logout_button.Location = new System.Drawing.Point(120, 10);
            this.logout_button.Name = "back_button";
            this.logout_button.Size = new System.Drawing.Size(30, 30);
            this.logout_button.TabIndex = 1;
            this.logout_button.UseVisualStyleBackColor = true;
            this.logout_button.Click+=logout_button_Click;
            // 

            this.user_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.user_button.Location = new System.Drawing.Point(160, 10);
            this.user_button.Name = "back_button";
            this.user_button.Size = new System.Drawing.Size(30, 30);
            this.user_button.TabIndex = 1;
            this.user_button.UseVisualStyleBackColor = true;
            this.user_button.Click += user_button_Click;


            // Course_Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.Controls.Add(this.back_button);
            this.Controls.Add(this.home_button);
            this.Controls.Add(this.refresh_button);
            this.Controls.Add(this.logout_button);
            this.Controls.Add(this.user_button);

            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Course_Home";
            this.Size = new System.Drawing.Size(800, 411);
            this.ResumeLayout(false);

        }

    

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button back_button;
        private Button home_button;
        private Button refresh_button;
        private Button logout_button;
        private Button user_button;


    }
}