namespace Prabesh_Academy.Modules.Forms
{
    partial class initMyProfile
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ProfilePIC = new System.Windows.Forms.PictureBox();
            this.fName = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.username_label = new System.Windows.Forms.Label();
            this.courseLevel = new System.Windows.Forms.ComboBox();
            this.educationGroup = new System.Windows.Forms.ComboBox();
            this.paymentButton = new System.Windows.Forms.Button();
            this.price_label = new System.Windows.Forms.Label();
            this.CouponCode = new System.Windows.Forms.TextBox();
            this.couponbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ProfilePIC
            this.ProfilePIC.Location = new System.Drawing.Point(180, 20);
            this.ProfilePIC.Size = new System.Drawing.Size(100, 100);
            this.ProfilePIC.BorderStyle = BorderStyle.FixedSingle;
            this.ProfilePIC.BackColor = System.Drawing.Color.LightGray;
            this.ProfilePIC.TabIndex = 0;

            // fName
            this.fName.Location = new System.Drawing.Point(170, 130);
            this.fName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.fName.ForeColor = System.Drawing.Color.Black;
            this.fName.Text = "First Name";

            // lName
            this.lName.Location = new System.Drawing.Point(270, 130);
            this.lName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lName.ForeColor = System.Drawing.Color.Black;
            this.lName.Text = "Last Name";

            // username_label
            this.username_label.Location = new System.Drawing.Point(180, 160);
            this.username_label.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.username_label.ForeColor = System.Drawing.Color.Gray;
            this.username_label.Text = "@username";

            // courseLevel
            this.courseLevel.Location = new System.Drawing.Point(160, 200);
            this.courseLevel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.courseLevel.Items.AddRange(new string[] { "Beginner", "Intermediate", "Advanced" });

            // educationGroup
            this.educationGroup.Location = new System.Drawing.Point(160, 240);
            this.educationGroup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.educationGroup.Items.AddRange(new string[] { "Science", "Arts", "Commerce" });

            // paymentButton
            this.paymentButton.Location = new System.Drawing.Point(180, 320);
            this.paymentButton.Size = new System.Drawing.Size(100, 30);
            this.paymentButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.paymentButton.Text = "Buy Now";
            this.paymentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paymentButton.BackColor = System.Drawing.Color.SeaGreen;
            this.paymentButton.ForeColor = System.Drawing.Color.White;

            // price_label
            this.price_label.Location = new System.Drawing.Point(180, 280);
            this.price_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.price_label.Text = "$5000";

            // CouponCode
            this.CouponCode.Location = new System.Drawing.Point(160, 360);
            this.CouponCode.Size = new System.Drawing.Size(120, 23);
            this.CouponCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CouponCode.Text = "Enter Coupon";

            // couponbutton
            this.couponbutton.Location = new System.Drawing.Point(300, 360);
            this.couponbutton.Size = new System.Drawing.Size(75, 23);
            this.couponbutton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.couponbutton.Text = "Apply";
            this.couponbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // initMyProfile
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Controls.Add(this.ProfilePIC);
            this.Controls.Add(this.fName);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.username_label);
            this.Controls.Add(this.courseLevel);
            this.Controls.Add(this.educationGroup);
            this.Controls.Add(this.paymentButton);
            this.Controls.Add(this.price_label);
            this.Controls.Add(this.CouponCode);
            this.Controls.Add(this.couponbutton);
            this.Name = "initMyProfile";
            this.Text = "Profile Setup";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox ProfilePIC;
        private System.Windows.Forms.Label fName;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label username_label;
        private System.Windows.Forms.ComboBox courseLevel;
        private System.Windows.Forms.ComboBox educationGroup;
        private System.Windows.Forms.Button paymentButton;
        private System.Windows.Forms.Label price_label;
        private System.Windows.Forms.TextBox CouponCode;
        private System.Windows.Forms.Button couponbutton;
    }
}
