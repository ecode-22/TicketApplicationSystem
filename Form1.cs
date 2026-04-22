using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicketApplicationSystem
{
    public partial class Form1 : Form
    {
        private TicketCalculator calculator;
        private Panel summaryPanel;
        private Label lblSummaryTitle;
        private Label lblSummaryDetails;
        private Timer autoHideTimer;

        public Form1()
        {
            InitializeComponent();
            calculator = new TicketCalculator();
            SetupProfessionalStyling();
            SetupLiveValidation();
            CreateSummaryPanel();
            SetupKeyboardShortcuts();
        }

        private void SetupProfessionalStyling()
        {
            // Form properties
            this.Text = "✈️ Ticket Application System";
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 700);

            // Style all buttons
            foreach (Button btn in this.Controls.OfType<Button>())
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(52, 152, 219);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                btn.Height = 40;

                // Hover effects
                btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(41, 128, 185);
                btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(52, 152, 219);
            }

            // Style the Clear button differently
            btnClear.BackColor = Color.FromArgb(231, 76, 60);
            btnClear.MouseEnter += (s, e) => btnClear.BackColor = Color.FromArgb(192, 57, 43);
            btnClear.MouseLeave += (s, e) => btnClear.BackColor = Color.FromArgb(231, 76, 60);

            // Style the Exit button
            btnExit.BackColor = Color.FromArgb(149, 165, 166);
            btnExit.MouseEnter += (s, e) => btnExit.BackColor = Color.FromArgb(127, 140, 141);
            btnExit.MouseLeave += (s, e) => btnExit.BackColor = Color.FromArgb(149, 165, 166);
        }

        private void SetupLiveValidation()
        {
            txtName.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtName.Text) &&
                    System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, @"^[a-zA-Z\s\-']*$"))
                    txtName.BackColor = Color.LightGreen;
                else if (!string.IsNullOrEmpty(txtName.Text))
                    txtName.BackColor = Color.LightPink;
                else
                    txtName.BackColor = SystemColors.Window;
            };

            txtAge.TextChanged += (s, e) =>
            {
                if (int.TryParse(txtAge.Text, out int age) && age >= 0 && age <= 120)
                    txtAge.BackColor = Color.LightGreen;
                else if (!string.IsNullOrEmpty(txtAge.Text))
                    txtAge.BackColor = Color.LightPink;
                else
                    txtAge.BackColor = SystemColors.Window;
            };

            txtDistance.TextChanged += (s, e) =>
            {
                if (double.TryParse(txtDistance.Text, out double dist) && dist > 0 && dist <= 10000)
                    txtDistance.BackColor = Color.LightGreen;
                else if (!string.IsNullOrEmpty(txtDistance.Text))
                    txtDistance.BackColor = Color.LightPink;
                else
                    txtDistance.BackColor = SystemColors.Window;
            };
        }

        private void CreateSummaryPanel()
        {
            summaryPanel = new Panel
            {
                Location = new Point(30, 450),
                Size = new Size(440, 180),
                BackColor = Color.FromArgb(236, 240, 241),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            lblSummaryTitle = new Label
            {
                Text = "🎫 TICKET SUMMARY",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(438, 35),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White
            };

            lblSummaryDetails = new Label
            {
                Location = new Point(10, 45),
                Size = new Size(420, 120),
                Font = new Font("Consolas", 9F),
                Text = ""
            };

            summaryPanel.Controls.AddRange(new Control[] { lblSummaryTitle, lblSummaryDetails });
            this.Controls.Add(summaryPanel);
            summaryPanel.BringToFront();
        }

        private void SetupKeyboardShortcuts()
        {
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                btnCalculate.PerformClick();
            else if (e.Control && e.KeyCode == Keys.R)
                btnClear.PerformClick();
            else if (e.KeyCode == Keys.F1)
                ShowHelpDialog();
            else if (e.KeyCode == Keys.Escape)
                btnExit.PerformClick();
        }

        private void ShowHelpDialog()
        {
            MessageBox.Show("✈️ Ticket Application System Help\n\n" +
                           "📝 How to Use:\n" +
                           "1. Enter passenger name (letters only)\n" +
                           "2. Enter age (0-120 years)\n" +
                           "3. Enter distance (1-10,000 km)\n" +
                           "4. Select gender and travel category\n" +
                           "5. Click CALCULATE or press Ctrl+C\n" +
                           "6. Use Ctrl+R to clear form\n" +
                           "7. Press F1 for this help\n" +
                           "8. Press ESC to exit\n\n" +
                           "💰 Discount Rules:\n" +
                           "• Children under 12: FREE TICKET\n" +
                           "• Female passengers: 50% discount\n" +
                           "• Note: Age discount overrides gender discount\n\n" +
                           "📊 Category Pricing:\n" +
                           "• Category One: R20/km\n" +
                           "• Category Two: R35/km\n" +
                           "• Category Three: R50/km",
                           "Help - Ticket System",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }

        private bool ValidateAllInputs()
        {
            ClearErrorHighlights();
            bool isValid = true;
            StringBuilder errorMessage = new StringBuilder();

            // 1. Name Validation - Letters, spaces, hyphens ONLY
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                HighlightError(txtName);
                errorMessage.AppendLine("• Passenger name is required");
                isValid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, @"^[a-zA-Z\s\-']+$"))
            {
                HighlightError(txtName);
                errorMessage.AppendLine("• Name can only contain letters, spaces, hyphens, and apostrophes");
                isValid = false;
            }

            // 2. Age Validation
            if (!int.TryParse(txtAge.Text, out int age))
            {
                HighlightError(txtAge);
                errorMessage.AppendLine("• Age must be a valid number");
                isValid = false;
            }
            else if (age < 0 || age > 120)
            {
                HighlightError(txtAge);
                errorMessage.AppendLine("• Age must be between 0 and 120 years");
                isValid = false;
            }

            // 3. Distance Validation
            if (!double.TryParse(txtDistance.Text, out double distance))
            {
                HighlightError(txtDistance);
                errorMessage.AppendLine("• Distance must be a valid number");
                isValid = false;
            }
            else if (distance <= 0)
            {
                HighlightError(txtDistance);
                errorMessage.AppendLine("• Distance must be greater than 0 km");
                isValid = false;
            }
            else if (distance > 10000)
            {
                HighlightError(txtDistance);
                errorMessage.AppendLine("• Distance cannot exceed 10,000 km");
                isValid = false;
            }

            // 4. Gender Selection
            if (!rdbMale.Checked && !rdbFemale.Checked)
            {
                HighlightError(grpGender);
                errorMessage.AppendLine("• Please select a gender");
                isValid = false;
            }

            // 5. Category Selection
            if (cmbCategory.SelectedIndex == -1)
            {
                HighlightError(cmbCategory);
                errorMessage.AppendLine("• Please select a travel category");
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show(errorMessage.ToString(), "Validation Errors",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private void HighlightError(Control control)
        {
            if (control is TextBox txt)
            {
                txt.BackColor = Color.LightPink;
            }
            else if (control is ComboBox cmb)
            {
                cmb.BackColor = Color.LightPink;
            }
            else if (control is GroupBox grp)
            {
                grp.BackColor = Color.LightPink;
            }
        }

        private void ClearErrorHighlights()
        {
            txtName.BackColor = SystemColors.Window;
            txtAge.BackColor = SystemColors.Window;
            txtDistance.BackColor = SystemColors.Window;
            cmbCategory.BackColor = SystemColors.Window;
            grpGender.BackColor = SystemColors.Control;
        }

        private void DisplayTicketSummary(double finalPrice, double originalPrice, string discountInfo)
        {
            string gender = rdbMale.Checked ? "Male" : "Female";
            string category = cmbCategory.SelectedItem.ToString();
            double distance = double.Parse(txtDistance.Text);
            int age = int.Parse(txtAge.Text);

            StringBuilder summaryText = new StringBuilder();
            summaryText.AppendLine($"Passenger: {txtName.Text}");
            summaryText.AppendLine($"Gender: {gender}");
            summaryText.AppendLine($"Age: {age} years");
            summaryText.AppendLine($"Category: {category}");
            summaryText.AppendLine($"Distance: {distance:N0} km");
            summaryText.AppendLine(new string('-', 35));

            if (finalPrice == 0)
            {
                summaryText.AppendLine();
                summaryText.AppendLine("💰 FINAL PRICE: FREE TICKET! 🎉");
                if (age < 12)
                    summaryText.AppendLine("   (Age < 12 - Free travel)");
                else if (discountInfo.Contains("Female"))
                    summaryText.AppendLine("   (50% Female Discount Applied)");
            }
            else
            {
                summaryText.AppendLine();
                summaryText.AppendLine($"Original Price: R{originalPrice:N2}");
                if (!string.IsNullOrEmpty(discountInfo))
                    summaryText.AppendLine($"Discount: {discountInfo}");
                summaryText.AppendLine(new string('-', 35));
                summaryText.AppendLine($"💰 TOTAL AMOUNT: R{finalPrice:N2}");
            }

            lblSummaryDetails.Text = summaryText.ToString();
            summaryPanel.Visible = true;
            summaryPanel.BringToFront();

            // Auto-hide after 8 seconds
            if (autoHideTimer != null)
                autoHideTimer.Dispose();
            
            autoHideTimer = new Timer();
            autoHideTimer.Interval = 8000;
            autoHideTimer.Tick += (s, e) => 
            { 
                summaryPanel.Visible = false; 
                autoHideTimer.Stop();
            };
            autoHideTimer.Start();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!ValidateAllInputs())
                return;

            try
            {
                string name = txtName.Text.Trim();
                int age = int.Parse(txtAge.Text);
                double distance = double.Parse(txtDistance.Text);
                string category = cmbCategory.SelectedItem.ToString();
                bool isFemale = rdbFemale.Checked;

                double originalPrice = calculator.CalculateBasePrice(category, distance);
                double finalPrice = originalPrice;
                string discountInfo = "";

                // Apply age discount (FREE if under 12)
                if (age < 12)
                {
                    finalPrice = 0;
                    discountInfo = "Age < 12 → FREE TICKET";
                }
                // Apply gender discount (50% off for females)
                else if (isFemale)
                {
                    finalPrice = originalPrice * 0.5;
                    discountInfo = $"50% Female Discount → -R{originalPrice * 0.5:N2}";
                }

                DisplayTicketSummary(finalPrice, originalPrice, discountInfo);

                // Show quick notification
                string message = finalPrice == 0 ?
                    $"🎉 {name}, your ticket is FREE! 🎉" :
                    $"Ticket Price for {name}: R{finalPrice:N2}";

                // Create a custom notification that auto-closes
                Form notification = new Form
                {
                    Size = new Size(300, 80),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.None,
                    BackColor = Color.FromArgb(52, 152, 219),
                    TopMost = true
                };
                Label lblMsg = new Label
                {
                    Text = message,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };
                notification.Controls.Add(lblMsg);
                notification.Show();

                Timer closeTimer = new Timer();
                closeTimer.Interval = 2000;
                closeTimer.Tick += (ts, te) => { notification.Close(); closeTimer.Stop(); };
                closeTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Calculation error: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txtName.Clear();
            txtAge.Clear();
            txtDistance.Clear();
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
            cmbCategory.SelectedIndex = -1;
            
            // Hide summary panel
            summaryPanel.Visible = false;
            
            // Reset colors
            ClearErrorHighlights();
            
            // Set focus to name field
            txtName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?",
                                                   "Confirm Exit",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}