using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using QRCoder;

namespace TicketApplicationSystem
{
    public partial class Form1 : Form
    {
        // ── Palette ────────────────────────────────────────────────────────────
        private static readonly Color Navy = Color.FromArgb(26, 45, 68);
        private static readonly Color NavyLight = Color.FromArgb(37, 62, 96);
        private static readonly Color AccentBlue = Color.FromArgb(59, 130, 246);
        private static readonly Color PageBg = Color.FromArgb(245, 247, 250);
        private static readonly Color CardBg = Color.White;
        private static readonly Color BorderClr = Color.FromArgb(220, 224, 230);
        private static readonly Color TextPri = Color.FromArgb(15, 23, 42);
        private static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        private static readonly Color GreenClr = Color.FromArgb(22, 163, 74);
        private static readonly Color RedClr = Color.FromArgb(220, 38, 38);
        private static readonly Color GreenBg = Color.FromArgb(220, 252, 231);
        private static readonly Color AmberBg = Color.FromArgb(254, 243, 199);
        private static readonly Color AmberText = Color.FromArgb(146, 64, 14);

        private TicketCalculator calculator;
        private Panel summaryPanel;
        private Label lblSummaryTitle, lblSummaryDetails;
        private Timer autoHideTimer;

        private bool isDarkMode = false;
        private Button btnTheme;

        private List<TicketRecord> ticketHistory = new List<TicketRecord>();
        private ListBox lstHistory;
        private Panel historyPanel;
        private Button btnHistory;

        private string currentLanguage = "English";
        private Dictionary<string, Dictionary<string, string>> translations;
        private ComboBox cmbLanguage;

        private PictureBox pbQRCode;
        private Button btnGenerateQR;
        private Button btnExport;
        private Label lblLivePrice;

        public Form1()
        {
            InitializeComponent();
            calculator = new TicketCalculator();
            InitializeTranslations();
            SetupProfessionalStyling();
            SetupLiveValidation();
            CreateSummaryPanel();
            SetupKeyboardShortcuts();
            SetupTopBar();
            SetupHistoryPanel();
            SetupPDFExport();
            SetupQRCodeFeature();
            SetupLivePricePreview();
            ApplyBaseTheme();
        }

        //  BASE THEME

        private void ApplyBaseTheme()
        {
            this.BackColor = PageBg;

            // Style every label on the form
            foreach (Control c in this.Controls)
                StyleControl(c);

            // Main action buttons
            StylePrimaryButton(btnCalculate);
            StyleSecondaryButton(btnClear);
            StyleDangerButton(btnExit);

            // Inputs & combo
            StyleTextBox(txtName);
            StyleTextBox(txtAge);
            StyleTextBox(txtDistance);
            StyleComboBox(cmbCategory);

            // GroupBox
            grpGender.FlatStyle = FlatStyle.Flat;
            grpGender.BackColor = CardBg;
            grpGender.ForeColor = TextMuted;
            grpGender.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            foreach (Control c in grpGender.Controls)
            {
                if (c is RadioButton rb)
                {
                    rb.FlatStyle = FlatStyle.Flat;
                    rb.BackColor = Color.Transparent;
                    rb.ForeColor = TextPri;
                    rb.Font = new Font("Segoe UI", 10F);
                    rb.Cursor = Cursors.Hand;
                }
            }
        }

        private void StyleControl(Control c)
        {
            if (c is Label lbl && lbl != lblLivePrice && lbl != lblSummaryTitle && lbl != lblSummaryDetails)
            {
                lbl.ForeColor = TextMuted;
                lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
        }

        private void StyleTextBox(TextBox tb)
        {
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.BackColor = CardBg;
            tb.ForeColor = TextPri;
            tb.Font = new Font("Segoe UI", 10F);
        }

        private void StyleComboBox(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = CardBg;
            cb.ForeColor = TextPri;
            cb.Font = new Font("Segoe UI", 10F);
        }

        private void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Navy;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Height = 40;
        }

        private void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = BorderClr;
            btn.BackColor = CardBg;
            btn.ForeColor = TextPri;
            btn.Font = new Font("Segoe UI", 10F);
            btn.Cursor = Cursors.Hand;
            btn.Height = 40;
        }

