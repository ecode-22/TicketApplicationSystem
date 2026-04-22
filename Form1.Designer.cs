namespace TicketApplicationSystem
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.GroupBox grpGender;
        private System.Windows.Forms.RadioButton rdbMale;
        private System.Windows.Forms.RadioButton rdbFemale;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExit;

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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblDistance = new System.Windows.Forms.Label();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.grpGender = new System.Windows.Forms.GroupBox();
            this.rdbMale = new System.Windows.Forms.RadioButton();
            this.rdbFemale = new System.Windows.Forms.RadioButton();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.grpGender.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.Location = new System.Drawing.Point(22, 81);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(73, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Full Name:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtName.Location = new System.Drawing.Point(112, 79);
            this.txtName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(124, 25);
            this.txtName.TabIndex = 1;
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAge.Location = new System.Drawing.Point(22, 122);
            this.lblAge.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(36, 19);
            this.lblAge.TabIndex = 2;
            this.lblAge.Text = "Age:";
            // 
            // txtAge
            // 
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAge.Location = new System.Drawing.Point(112, 119);
            this.txtAge.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(124, 25);
            this.txtAge.TabIndex = 3;
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDistance.Location = new System.Drawing.Point(22, 162);
            this.lblDistance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(95, 19);
            this.lblDistance.TabIndex = 4;
            this.lblDistance.Text = "Distance (km):";
            // 
            // txtDistance
            // 
            this.txtDistance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDistance.Location = new System.Drawing.Point(121, 159);
            this.txtDistance.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(108, 25);
            this.txtDistance.TabIndex = 5;
            // 
            // grpGender
            // 
            this.grpGender.Controls.Add(this.rdbMale);
            this.grpGender.Controls.Add(this.rdbFemale);
            this.grpGender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grpGender.Location = new System.Drawing.Point(22, 203);
            this.grpGender.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpGender.Name = "grpGender";
            this.grpGender.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grpGender.Size = new System.Drawing.Size(315, 49);
            this.grpGender.TabIndex = 6;
            this.grpGender.TabStop = false;
            this.grpGender.Text = "Gender";
            // 
            // rdbMale
            // 
            this.rdbMale.AutoSize = true;
            this.rdbMale.Location = new System.Drawing.Point(22, 20);
            this.rdbMale.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdbMale.Name = "rdbMale";
            this.rdbMale.Size = new System.Drawing.Size(57, 23);
            this.rdbMale.TabIndex = 0;
            this.rdbMale.TabStop = true;
            this.rdbMale.Text = "Male";
            this.rdbMale.UseVisualStyleBackColor = true;
            // 
            // rdbFemale
            // 
            this.rdbFemale.AutoSize = true;
            this.rdbFemale.Location = new System.Drawing.Point(112, 20);
            this.rdbFemale.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdbFemale.Name = "rdbFemale";
            this.rdbFemale.Size = new System.Drawing.Size(70, 23);
            this.rdbFemale.TabIndex = 1;
            this.rdbFemale.TabStop = true;
            this.rdbFemale.Text = "Female";
            this.rdbFemale.UseVisualStyleBackColor = true;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCategory.Location = new System.Drawing.Point(11, 319);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(68, 19);
            this.lblCategory.TabIndex = 7;
            this.lblCategory.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCategory.Items.AddRange(new object[] {
            "Category One",
            "Category Two",
            "Category Three"});
            this.cmbCategory.Location = new System.Drawing.Point(111, 313);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(226, 25);
            this.cmbCategory.TabIndex = 8;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCalculate.Location = new System.Drawing.Point(11, 395);
            this.btnCalculate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(98, 32);
            this.btnCalculate.TabIndex = 9;
            this.btnCalculate.Text = "CALCULATE";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.Location = new System.Drawing.Point(131, 395);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(98, 32);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.Location = new System.Drawing.Point(240, 395);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 32);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "EXIT";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 569);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.grpGender);
            this.Controls.Add(this.txtDistance);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Ticket Application System";
            this.grpGender.ResumeLayout(false);
            this.grpGender.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}