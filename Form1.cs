using System;
using System.Windows.Forms;

namespace TicketApplicationSystem
{
    public partial class Form1 : Form
    {
        private TicketCalculator calculator;

        public Form1()
        {
            InitializeComponent();
            calculator = new TicketCalculator();
            InitializeComboBox();
        }

        // Initialize ComboBox with categories
        private void InitializeComboBox()
        {
            cmbCategory.Items.Add("Category One");
            cmbCategory.Items.Add("Category Two");
            cmbCategory.Items.Add("Category Three");
        }

        // Calculate button click event
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (!ValidateInputs())
                {
                    return;
                }

                // Get input values
                string passengerName = txtName.Text.Trim();
                int age = int.Parse(txtAge.Text);
                decimal distance = decimal.Parse(txtDistance.Text);
                string category = cmbCategory.SelectedItem.ToString();
                string gender = rbMale.Checked ? "Male" : "Female";

                // Calculate final price
                decimal finalPrice = calculator.CalculateFinalPrice(category, distance, age, gender);

                // Display ticket summary
                DisplayTicketSummary(passengerName, gender, age, category, distance, finalPrice);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Calculation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validate all input fields
        private bool ValidateInputs()
        {
            // Check if name is empty
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter passenger name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            // Check if age is numeric and valid
            if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 120)
            {
                MessageBox.Show("Please enter a valid age (0-120).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return false;
            }

            // Check if distance is numeric and valid
            if (!decimal.TryParse(txtDistance.Text, out decimal distance) || distance <= 0)
            {
                MessageBox.Show("Please enter a valid distance (greater than 0).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDistance.Focus();
                return false;
            }

            // Check if category is selected
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a travel category.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            // Check if gender is selected
            if (!rbMale.Checked && !rbFemale.Checked)
            {
                MessageBox.Show("Please select gender.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // Display ticket summary in MessageBox
        private void DisplayTicketSummary(string name, string gender, int age, 
            string category, decimal distance, decimal finalPrice)
        {
            string summary = "═══════════════════════════════\n";
            summary += "     TICKET SUMMARY\n";
            summary += "═══════════════════════════════\n\n";
            summary += $"Passenger Name: {name}\n";
            summary += $"Gender: {gender}\n";
            summary += $"Age: {age} years\n";
            summary += $"Category: {category}\n";
            summary += $"Distance: {distance} km\n";
            summary += $"\n─────────────────────────────\n";
            summary += $"Final Ticket Price: R{finalPrice:F2}\n";
            summary += "═══════════════════════════════";

            MessageBox.Show(summary, "Ticket Calculation Result", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Clear button click event
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txtName.Clear();
            txtAge.Clear();
            txtDistance.Clear();
            cmbCategory.SelectedIndex = -1;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            
            // Set focus to name field
            txtName.Focus();
        }

        // Exit button click event
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?", 
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
