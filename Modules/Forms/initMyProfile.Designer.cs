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
            ProfilePIC = new PictureBox();
            fName = new Label();
            lName = new Label();
            username_label = new Label();
            courseLevel = new ComboBox();
            educationGroup = new ComboBox();
            paymentButton = new Button();
            price_label = new Label();
            CouponCode = new TextBox();
            couponbutton = new Button();
            ((System.ComponentModel.ISupportInitialize)ProfilePIC).BeginInit();
            SuspendLayout();
            // 
            // ProfilePIC
            // 
            ProfilePIC.BackColor = Color.LightGray;
            ProfilePIC.BorderStyle = BorderStyle.FixedSingle;
            ProfilePIC.Location = new Point(180, 20);
            ProfilePIC.Name = "ProfilePIC";
            ProfilePIC.Size = new Size(100, 100);
            ProfilePIC.TabIndex = 0;
            ProfilePIC.TabStop = false;
            // 
            // fName
            // 
            fName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            fName.ForeColor = Color.Black;
            fName.Location = new Point(142, 130);
            fName.Name = "fName";
            fName.Size = new Size(100, 23);
            fName.TabIndex = 1;
            fName.Text = "First Name";
            // 
            // lName
            // 
            lName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lName.ForeColor = Color.Black;
            lName.Location = new Point(248, 130);
            lName.Name = "lName";
            lName.Size = new Size(100, 23);
            lName.TabIndex = 2;
            lName.Text = "Last Name";
            // 
            // username_label
            // 
            username_label.Font = new Font("Segoe UI", 9F);
            username_label.ForeColor = Color.Gray;
            username_label.Location = new Point(180, 160);
            username_label.Name = "username_label";
            username_label.Size = new Size(100, 23);
            username_label.TabIndex = 3;
            username_label.Text = "@username";
            // 
            // courseLevel
            // 
            courseLevel.Font = new Font("Segoe UI", 9F);
            courseLevel.Items.AddRange(new object[] { "Beginner", "Intermediate", "Advanced" });
            courseLevel.Location = new Point(160, 200);
            courseLevel.Name = "courseLevel";
            courseLevel.Size = new Size(121, 23);
            courseLevel.TabIndex = 4;
            // 
            // educationGroup
            // 
            educationGroup.Font = new Font("Segoe UI", 9F);
            educationGroup.Items.AddRange(new object[] { "Science", "Arts", "Commerce" });
            educationGroup.Location = new Point(160, 240);
            educationGroup.Name = "educationGroup";
            educationGroup.Size = new Size(121, 23);
            educationGroup.TabIndex = 5;
            // 
            // paymentButton
            // 
            paymentButton.BackColor = Color.SeaGreen;
            paymentButton.FlatStyle = FlatStyle.Flat;
            paymentButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            paymentButton.ForeColor = Color.White;
            paymentButton.Location = new Point(180, 320);
            paymentButton.Name = "paymentButton";
            paymentButton.Size = new Size(100, 30);
            paymentButton.TabIndex = 6;
            paymentButton.Text = "Buy Now";
            paymentButton.UseVisualStyleBackColor = false;
            // 
            // price_label
            // 
            price_label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            price_label.Location = new Point(180, 280);
            price_label.Name = "price_label";
            price_label.Size = new Size(100, 23);
            price_label.TabIndex = 7;
            price_label.Text = "$5000";
            // 
            // CouponCode
            // 
            CouponCode.Font = new Font("Segoe UI", 9F);
            CouponCode.Location = new Point(160, 360);
            CouponCode.Name = "CouponCode";
            CouponCode.Size = new Size(120, 23);
            CouponCode.TabIndex = 8;
            CouponCode.Text = "Enter Coupon";
            // 
            // couponbutton
            // 
            couponbutton.FlatStyle = FlatStyle.Flat;
            couponbutton.Font = new Font("Segoe UI", 9F);
            couponbutton.Location = new Point(300, 360);
            couponbutton.Name = "couponbutton";
            couponbutton.Size = new Size(75, 23);
            couponbutton.TabIndex = 9;
            couponbutton.Text = "Apply";
            // 
            // initMyProfile
            // 
            ClientSize = new Size(500, 450);
            Controls.Add(ProfilePIC);
            Controls.Add(fName);
            Controls.Add(lName);
            Controls.Add(username_label);
            Controls.Add(courseLevel);
            Controls.Add(educationGroup);
            Controls.Add(paymentButton);
            Controls.Add(price_label);
            Controls.Add(CouponCode);
            Controls.Add(couponbutton);
            Name = "initMyProfile";
            Text = "Profile Setup";
            ((System.ComponentModel.ISupportInitialize)ProfilePIC).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