        private void StyleDangerButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
            btn.BackColor = Color.FromArgb(254, 242, 242);
            btn.ForeColor = RedClr;
            btn.Font = new Font("Segoe UI", 10F);
            btn.Cursor = Cursors.Hand;
            btn.Height = 40;
        }

        private void StyleSmallIconButton(Button btn, Color accent)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = BorderClr;
            btn.BackColor = CardBg;
            btn.ForeColor = accent;
            btn.Font = new Font("Segoe UI", 9F);
            btn.Cursor = Cursors.Hand;
            btn.Height = 32;
        }

        //  TOP BAR (language + theme pills)

        private void SetupTopBar()
        {
            // Navy top banner (drawn as a Panel behind the title label)
            Panel topBar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(this.ClientSize.Width, 52),
                BackColor = Navy
            };
            this.Controls.Add(topBar);
            topBar.BringToFront();

            // App title inside the bar
            Label titleLabel = new Label
            {
                Text = "✈  Ticket Application System",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(16, 14),
                AutoSize = true
            };
            topBar.Controls.Add(titleLabel);

            // Language pill
            cmbLanguage = new ComboBox
            {
                Location = new Point(this.ClientSize.Width - 200, 13),
                Size = new Size(90, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                BackColor = NavyLight,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cmbLanguage.Items.AddRange(new[] { "English", "Afrikaans", "Xhosa" });
            cmbLanguage.SelectedIndex = 0;
            cmbLanguage.SelectedIndexChanged += (s, e) => ChangeLanguage(cmbLanguage.SelectedItem.ToString());
            topBar.Controls.Add(cmbLanguage);

            // Theme toggle pill
            btnTheme = new Button
            {
                Text = "🌙  Dark",
                Location = new Point(this.ClientSize.Width - 102, 12),
                Size = new Size(88, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = NavyLight,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            btnTheme.FlatAppearance.BorderSize = 0;
            btnTheme.Click += BtnTheme_Click;
            topBar.Controls.Add(btnTheme);

            // Push form content down so it doesn't overlap the top bar
            // (set all existing controls' Y += 52 if they start at Y < 52)
            foreach (Control c in this.Controls)
            {
                if (c != topBar && c.Top < 80)
                    c.Top += 52;
            }
        }

        //  DARK/LIGHT THEME

        private void BtnTheme_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            btnTheme.Text = isDarkMode ? "☀  Light" : "🌙  Dark";

            Color bg = isDarkMode ? Color.FromArgb(15, 23, 42) : PageBg;
            Color surf = isDarkMode ? Color.FromArgb(30, 41, 59) : CardBg;
            Color txt = isDarkMode ? Color.FromArgb(226, 232, 240) : TextPri;
            Color muted = isDarkMode ? Color.FromArgb(148, 163, 184) : TextMuted;
            Color bdr = isDarkMode ? Color.FromArgb(51, 65, 85) : BorderClr;

            this.BackColor = bg;

            void ThemeControls(Control.ControlCollection controls)
            {
                foreach (Control c in controls)
                {
                    if (c is TextBox tb) { tb.BackColor = surf; tb.ForeColor = txt; }
                    else if (c is ComboBox cb) { cb.BackColor = surf; cb.ForeColor = txt; }
                    else if (c is Label lbl && lbl != lblLivePrice && lbl != lblSummaryTitle)
                    { lbl.ForeColor = muted; lbl.BackColor = Color.Transparent; }
                    else if (c is GroupBox gb)
                    { gb.ForeColor = muted; gb.BackColor = bg; ThemeControls(gb.Controls); }
                    else if (c is RadioButton rb) { rb.ForeColor = txt; rb.BackColor = Color.Transparent; }
                    else if (c is Panel p && p != summaryPanel && p != historyPanel)
                    { p.BackColor = bg; ThemeControls(p.Controls); }
                    else if (c is ListBox lb) { lb.BackColor = surf; lb.ForeColor = txt; }
                    ThemeControls(c.Controls);
                }
            }

            ThemeControls(this.Controls);

            if (lblLivePrice != null)
                lblLivePrice.BackColor = isDarkMode ? Color.FromArgb(30, 41, 59) : AmberBg;

            if (summaryPanel != null)
                summaryPanel.BackColor = isDarkMode ? Color.FromArgb(30, 41, 59) : CardBg;

            if (historyPanel != null)
            {
                historyPanel.BackColor = isDarkMode ? Color.FromArgb(30, 41, 59) : Color.FromArgb(248, 250, 252);
                lstHistory.BackColor = isDarkMode ? Color.FromArgb(15, 23, 42) : CardBg;
                lstHistory.ForeColor = isDarkMode ? Color.FromArgb(226, 232, 240) : TextPri;
            }
        }

        //  LIVE PRICE PREVIEW

        private void SetupLivePricePreview()
        {
            lblLivePrice = new Label
            {
                Location = new Point(30, 365),
                Size = new Size(420, 38),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = translations[currentLanguage]["LivePrice"],
                BackColor = AmberBg,
                ForeColor = AmberText,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblLivePrice);

            txtDistance.TextChanged += UpdateLivePreview;
            cmbCategory.SelectedIndexChanged += UpdateLivePreview;
            rdbMale.CheckedChanged += UpdateLivePreview;
            rdbFemale.CheckedChanged += UpdateLivePreview;
            txtAge.TextChanged += UpdateLivePreview;
        }

        private void UpdateLivePreview(object sender, EventArgs e)
        {
            var dict = translations[currentLanguage];
            try
            {
                if (cmbCategory.SelectedIndex != -1 &&
                    double.TryParse(txtDistance.Text, out double distance) && distance > 0 &&
                    int.TryParse(txtAge.Text, out int age))
                {
                    string internalCategory = GetInternalCategory(cmbCategory.SelectedItem.ToString());
                    double basePrice = calculator.CalculateBasePrice(internalCategory, distance);
                    double finalPrice = calculator.ApplyDiscounts(basePrice, age, rdbFemale.Checked);

                    if (age < 12)
                    {
                        lblLivePrice.Text = dict["LivePriceFree"];
                        lblLivePrice.BackColor = GreenBg;
                        lblLivePrice.ForeColor = GreenClr;
                    }
                    else if (rdbFemale.Checked)
                    {
                        lblLivePrice.Text = string.Format(dict["LivePriceDiscount"], finalPrice);
                        lblLivePrice.BackColor = GreenBg;
                        lblLivePrice.ForeColor = GreenClr;
                    }
                    else
                    {
                        lblLivePrice.Text = string.Format(dict["LivePriceNormal"], finalPrice);
                        lblLivePrice.BackColor = GreenBg;
                        lblLivePrice.ForeColor = GreenClr;
                    }
                }
                else
                {
                    lblLivePrice.Text = dict["LivePrice"];
                    lblLivePrice.BackColor = AmberBg;
                    lblLivePrice.ForeColor = AmberText;
                }
            }
            catch
            {
                lblLivePrice.Text = dict["LivePrice"];
                lblLivePrice.BackColor = AmberBg;
                lblLivePrice.ForeColor = AmberText;
            }
        }

        //  TICKET SUMMARY PANEL

        private void CreateSummaryPanel()
        {
            var dict = translations[currentLanguage];

            summaryPanel = new Panel
            {
                Location = new Point(30, 415),
                Size = new Size(420, 170),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            lblSummaryTitle = new Label
            {
                Text = dict["TicketSummary"],
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(418, 36),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Navy,
                ForeColor = Color.White
            };

            lblSummaryDetails = new Label
            {
                Location = new Point(14, 44),
                Size = new Size(392, 120),
                Font = new Font("Consolas", 9F),
                ForeColor = TextPri,
                BackColor = Color.Transparent,
                Text = ""
            };

            summaryPanel.Controls.AddRange(new Control[] { lblSummaryTitle, lblSummaryDetails });
            this.Controls.Add(summaryPanel);
            summaryPanel.BringToFront();
        }

        private void DisplayTicketSummary(double finalPrice, double originalPrice, string discountInfo)
        {
            var dict = translations[currentLanguage];
            string gender = rdbFemale.Checked ? dict["Female"] : dict["Male"];
            string category = cmbCategory.SelectedItem.ToString();
            double distance = double.Parse(txtDistance.Text);
            int age = int.Parse(txtAge.Text);

            var sb = new StringBuilder();
            sb.AppendLine($"  {dict["Passenger"]}  {txtName.Text}");
            sb.AppendLine($"  {dict["Gender"]}:     {gender}   |   {dict["Age"]}: {age} {dict["Years"]}");
            sb.AppendLine($"  {dict["Category"]}:   {category}");
            sb.AppendLine($"  {dict["Distance"]}:   {distance:N0} km");
            sb.AppendLine(new string('─', 44));

            if (finalPrice == 0)
            {
                sb.AppendLine();
                sb.AppendLine($"  {dict["FreeTicket"]}");
                sb.AppendLine(age < 12 ? $"  {dict["FreeReason"]}" : $"  {dict["FemaleDiscount"]}");
            }
            else
            {
                sb.AppendLine($"  {dict["OriginalPrice"]}  R{originalPrice:N2}");
                if (!string.IsNullOrEmpty(discountInfo))
                    sb.AppendLine($"  {dict["Discount"]}: {discountInfo}");
                sb.AppendLine(new string('─', 44));
                sb.AppendLine($"  {dict["Total"]}  R{finalPrice:N2}");
            }

            lblSummaryDetails.Text = sb.ToString();
            lblSummaryDetails.ForeColor = TextPri;
            summaryPanel.Visible = true;
            summaryPanel.BringToFront();

            if (historyPanel != null) historyPanel.Visible = false;

            SaveToHistory(txtName.Text, finalPrice);

            autoHideTimer?.Dispose();
            autoHideTimer = new Timer { Interval = 8000 };
            autoHideTimer.Tick += (s, ev) => { summaryPanel.Visible = false; autoHideTimer.Stop(); };
            autoHideTimer.Start();
        }

        //  HISTORY PANEL

        private void SetupHistoryPanel()
        {
            historyPanel = new Panel
            {
                Location = new Point(30, 415),
                Size = new Size(220, 180),
                BackColor = Color.FromArgb(248, 250, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            Label lblHTitle = new Label
            {
                Text = translations[currentLanguage]["📜 History"],
                Location = new Point(0, 0),
                Size = new Size(220, 32)
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = NavyLight,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            lstHistory = new ListBox
            {
                Location = new Point(4, 36),
                Size = new Size(212, 138)
                Font = new Font("Consolas", 8F),
                BorderStyle = BorderStyle.None
            };
            lstHistory.DoubleClick += LstHistory_DoubleClick;

            historyPanel.Controls.AddRange(new Control[] { lblHTitle, lstHistory });
            this.Controls.Add(historyPanel);
            historyPanel.SendToBack();

            btnHistory = new Button
            {
                Text = translations[currentLanguage]["History"],
                Location = new Point(285, 415),
                Size = new Size(74, 32)
            };
            StyleSmallIconButton(btnHistory, NavyLight);
            btnHistory.Text = "📜 History";
            btnHistory.Click += (s, e) => historyPanel.Visible = !historyPanel.Visible;
            this.Controls.Add(btnHistory);
        }

        private void LstHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lstHistory.SelectedItem == null || lstHistory.SelectedIndex >= ticketHistory.Count) return;
            var sel = ticketHistory[lstHistory.SelectedIndex];
            txtName.Text = sel.PassengerName;
            txtAge.Text = sel.Age.ToString();
            txtDistance.Text = sel.Distance.ToString("N0");

            if (sel.Gender == "Female" || sel.Gender == "Vroulik" || sel.Gender == "Ibhinqa")
                rdbFemale.Checked = true;
            else
                rdbMale.Checked = true;

            if (sel.Category.Contains("One") || sel.Category.Contains("Een") || sel.Category.Contains("loku-1")) cmbCategory.SelectedIndex = 0;
            else if (sel.Category.Contains("Two") || sel.Category.Contains("Twee") || sel.Category.Contains("loku-2")) cmbCategory.SelectedIndex = 1;
            else cmbCategory.SelectedIndex = 2;

            var dict = translations[currentLanguage];
            MessageBox.Show(string.Format(dict["LoadSuccess"], sel.PassengerName),
                            dict["History"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveToHistory(string passenger, double price)
        {
            var dict = translations[currentLanguage];
            ticketHistory.Insert(0, new TicketRecord
            {
                PassengerName = passenger,
                Price = price,
                DateTime = DateTime.Now,
                Age = int.Parse(txtAge.Text),
                Gender = rdbFemale.Checked ? dict["Female"] : dict["Male"],
                Category = cmbCategory.SelectedItem.ToString(),
                Distance = double.Parse(txtDistance.Text)
            });

            if (ticketHistory.Count > 10) ticketHistory.RemoveAt(ticketHistory.Count - 1);

            lstHistory.Items.Clear();
            foreach (var t in ticketHistory)
                lstHistory.Items.Add($"{t.DateTime:HH:mm} – {t.PassengerName}: R{t.Price:N2}");
        }

        //  PDF / TXT EXPORT

        private void SetupPDFExport()
        {
            btnExport = new Button
            {
                Text = "💾 Export",
                Location = new Point(365, 415),
                Size = new Size(110, 32)
            };
            StyleSmallIconButton(btnExport, GreenClr);
            btnExport.Click += BtnExport_Click;
            this.Controls.Add(btnExport);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (!ValidateAllInputs()) return;
            using (var sfd = new SaveFileDialog
            {
                Filter = "Text Files|*.txt|All Files|*.*",
                FileName = $"Ticket_{txtName.Text.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}",
                DefaultExt = "txt"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, GenerateTicketContent());
                    var dict = translations[currentLanguage];
                    MessageBox.Show($"{dict["ExportSuccess"]}\n{sfd.FileName}",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private string GenerateTicketContent()
        {
            var dict = translations[currentLanguage];
            var sb = new StringBuilder();
            int age = int.Parse(txtAge.Text);
            double distance = double.Parse(txtDistance.Text);
            string category = cmbCategory.SelectedItem.ToString();
            bool isFemale = rdbFemale.Checked;

            double originalPrice = calculator.CalculateBasePrice(GetInternalCategory(category), distance);
            double finalPrice = calculator.ApplyDiscounts(originalPrice, age, isFemale);

            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"          {dict["Title"]}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"Date:      {DateTime.Now:dddd, MMMM dd, yyyy HH:mm:ss}");
            sb.AppendLine($"Ticket #:  {Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
            sb.AppendLine("-".PadRight(60, '-'));
            sb.AppendLine($"{dict["Passenger"]} {txtName.Text}");
            sb.AppendLine($"{dict["Gender"]}: {(isFemale ? dict["Female"] : dict["Male"])}");
            sb.AppendLine($"{dict["Age"]}: {age} {dict["Years"]}");
            sb.AppendLine($"{dict["Category"]}: {category}");
            sb.AppendLine($"{dict["Distance"]}: {distance:N0} km");
            sb.AppendLine("-".PadRight(60, '-'));
            sb.AppendLine($"{dict["OriginalPrice"]} R{originalPrice:N2}");
            if (age < 12) sb.AppendLine($"{dict["Discount"]}: {dict["FreeReason"]}");
            else if (isFemale) sb.AppendLine($"{dict["Discount"]}: 50% {dict["FemaleDiscount"]}");
            sb.AppendLine($"{dict["Total"]} R{finalPrice:N2}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine("Thank you for choosing our service!");
            sb.AppendLine("Safe travels! ✈️");
            return sb.ToString();
        }

            btnGenerateQR = new Button
            {
        //  QR CODE

                Text = "📱 QR Code",
                Location = new Point(285, 453),
                Size = new Size(190, 20)
        private void SetupQRCodeFeature()
        {
            };
            StyleSmallIconButton(btnGenerateQR, Color.FromArgb(124, 58, 237));
            btnGenerateQR.Click += BtnGenerateQR_Click;
            this.Controls.Add(btnGenerateQR);

            pbQRCode = new PictureBox
            {
                Location = new Point(310, 495),
                Size = new Size(150, 150),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                BackColor = Color.White
            };
            this.Controls.Add(pbQRCode);
            pbQRCode.SendToBack();
        }

        private void BtnGenerateQR_Click(object sender, EventArgs e)
        {
            if (!ValidateAllInputs()) return;
            var dict = translations[currentLanguage];
            try
            {
                using (var qrGen = new QRCodeGenerator())
                {
                    var qrData = qrGen.CreateQrCode(GenerateTicketContent(), QRCodeGenerator.ECCLevel.Q);
                    using (var qr = new QRCode(qrData))
                    {
                        if (pbQRCode.Image != null) pbQRCode.Image.Dispose();
                        pbQRCode.Image = qr.GetGraphic(20);
                        pbQRCode.Visible = true;
                        pbQRCode.BringToFront();

                        var hideTimer = new Timer { Interval = 10000 };
                        hideTimer.Tick += (ts, te) => { pbQRCode.Visible = false; hideTimer.Stop(); };
                        hideTimer.Start();

                        MessageBox.Show(dict["QRSuccess"], "QR Code",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(dict["QRError"], ex.Message),
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  FORM SETUP & VALIDATION

        private void SetupProfessionalStyling()
        {
            this.Text = translations[currentLanguage]["Title"];
            this.BackColor = PageBg;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 720);
            this.MinimumSize = new Size(500, 720);
            this.MaximumSize = new Size(500, 720);
        }

        private void SetupLiveValidation()
        {
            txtName.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtName.Text)) { txtName.BackColor = CardBg; return; }
                txtName.BackColor = System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, @"^[a-zA-Z\s\-']*$")
                    ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 202, 202);
            };
            txtAge.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtAge.Text)) { txtAge.BackColor = CardBg; return; }
                txtAge.BackColor = (int.TryParse(txtAge.Text, out int a) && a >= 0 && a <= 120)
                    ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 202, 202);
            };
            txtDistance.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtDistance.Text)) { txtDistance.BackColor = CardBg; return; }
                txtDistance.BackColor = (double.TryParse(txtDistance.Text, out double d) && d > 0 && d <= 10000)
                    ? Color.FromArgb(220, 252, 231) : Color.FromArgb(254, 202, 202);
            };
        }

        private void SetupKeyboardShortcuts()
        {
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C) btnCalculate.PerformClick();
            else if (e.Control && e.KeyCode == Keys.R) btnClear.PerformClick();
            else if (e.KeyCode == Keys.F1) ShowHelpDialog();
            else if (e.KeyCode == Keys.Escape) btnExit.PerformClick();
        }

        private void ShowHelpDialog()
        {
            var dict = translations[currentLanguage];
            MessageBox.Show(
                $"{dict["Help"]}\n\n" +
                "📝 How to use:\n" +
                "1. Enter passenger name (letters only)\n" +
                "2. Enter age (0–120)\n" +
                "3. Enter distance (1–10 000 km)\n" +
                "4. Select gender and category\n" +
                "5. Click CALCULATE or Ctrl+C\n" +
                "6. Ctrl+R to clear   |   F1 Help   |   ESC Exit\n\n" +
                "💰 Discount rules:\n" +
                "• Under 12: FREE ticket\n" +
                "• Female passengers: 50% off\n\n" +
                "📊 Category pricing:\n" +
                "• Category One:   R20/km\n" +
                "• Category Two:   R35/km\n" +
                "• Category Three: R50/km",
                dict["Help"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateAllInputs()
        {
            ClearErrorHighlights();
            bool isValid = true;
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtName.Text))
            { HighlightError(txtName); errors.AppendLine("• Passenger name is required"); isValid = false; }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, @"^[a-zA-Z\s\-']+$"))
            { HighlightError(txtName); errors.AppendLine("• Name may only contain letters, spaces, hyphens, apostrophes"); isValid = false; }

            if (!int.TryParse(txtAge.Text, out int age))
            { HighlightError(txtAge); errors.AppendLine("• Age must be a valid number"); isValid = false; }
            else if (age < 0 || age > 120)
            { HighlightError(txtAge); errors.AppendLine("• Age must be 0–120"); isValid = false; }

            if (!double.TryParse(txtDistance.Text, out double dist))
            { HighlightError(txtDistance); errors.AppendLine("• Distance must be a valid number"); isValid = false; }
            else if (dist <= 0)
            { HighlightError(txtDistance); errors.AppendLine("• Distance must be greater than 0 km"); isValid = false; }
            else if (dist > 10000)
            { HighlightError(txtDistance); errors.AppendLine("• Distance cannot exceed 10 000 km"); isValid = false; }

            if (!rdbMale.Checked && !rdbFemale.Checked)
            { HighlightError(grpGender); errors.AppendLine("• Please select a gender"); isValid = false; }

            if (cmbCategory.SelectedIndex == -1)
            { HighlightError(cmbCategory); errors.AppendLine("• Please select a travel category"); isValid = false; }

            if (!isValid)
                MessageBox.Show(errors.ToString(), "Validation Errors",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return isValid;
        }

        private void HighlightError(Control c)
        {
            if (c is TextBox tb) tb.BackColor = Color.FromArgb(254, 202, 202);
            else if (c is ComboBox cb) cb.BackColor = Color.FromArgb(254, 202, 202);
            else if (c is GroupBox gb) gb.BackColor = Color.FromArgb(254, 242, 242);
        }

        private void ClearErrorHighlights()
        {
            txtName.BackColor = CardBg;
            txtAge.BackColor = CardBg;
            txtDistance.BackColor = CardBg;
            cmbCategory.BackColor = CardBg;
            grpGender.BackColor = SystemColors.Control;
        }

        //  BUTTON HANDLERS

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!ValidateAllInputs()) return;
            try
            {
                string name = txtName.Text.Trim();
                int age = int.Parse(txtAge.Text);
                double distance = double.Parse(txtDistance.Text);
                string category = cmbCategory.SelectedItem.ToString();
                bool isFemale = rdbFemale.Checked;

                string internalCategory = GetInternalCategory(category);
                double originalPrice = calculator.CalculateBasePrice(internalCategory, distance);
                double finalPrice = calculator.ApplyDiscounts(originalPrice, age, isFemale);
                string discountInfo = GetDiscountInfo(age, isFemale, originalPrice);

                DisplayTicketSummary(finalPrice, originalPrice, discountInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Calculation error: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear(); txtAge.Clear(); txtDistance.Clear();
            rdbMale.Checked = rdbFemale.Checked = false;
            cmbCategory.SelectedIndex = -1;
            summaryPanel.Visible = false;
            if (pbQRCode != null) pbQRCode.Visible = false;
            ClearErrorHighlights();
            txtName.Focus();
            UpdateLivePreview(null, null);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var dict = translations[currentLanguage];
            if (MessageBox.Show(dict["ConfirmExit"], "Confirm Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        //  MULTI-LANGUAGE

        private void ChangeLanguage(string language)
        {
            currentLanguage = language;
            var dict = translations[language];

            this.Text = dict["Title"];
            lblName.Text = dict["Name"];
            lblAge.Text = dict["Age"];
            lblDistance.Text = dict["Distance"];
            grpGender.Text = dict["Gender"];
            rdbMale.Text = dict["Male"];
            rdbFemale.Text = dict["Female"];
            lblCategory.Text = dict["Category"];
            btnCalculate.Text = dict["Calculate"];
            btnClear.Text = dict["Clear"];
            btnExit.Text = dict["Exit"];

            if (lblLivePrice != null) lblLivePrice.Text = dict["LivePrice"];
            if (lblSummaryTitle != null) lblSummaryTitle.Text = dict["TicketSummary"];

            string currentCat = cmbCategory.SelectedItem?.ToString();
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new[] { dict["CategoryOne"], dict["CategoryTwo"], dict["CategoryThree"] });

            if (currentCat != null)
            {
                if (currentCat.Contains("One") || currentCat.Contains("Een") || currentCat.Contains("loku-1")) cmbCategory.SelectedIndex = 0;
                else if (currentCat.Contains("Two") || currentCat.Contains("Twee") || currentCat.Contains("loku-2")) cmbCategory.SelectedIndex = 1;
                else if (currentCat.Contains("Three") || currentCat.Contains("Drie") || currentCat.Contains("loku-3")) cmbCategory.SelectedIndex = 2;
            }

            if (summaryPanel != null && summaryPanel.Visible && ValidateAllInputs())
                RecalculateAndDisplay();
        }

        private void RecalculateAndDisplay()
        {
            try
            {
                int age = int.Parse(txtAge.Text);
                double distance = double.Parse(txtDistance.Text);
                bool isFemale = rdbFemale.Checked;
                string internal_ = GetInternalCategory(cmbCategory.SelectedItem.ToString());
                double orig = calculator.CalculateBasePrice(internal_, distance);
                double final = calculator.ApplyDiscounts(orig, age, isFemale);
                DisplayTicketSummary(final, orig, GetDiscountInfo(age, isFemale, orig));
            }
            catch { }
        }

        private string GetInternalCategory(string display)
        {
            if (display.Contains("Two") || display.Contains("Twee") || display.Contains("loku-2")) return "Category Two";
            if (display.Contains("Three") || display.Contains("Drie") || display.Contains("loku-3")) return "Category Three";
            return "Category One";
        }

        private string GetDiscountInfo(int age, bool isFemale, double originalPrice)
        {
            if (age < 12) return translations[currentLanguage]["FreeReason"];
            if (isFemale) return $"50% {translations[currentLanguage]["FemaleDiscount"]} → -R{originalPrice * 0.5:N2}";
            return "";
        }

        //  TRANSLATIONS (unchanged from original)

        private void InitializeTranslations()
        {
            translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["English"] = new Dictionary<string, string>
                {
                    {"Title","✈️ Ticket Application System"},{"Name","Full Name:"},{"Age","Age:"},
                    {"Distance","Distance (km):"},{"Gender","Gender"},{"Male","Male"},{"Female","Female"},
                    {"Category","Category:"},{"Calculate","CALCULATE"},{"Clear","CLEAR"},{"Exit","EXIT"},
                    {"Theme","🌙 Dark Mode"},{"History","📜 History"},{"Export","💾 Export Ticket"},
                    {"QR","📱 Generate QR"},{"TicketSummary","🎫  TICKET SUMMARY"},
                    {"FreeTicket","💰  FINAL PRICE: FREE TICKET! 🎉"},{"FreeReason","(Age < 12 — free travel)"},
                    {"FemaleDiscount","(50% female discount applied)"},{"OriginalPrice","Original Price:"},
                    {"Discount","Discount:"},{"Total","💰  TOTAL AMOUNT:"},
                    {"ExportSuccess","Ticket saved successfully!"},
                    {"ConfirmExit","Are you sure you want to exit?"},{"Help","Help — Ticket System"},
                    {"Passenger","Passenger:"},{"Years","years"},
                    {"CategoryOne","Category One"},{"CategoryTwo","Category Two"},{"CategoryThree","Category Three"},
                    {"LivePrice","💵  Enter details to see price"},{"LivePriceFree","🎉  FREE TICKET!"},
                    {"LivePriceDiscount","💰  Preview: R{0:N2}  (50% off!)"},{"LivePriceNormal","💰  Preview: R{0:N2}"},
                    {"LoadSuccess","Loaded ticket for {0}"},
                    {"QRSuccess","QR Code generated!\nScan to view ticket details."},
                    {"QRError","QR Code error: {0}\n\nInstall-Package QRCoder"}
                },
                ["Afrikaans"] = new Dictionary<string, string>
                {
                    {"Title","✈️ Kaartjie Stelsel"},{"Name","Volle Naam:"},{"Age","Ouderdom:"},
                    {"Distance","Afstand (km):"},{"Gender","Geslag"},{"Male","Manlik"},{"Female","Vroulik"},
                    {"Category","Kategorie:"},{"Calculate","BEREKEN"},{"Clear","SKOONMAAK"},{"Exit","SLUIT"},
                    {"Theme","🌙 Donker Modus"},{"History","📜 Geskiedenis"},{"Export","💾 Stoor Ticket"},
                    {"QR","📱 Genereer QR"},{"TicketSummary","🎫  KAARTJIE OPSOMMING"},
                    {"FreeTicket","💰  FINALE PRYS: GRATIS! 🎉"},{"FreeReason","(Ouderdom < 12 — gratis reis)"},
                    {"FemaleDiscount","(50% vroulike afslag)"},{"OriginalPrice","Oorspronklike Prys:"},
                    {"Discount","Afslag:"},{"Total","💰  TOTALE BEDRAG:"},
                    {"ExportSuccess","Kaartjie suksesvol gestoor!"},
                    {"ConfirmExit","Is u seker u wil sluit?"},{"Help","Hulp — Kaartjie Stelsel"},
                    {"Passenger","Passasier:"},{"Years","jaar"},
                    {"CategoryOne","Kategorie Een"},{"CategoryTwo","Kategorie Twee"},{"CategoryThree","Kategorie Drie"},
                    {"LivePrice","💵  Voer besonderhede in"},{"LivePriceFree","🎉  GRATIS KAARTJIE!"},
                    {"LivePriceDiscount","💰  Voorskou: R{0:N2}  (50% af!)"},{"LivePriceNormal","💰  Voorskou: R{0:N2}"},
                    {"LoadSuccess","Kaartjie gelaai vir {0}"},
                    {"QRSuccess","QR kode gegenereer!\nSkandeer vir besonderhede."},
                    {"QRError","QR fout: {0}\n\nInstall-Package QRCoder"}
                },
                ["Xhosa"] = new Dictionary<string, string>
                {
                    {"Title","✈️ Inkqubo yamatikithi"},{"Name","Igama elipheleleyo:"},{"Age","Ubudala:"},
                    {"Distance","Umgama (km):"},{"Gender","Isini"},{"Male","Indoda"},{"Female","Ibhinqa"},
                    {"Category","Udidi:"},{"Calculate","BALA"},{"Clear","COCA"},{"Exit","PHUMA"},
                    {"Theme","🌙 Imowudi emnyama"},{"History","📜 Imbali"},{"Export","💾 Gcina Itikiti"},
                    {"QR","📱 Yenza iQR"},{"TicketSummary","🎫  ISISHWANKATHELO SETIKITI"},
                    {"FreeTicket","💰  IXABISO: TIKITI LASIMAHALA! 🎉"},{"FreeReason","(Ubudala < 12 — simahla)"},
                    {"FemaleDiscount","(50% isaphulelo sabasetyhini)"},{"OriginalPrice","Ixabiso loqobo:"},
                    {"Discount","Isaphulelo:"},{"Total","💰  ISIYONO SEZIMALI:"},
                    {"ExportSuccess","Itikiti ligcinwe!"},
                    {"ConfirmExit","Uqinisekile ukuba ufuna ukuphuma?"},{"Help","Uncedo — Inkqubo yamatikithi"},
                    {"Passenger","Umkhweli:"},{"Years","iminyaka"},
                    {"CategoryOne","Udidi loku-1"},{"CategoryTwo","Udidi loku-2"},{"CategoryThree","Udidi loku-3"},
                    {"LivePrice","💵  Faka iinkcukacha"},{"LivePriceFree","🎉  TIKITI LASIMAHALA!"},
                    {"LivePriceDiscount","💰  Ujongo: R{0:N2}  (50% isaphulelo!)"},{"LivePriceNormal","💰  Ujongo: R{0:N2}"},
                    {"LoadSuccess","Itikiti lilayishwe ku {0}"},
                    {"QRSuccess","IKhowudi yeQR yenziwe!\nSkena ukubona iinkcukacha."},
                    {"QRError","Impazamo yeQR: {0}\n\nInstall-Package QRCoder"}
                }
            };
        }
    }

    public class TicketRecord
    {
        public string PassengerName { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Category { get; set; }
        public double Distance { get; set; }
    }
}
