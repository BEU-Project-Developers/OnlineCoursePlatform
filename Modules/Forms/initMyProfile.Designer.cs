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
            ProfilePIC.Location = new Point(100, 20); // Adjusted Location
            ProfilePIC.Name = "ProfilePIC";
            ProfilePIC.Size = new Size(150, 150); // Increased Size
            ProfilePIC.TabIndex = 0;
            ProfilePIC.TabStop = false;
            ProfilePIC.SizeMode = PictureBoxSizeMode.StretchImage; // Added SizeMode for better image scaling
            //
            // fName
            //
            fName.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Increased Font Size
            fName.ForeColor = Color.Black;
            fName.Location = new Point(70, 180); // Adjusted Location
            fName.Name = "fName";
            fName.Size = new Size(150, 30); // Increased Size
            fName.TabIndex = 1;
            fName.Text = "First Name";
            fName.TextAlign = ContentAlignment.MiddleCenter; // Centered Text
            //
            // lName
            //
            lName.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Increased Font Size
            lName.ForeColor = Color.Black;
            lName.Location = new Point(230, 180); // Adjusted Location
            lName.Name = "lName";
            lName.Size = new Size(150, 30); // Increased Size
            lName.TabIndex = 2;
            lName.Text = "Last Name";
            lName.TextAlign = ContentAlignment.MiddleCenter; // Centered Text
            //
            // username_label
            //
            username_label.Font = new Font("Segoe UI", 12F); // Increased Font Size
            username_label.ForeColor = Color.Gray;
            username_label.Location = new Point(150, 210); // Adjusted Location
            username_label.Name = "username_label";
            username_label.Size = new Size(150, 30); // Increased Size
            username_label.TabIndex = 3;
            username_label.Text = "@username";
            username_label.TextAlign = ContentAlignment.MiddleCenter; // Centered Text
            //
            // courseLevel
            //
            courseLevel.Font = new Font("Segoe UI", 12F); // Increased Font Size
            courseLevel.Items.AddRange(new object[] { "Beginner", "Intermediate", "Advanced" });
            courseLevel.Location = new Point(130, 260); // Adjusted Location
            courseLevel.Name = "courseLevel";
            courseLevel.Size = new Size(200, 30); // Increased Size
            courseLevel.TabIndex = 4;
            //
            // educationGroup
            //
            educationGroup.Font = new Font("Segoe UI", 12F); // Increased Font Size
            educationGroup.Items.AddRange(new object[] { "Science", "Arts", "Commerce" });
            educationGroup.Location = new Point(130, 310); // Adjusted Location
            educationGroup.Name = "educationGroup";
            educationGroup.Size = new Size(200, 30); // Increased Size
            educationGroup.TabIndex = 5;
            //
            // paymentButton
            //
            paymentButton.BackColor = Color.SeaGreen;
            paymentButton.FlatStyle = FlatStyle.Flat;
            paymentButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Increased Font Size
            paymentButton.ForeColor = Color.White;
            paymentButton.Location = new Point(150, 440); // Adjusted Location
            paymentButton.Name = "paymentButton";
            paymentButton.Size = new Size(160, 40); // Increased Size
            paymentButton.TabIndex = 6;
            paymentButton.Text = "Buy Now";
            paymentButton.UseVisualStyleBackColor = false;
            paymentButton.Click += paymentButton_Click;
            //
            // price_label
            //
            price_label.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Increased Font Size
            price_label.Location = new Point(150, 390); // Adjusted Location
            price_label.Name = "price_label";
            price_label.Size = new Size(160, 30); // Increased Size
            price_label.TabIndex = 7;
            price_label.Text = "$5000";
            price_label.TextAlign = ContentAlignment.MiddleCenter; // Centered Text
            //
            // CouponCode
            //
            CouponCode.Font = new Font("Segoe UI", 12F); // Increased Font Size
            CouponCode.Location = new Point(130, 490); // Adjusted Location
            CouponCode.Name = "CouponCode";
            CouponCode.Size = new Size(160, 30); // Increased Size
            CouponCode.TabIndex = 8;
            CouponCode.Text = "Enter Coupon";
            CouponCode.Enter += CouponCode_Enter;
            CouponCode.Leave += CouponCode_Leave;
            //
            // couponbutton
            //
            couponbutton.FlatStyle = FlatStyle.Flat;
            couponbutton.Font = new Font("Segoe UI", 12F); // Increased Font Size
            couponbutton.Location = new Point(300, 490); // Adjusted Location
            couponbutton.Name = "couponbutton";
            couponbutton.Size = new Size(80, 30); // Increased Size
            couponbutton.TabIndex = 9;
            couponbutton.Text = "Apply";
            //
            // initMyProfile
            //
            AutoScaleDimensions = new SizeF(6F, 13F); // Keep default or adjust if needed for scaling
            AutoScaleMode = AutoScaleMode.Font; // Keep default or adjust if needed for scaling
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
            Size = new Size(500, 600); // Increased Form Size to accommodate bigger components
            ((System.ComponentModel.ISupportInitialize)ProfilePIC).EndInit();
            ResumeLayout(false);
            PerformLayout();

            // Center the controls on the form
            CenterControls();
        }

        private void CenterControls()
        {
            // Calculate total width dynamically based on components
            int labelPairWidth = fName.Width + 10 + lName.Width; // Width of First and Last Name labels + spacing
            int couponGroupWidth = CouponCode.Width + 10 + couponbutton.Width; // Width of Coupon Code and Button + spacing
            int maxHorizontalWidth = Math.Max(labelPairWidth, Math.Max(couponGroupWidth, Math.Max(courseLevel.Width, educationGroup.Width))); // Find the widest element

            int totalWidth = Math.Max(maxHorizontalWidth, ProfilePIC.Width); // Consider ProfilePIC width as well
            totalWidth = Math.Max(totalWidth, paymentButton.Width); // Consider paymentButton width
            totalWidth = Math.Max(totalWidth, price_label.Width); // Consider price_label width
            totalWidth = Math.Max(totalWidth, username_label.Width); // Consider username_label width


            int totalHeight = 600; // Approximate total height, adjust if needed

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            int startX = centerX - (totalWidth / 2); // Calculate starting X position for centering
            int startY = centerY - (totalHeight / 2); // Calculate starting Y position for centering

            int currentY = 20; // Initial top margin

            ProfilePIC.Location = new Point(centerX - (ProfilePIC.Width / 2), currentY);
            currentY += ProfilePIC.Height + 20; // Add spacing below PictureBox

            fName.Location = new Point(centerX - (fName.Width + 10 + lName.Width) / 2, currentY); // Center fName and lName together
            lName.Location = new Point(fName.Location.X + fName.Width + 10, currentY); // Position to the right of fName
            currentY += fName.Height + 10; // Add spacing below names

            username_label.Location = new Point(centerX - (username_label.Width / 2), currentY);
            currentY += username_label.Height + 30; // Add spacing below username

            courseLevel.Location = new Point(centerX - (courseLevel.Width / 2), currentY);
            currentY += courseLevel.Height + 10; // Add spacing below courseLevel

            educationGroup.Location = new Point(centerX - (educationGroup.Width / 2), currentY);
            currentY += educationGroup.Height + 30; // Add spacing below educationGroup

            price_label.Location = new Point(centerX - (price_label.Width / 2), currentY);
            currentY += price_label.Height + 10; // Add spacing below price_label

            paymentButton.Location = new Point(centerX - (paymentButton.Width / 2), currentY);
            currentY += paymentButton.Height + 30; // Add spacing below paymentButton

            CouponCode.Location = new Point(centerX - (CouponCode.Width + 10 + couponbutton.Width) / 2, currentY); // Center CouponCode and couponbutton together
            couponbutton.Location = new Point(CouponCode.Location.X + CouponCode.Width + 10, currentY); // Position couponbutton next to CouponCode

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