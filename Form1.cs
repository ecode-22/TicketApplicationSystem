using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using QRCoder; // Install-Package QRCoder

namespace TicketApplicationSystem
{
    public partial class Form1 : Form
    {
        private TicketCalculator calculator;
        private Panel summaryPanel;
        private Label lblSummaryTitle;
        private Label lblSummaryDetails;
        private Timer autoHideTimer;

        // Dark/Light Theme
        private bool isDarkMode = false;
        private Button btnTheme;

        // Ticket History
        private List<TicketRecord> ticketHistory = new List<TicketRecord>();
        private ListBox lstHistory;
        private Panel historyPanel;
        private Button btnHistory;

        // Multi-Language Support
        private string currentLanguage = "English";
        private Dictionary<string, Dictionary<string, string>> translations;
        private ComboBox cmbLanguage;

        // QR Code
        private PictureBox pbQRCode;
        private Button btnGenerateQR;

        // PDF Export
        private Button btnExport;

        // Live Price Preview
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
            SetupThemeToggle();
            SetupLanguageSelector();
            SetupHistoryPanel();
            SetupPDFExport();
            SetupQRCodeFeature();
            SetupLivePricePreview();
        }

        #region Multi-Language Support (English, Afrikaans, Xhosa)

        private void InitializeTranslations()
        {
            translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["English"] = new Dictionary<string, string>
                {
                    {"Title", "✈️ Ticket Application System"},
                    {"Name", "Full Name:"},
                    {"Age", "Age:"},
                    {"Distance", "Distance (km):"},
                    {"Gender", "Gender"},
                    {"Male", "Male"},
                    {"Female", "Female"},
                    {"Category", "Category:"},
                    {"Calculate", "CALCULATE"},
                    {"Clear", "CLEAR"},
                    {"Exit", "EXIT"},
                    {"Theme", "🌙 Dark Mode"},
                    {"History", "📜 History"},
                    {"Export", "💾 Export Ticket"},
                    {"QR", "📱 Generate QR"},
                    {"TicketSummary", "🎫 TICKET SUMMARY"},
                    {"FreeTicket", "💰 FINAL PRICE: FREE TICKET! 🎉"},
                    {"FreeReason", "   (Age < 12 - Free travel)"},
                    {"FemaleDiscount", "   (50% Female Discount Applied)"},
                    {"OriginalPrice", "Original Price:"},
                    {"Discount", "Discount:"},
                    {"Total", "💰 TOTAL AMOUNT:"},
                    {"ExportSuccess", "Ticket saved successfully!"},
                    {"ConfirmExit", "Are you sure you want to exit the application?"},
                    {"Help", "Help - Ticket System"},
                    {"Passenger", "Passenger:"},
                    {"Years", "years"},
                    {"CategoryOne", "Category One"},
                    {"CategoryTwo", "Category Two"},
                    {"CategoryThree", "Category Three"},
                    {"LivePrice", "💵 Enter details to see price"},
                    {"LivePriceFree", "🎉 FREE TICKET! 🎉"},
                    {"LivePriceDiscount", "💰 Preview: R{0:N2} (50% off!)"},
                    {"LivePriceNormal", "💰 Preview: R{0:N2}"},
                    {"LoadSuccess", "Loaded ticket for {0}"},
                    {"QRSuccess", "QR Code generated successfully!\nScan to view ticket details."},
                    {"QRError", "QR Code generation error: {0}\n\nMake sure to install QRCoder NuGet package:\nInstall-Package QRCoder"}
                },

                ["Afrikaans"] = new Dictionary<string, string>
                {
                    {"Title", "✈️ Kaartjie Stelsel"},
                    {"Name", "Volle Naam:"},
                    {"Age", "Ouderdom:"},
                    {"Distance", "Afstand (km):"},
                    {"Gender", "Geslag"},
                    {"Male", "Manlik"},
                    {"Female", "Vroulik"},
                    {"Category", "Kategorie:"},
                    {"Calculate", "BEREKEN"},
                    {"Clear", "SKOONMAAK"},
                    {"Exit", "SLUIT"},
                    {"Theme", "🌙 Donker Modus"},
                    {"History", "📜 Geskiedenis"},
                    {"Export", "💾 Stoor Ticket"},
                    {"QR", "📱 Genereer QR"},
                    {"TicketSummary", "🎫 KAARTJIE OPSOMMING"},
                    {"FreeTicket", "💰 FINALE PRYS: GRATIS KAARTJIE! 🎉"},
                    {"FreeReason", "   (Ouderdom < 12 - Gratis reis)"},
                    {"FemaleDiscount", "   (50% Vroulike Afslag Toegepas)"},
                    {"OriginalPrice", "Oorspronklike Prys:"},
                    {"Discount", "Afslag:"},
                    {"Total", "💰 TOTALE BEDRAG:"},
                    {"ExportSuccess", "Kaartjie suksesvol gestoor!"},
                    {"ConfirmExit", "Is u seker u wil die program sluit?"},
                    {"Help", "Hulp - Kaartjie Stelsel"},
                    {"Passenger", "Passasier:"},
                    {"Years", "jaar"},
                    {"CategoryOne", "Kategorie Een"},
                    {"CategoryTwo", "Kategorie Twee"},
                    {"CategoryThree", "Kategorie Drie"},
                    {"LivePrice", "💵 Voer besonderhede in om prys te sien"},
                    {"LivePriceFree", "🎉 GRATIS KAARTJIE! 🎉"},
                    {"LivePriceDiscount", "💰 Voorskou: R{0:N2} (50% af!)"},
                    {"LivePriceNormal", "💰 Voorskou: R{0:N2}"},
                    {"LoadSuccess", "Kaartjie gelaai vir {0}"},
                    {"QRSuccess", "QR kode suksesvol gegenereer!\nSkandeer om kaartjie besonderhede te sien."},
                    {"QRError", "QR kode generering fout: {0}\n\nMaak seker om QRCoder NuGet pakket te installeer:\nInstall-Package QRCoder"}
                },

                ["Xhosa"] = new Dictionary<string, string>
                {
                    {"Title", "✈️ Inkqubo yamatikithi"},
                    {"Name", "Igama elipheleleyo:"},
                    {"Age", "Ubudala:"},
                    {"Distance", "Umgama (km):"},
                    {"Gender", "Isini"},
                    {"Male", "Indoda"},
                    {"Female", "Ibhinqa"},
                    {"Category", "Udidi:"},
                    {"Calculate", "BALA"},
                    {"Clear", "COCA"},
                    {"Exit", "PHUMA"},
                    {"Theme", "🌙 Imowudi emnyama"},
                    {"History", "📜 Imbali"},
                    {"Export", "💾 Gcina Itikiti"},
                    {"QR", "📱 Yenza iQR"},
                    {"TicketSummary", "🎫 ISISHWANKATHELO SETIKITI"},
                    {"FreeTicket", "💰 IXABISO LOKUGQIBELA: TIKITI LASIMAHALA! 🎉"},
                    {"FreeReason", "   (Ubudala < 12 - Ukuhamba simahla)"},
                    {"FemaleDiscount", "   (50% Isaphulelo sabasetyhini)"},
                    {"OriginalPrice", "Ixabiso loqobo:"},
                    {"Discount", "Isaphulelo:"},
                    {"Total", "💰 ISIYONO SEZIMALI:"},
                    {"ExportSuccess", "Itikiti ligcinwe ngempumelelo!"},
                    {"ConfirmExit", "Uqinisekile ukuba ufuna ukuphuma kule nkqubo?"},
                    {"Help", "Uncedo - Inkqubo yamatikithi"},
                    {"Passenger", "Umkhweli:"},
                    {"Years", "iminyaka"},
                    {"CategoryOne", "Udidi loku-1"},
                    {"CategoryTwo", "Udidi loku-2"},
                    {"CategoryThree", "Udidi loku-3"},
                    {"LivePrice", "💵 Faka iinkcukacha ukubona ixabiso"},
                    {"LivePriceFree", "🎉 TIKITI LASIMAHALA! 🎉"},
                    {"LivePriceDiscount", "💰 Ujongo lwangaphambili: R{0:N2} (50% isaphulelo!)"},
                    {"LivePriceNormal", "💰 Ujongo lwangaphambili: R{0:N2}"},
                    {"LoadSuccess", "Itikiti lilayishwe ku {0}"},
                    {"QRSuccess", "IKhowudi yeQR yenziwe ngempumelelo!\nSkena ukubona iinkcukacha zetikiti."},
                    {"QRError", "Impazamo yokwenza iQR: {0}\n\nQinisekisa ukufaka iQRCoder NuGet package:\nInstall-Package QRCoder"}
                }
            };
        }

        private void SetupLanguageSelector()
        {
            cmbLanguage = new ComboBox
            {
                Location = new Point(380, 15),
                Size = new Size(90, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.White
            };
            cmbLanguage.Items.AddRange(new[] { "English", "Afrikaans", "Xhosa" });
            cmbLanguage.SelectedIndex = 0;
            cmbLanguage.SelectedIndexChanged += (s, e) => ChangeLanguage(cmbLanguage.SelectedItem.ToString());
            this.Controls.Add(cmbLanguage);
            cmbLanguage.BringToFront();
        }

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

            if (btnTheme != null) btnTheme.Text = isDarkMode ? dict["Theme"].Replace("🌙 ", "☀️ ") : dict["Theme"];
            if (btnHistory != null) btnHistory.Text = dict["History"];
            if (btnExport != null) btnExport.Text = dict["Export"];
            if (btnGenerateQR != null) btnGenerateQR.Text = dict["QR"];
            if (lblLivePrice != null) lblLivePrice.Text = dict["LivePrice"];
            if (lblSummaryTitle != null) lblSummaryTitle.Text = dict["TicketSummary"];

            string currentCategory = cmbCategory.SelectedItem?.ToString();
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new[] { dict["CategoryOne"], dict["CategoryTwo"], dict["CategoryThree"] });

            if (currentCategory != null)
            {
                if (currentCategory.Contains("One") || currentCategory.Contains("Een") || currentCategory.Contains("loku-1"))
                    cmbCategory.SelectedIndex = 0;
                else if (currentCategory.Contains("Two") || currentCategory.Contains("Twee") || currentCategory.Contains("loku-2"))
                    cmbCategory.SelectedIndex = 1;
                else if (currentCategory.Contains("Three") || currentCategory.Contains("Drie") || currentCategory.Contains("loku-3"))
                    cmbCategory.SelectedIndex = 2;
            }

            if (summaryPanel != null && summaryPanel.Visible && !string.IsNullOrEmpty(lblSummaryDetails.Text))
            {
                if (ValidateAllInputs())
                {
                    RecalculateAndDisplay();
                }
            }
        }

        private void RecalculateAndDisplay()
        {
            try
            {
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
            catch { }
        }

        private string GetInternalCategory(string displayCategory)
        {
            if (displayCategory.Contains("Two") || displayCategory.Contains("Twee") || displayCategory.Contains("loku-2"))
                return "Category Two";
            if (displayCategory.Contains("Three") || displayCategory.Contains("Drie") || displayCategory.Contains("loku-3"))
                return "Category Three";
            return "Category One";
        }

        private string GetDiscountInfo(int age, bool isFemale, double originalPrice)
        {
            if (age < 12) return translations[currentLanguage]["FreeReason"];
            if (isFemale) return $"50% {translations[currentLanguage]["FemaleDiscount"]} → -R{originalPrice * 0.5:N2}";
            return "";
        }

        #endregion

        #region Dark/Light Theme

        private void SetupThemeToggle()
        {
            btnTheme = new Button
            {
                Text = translations[currentLanguage]["Theme"],
                Location = new Point(280, 15),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnTheme.Click += BtnTheme_Click;
            this.Controls.Add(btnTheme);
            btnTheme.BringToFront();
        }

        private void BtnTheme_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            var dict = translations[currentLanguage];

            if (isDarkMode)
            {
                btnTheme.Text = "☀️ " + dict["Theme"].Replace("🌙 ", "");
                this.BackColor = Color.FromArgb(44, 62, 80);

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Label lbl)
                        lbl.ForeColor = Color.White;
                    else if (ctrl is GroupBox grp)
                    {
                        grp.ForeColor = Color.White;
                        grp.BackColor = Color.FromArgb(44, 62, 80);
                        foreach (Control inner in grp.Controls)
                            if (inner is RadioButton rb)
                            {
                                rb.ForeColor = Color.White;
                                rb.BackColor = Color.Transparent;
                            }
                    }
                    else if (ctrl is Button btn && btn != btnTheme && btn != btnHistory && btn != btnExport && btn != btnGenerateQR)
                    {
                        if (btn.BackColor == Color.FromArgb(52, 152, 219))
                            btn.BackColor = Color.FromArgb(41, 128, 185);
                    }
                }

                txtName.BackColor = Color.FromArgb(52, 73, 94);
                txtName.ForeColor = Color.White;
                txtAge.BackColor = Color.FromArgb(52, 73, 94);
                txtAge.ForeColor = Color.White;
                txtDistance.BackColor = Color.FromArgb(52, 73, 94);
                txtDistance.ForeColor = Color.White;
                cmbCategory.BackColor = Color.FromArgb(52, 73, 94);
                cmbCategory.ForeColor = Color.White;
                cmbLanguage.BackColor = Color.FromArgb(52, 73, 94);
                cmbLanguage.ForeColor = Color.White;

                if (historyPanel != null)
                {
                    historyPanel.BackColor = Color.FromArgb(44, 62, 80);
                    lstHistory.BackColor = Color.FromArgb(52, 73, 94);
                    lstHistory.ForeColor = Color.White;
                }

                if (lblLivePrice != null) lblLivePrice.BackColor = Color.FromArgb(52, 73, 94);
                if (summaryPanel != null) summaryPanel.BackColor = Color.FromArgb(44, 62, 80);
            }
            else
            {
                btnTheme.Text = "🌙 " + dict["Theme"];
                this.BackColor = Color.FromArgb(240, 248, 255);

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Label lbl)
                        lbl.ForeColor = SystemColors.ControlText;
                    else if (ctrl is GroupBox grp)
                    {
                        grp.ForeColor = SystemColors.ControlText;
                        grp.BackColor = SystemColors.Control;
                        foreach (Control inner in grp.Controls)
                            if (inner is RadioButton rb)
                            {
                                rb.ForeColor = SystemColors.ControlText;
                                rb.BackColor = Color.Transparent;
                            }
                    }
                    else if (ctrl is Button btn && btn != btnTheme && btn != btnHistory && btn != btnExport && btn != btnGenerateQR)
                    {
                        if (btn.BackColor == Color.FromArgb(41, 128, 185))
                            btn.BackColor = Color.FromArgb(52, 152, 219);
                    }
                }

                txtName.BackColor = SystemColors.Window;
                txtName.ForeColor = SystemColors.ControlText;
                txtAge.BackColor = SystemColors.Window;
                txtAge.ForeColor = SystemColors.ControlText;
                txtDistance.BackColor = SystemColors.Window;
                txtDistance.ForeColor = SystemColors.ControlText;
                cmbCategory.BackColor = SystemColors.Window;
                cmbCategory.ForeColor = SystemColors.ControlText;
                cmbLanguage.BackColor = Color.White;
                cmbLanguage.ForeColor = SystemColors.ControlText;

                if (historyPanel != null)
                {
                    historyPanel.BackColor = Color.FromArgb(236, 240, 241);
                    lstHistory.BackColor = Color.White;
                    lstHistory.ForeColor = SystemColors.ControlText;
                }

                if (lblLivePrice != null) lblLivePrice.BackColor = Color.FromArgb(255, 243, 205);
                if (summaryPanel != null) summaryPanel.BackColor = Color.FromArgb(236, 240, 241);
            }
        }

        #endregion

        #region Ticket History

        private void SetupHistoryPanel()
        {
            historyPanel = new Panel
            {
                Location = new Point(30, 440),
                Size = new Size(200, 150),
                BackColor = Color.FromArgb(236, 240, 241),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            Label lblHistoryTitle = new Label
            {
                Text = translations[currentLanguage]["History"],
                Location = new Point(0, 0),
                Size = new Size(198, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            lstHistory = new ListBox
            {
                Location = new Point(5, 35),
                Size = new Size(190, 110),
                Font = new Font("Consolas", 8)
            };
            lstHistory.DoubleClick += LstHistory_DoubleClick;

            historyPanel.Controls.AddRange(new Control[] { lblHistoryTitle, lstHistory });
            this.Controls.Add(historyPanel);
            historyPanel.SendToBack();

            btnHistory = new Button
            {
                Text = translations[currentLanguage]["History"],
                Location = new Point(280, 55),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnHistory.Click += (s, e) => historyPanel.Visible = !historyPanel.Visible;
            this.Controls.Add(btnHistory);
        }

        private void LstHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lstHistory.SelectedItem != null && lstHistory.SelectedIndex < ticketHistory.Count)
            {
                var selected = ticketHistory[lstHistory.SelectedIndex];
                txtName.Text = selected.PassengerName;
                txtAge.Text = selected.Age.ToString();
                txtDistance.Text = selected.Distance.ToString("N0");

                string femaleText = translations[currentLanguage]["Female"];
                if (selected.Gender == femaleText || selected.Gender == "Female" || selected.Gender == "Vroulik" || selected.Gender == "Ibhinqa")
                    rdbFemale.Checked = true;
                else
                    rdbMale.Checked = true;

                var dict = translations[currentLanguage];
                if (selected.Category.Contains("One") || selected.Category.Contains("Een") || selected.Category.Contains("loku-1"))
                    cmbCategory.SelectedIndex = 0;
                else if (selected.Category.Contains("Two") || selected.Category.Contains("Twee") || selected.Category.Contains("loku-2"))
                    cmbCategory.SelectedIndex = 1;
                else
                    cmbCategory.SelectedIndex = 2;

                MessageBox.Show(string.Format(dict["LoadSuccess"], selected.PassengerName),
                               dict["History"], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveToHistory(string passenger, double price)
        {
            var dict = translations[currentLanguage];
            TicketRecord record = new TicketRecord
            {
                PassengerName = passenger,
                Price = price,
                DateTime = DateTime.Now,
                Age = int.Parse(txtAge.Text),
                Gender = rdbFemale.Checked ? dict["Female"] : dict["Male"],
                Category = cmbCategory.SelectedItem.ToString(),
                Distance = double.Parse(txtDistance.Text)
            };

            ticketHistory.Insert(0, record);

            if (ticketHistory.Count > 10)
                ticketHistory.RemoveAt(ticketHistory.Count - 1);

            lstHistory.Items.Clear();
            foreach (var t in ticketHistory)
            {
                lstHistory.Items.Add($"{t.DateTime:HH:mm:ss} - {t.PassengerName}: R{t.Price:N2}");
            }
        }

        #endregion

        #region PDF Export

        private void SetupPDFExport()
        {
            btnExport = new Button
            {
                Text = translations[currentLanguage]["Export"],
                Location = new Point(280, 95),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnExport.Click += BtnExport_Click;
            this.Controls.Add(btnExport);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (!ValidateAllInputs()) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files|*.txt|All Files|*.*";
                sfd.FileName = $"Ticket_{txtName.Text.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}";
                sfd.DefaultExt = "txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string ticketContent = GenerateTicketContent();
                    File.WriteAllText(sfd.FileName, ticketContent);

                    var dict = translations[currentLanguage];
                    MessageBox.Show($"{dict["ExportSuccess"]}\n{sfd.FileName}",
                                   "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private string GenerateTicketContent()
        {
            var dict = translations[currentLanguage];
            StringBuilder sb = new StringBuilder();

            int age = int.Parse(txtAge.Text);
            double distance = double.Parse(txtDistance.Text);
            string category = cmbCategory.SelectedItem.ToString();
            string internalCategory = GetInternalCategory(category);
            bool isFemale = rdbFemale.Checked;

            double originalPrice = calculator.CalculateBasePrice(internalCategory, distance);
            double finalPrice = calculator.ApplyDiscounts(originalPrice, age, isFemale);

            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"          {dict["Title"]}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine($"Date: {DateTime.Now:dddd, MMMM dd, yyyy HH:mm:ss}");
            sb.AppendLine($"Ticket #: {Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}");
            sb.AppendLine("-".PadRight(60, '-'));
            sb.AppendLine($"{dict["Passenger"]} {txtName.Text}");
            sb.AppendLine($"{dict["Gender"]}: {(isFemale ? dict["Female"] : dict["Male"])}");
            sb.AppendLine($"{dict["Age"]}: {age} {dict["Years"]}");
            sb.AppendLine($"{dict["Category"]}: {category}");
            sb.AppendLine($"{dict["Distance"]}: {distance:N0} km");
            sb.AppendLine("-".PadRight(60, '-'));
            sb.AppendLine($"{dict["OriginalPrice"]} R{originalPrice:N2}");

            if (age < 12)
                sb.AppendLine($"{dict["Discount"]}: {dict["FreeReason"]}");
            else if (isFemale)
                sb.AppendLine($"{dict["Discount"]}: 50% {dict["FemaleDiscount"]}");

            sb.AppendLine($"{dict["Total"]} R{finalPrice:N2}");
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine("Thank you for choosing our service!");
            sb.AppendLine("Safe travels! ✈️");

            return sb.ToString();
        }

        #endregion

        #region QR Code Generation

        private void SetupQRCodeFeature()
        {
            btnGenerateQR = new Button
            {
                Text = translations[currentLanguage]["QR"],
                Location = new Point(280, 135),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnGenerateQR.Click += BtnGenerateQR_Click;
            this.Controls.Add(btnGenerateQR);

            pbQRCode = new PictureBox
            {
                Location = new Point(310, 440),
                Size = new Size(140, 140),
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
                string ticketData = GenerateTicketContent();

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(ticketData, QRCodeGenerator.ECCLevel.Q);
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        Bitmap qrBitmap = qrCode.GetGraphic(20);
                        if (pbQRCode.Image != null) pbQRCode.Image.Dispose();
                        pbQRCode.Image = qrBitmap;
                        pbQRCode.Visible = true;
                        pbQRCode.BringToFront();

                        Timer hideTimer = new Timer();
                        hideTimer.Interval = 10000;
                        hideTimer.Tick += (ts, te) =>
                        {
                            pbQRCode.Visible = false;
                            hideTimer.Stop();
                        };
                        hideTimer.Start();

                        MessageBox.Show(string.Format(dict["QRSuccess"], dict["QR"]),
                                      "QR Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(dict["QRError"], ex.Message),
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Live Price Preview

        private void SetupLivePricePreview()
        {
            lblLivePrice = new Label
            {
                Location = new Point(150, 355),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = translations[currentLanguage]["LivePrice"],
                BackColor = Color.FromArgb(255, 243, 205),
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
                    double.TryParse(txtDistance.Text, out double distance) &&
                    distance > 0 &&
                    int.TryParse(txtAge.Text, out int age))
                {
                    string category = cmbCategory.SelectedItem.ToString();
                    string internalCategory = GetInternalCategory(category);
                    double basePrice = calculator.CalculateBasePrice(internalCategory, distance);
                    double finalPrice = calculator.ApplyDiscounts(basePrice, age, rdbFemale.Checked);

                    if (age < 12)
                    {
                        lblLivePrice.Text = dict["LivePriceFree"];
                        lblLivePrice.BackColor = Color.FromArgb(200, 230, 200);
                    }
                    else if (rdbFemale.Checked)
                    {
                        lblLivePrice.Text = string.Format(dict["LivePriceDiscount"], finalPrice);
                        lblLivePrice.BackColor = Color.FromArgb(200, 230, 200);
                    }
                    else
                    {
                        lblLivePrice.Text = string.Format(dict["LivePriceNormal"], finalPrice);
                        lblLivePrice.BackColor = Color.FromArgb(200, 230, 200);
                    }
                }
                else
                {
                    lblLivePrice.Text = dict["LivePrice"];
                    lblLivePrice.BackColor = Color.FromArgb(255, 243, 205);
                }
            }
            catch
            {
                lblLivePrice.Text = dict["LivePrice"];
                lblLivePrice.BackColor = Color.FromArgb(255, 243, 205);
            }
        }

        #endregion

        #region Main Form Methods

        private void SetupProfessionalStyling()
        {
            var dict = translations[currentLanguage];
            this.Text = dict["Title"];
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 680);
            this.MinimumSize = new Size(500, 680);
            this.MaximumSize = new Size(500, 680);
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
            var dict = translations[currentLanguage];
            summaryPanel = new Panel
            {
                Location = new Point(30, 440),
                Size = new Size(420, 150),
                BackColor = Color.FromArgb(236, 240, 241),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            lblSummaryTitle = new Label
            {
                Text = dict["TicketSummary"],
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(418, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White
            };

            lblSummaryDetails = new Label
            {
                Location = new Point(10, 38),
                Size = new Size(400, 100),
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
            var dict = translations[currentLanguage];
            MessageBox.Show($"{dict["Help"]}\n\n" +
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
                           "• Female passengers: 50% discount\n\n" +
                           "📊 Category Pricing:\n" +
                           "• Category One: R20/km\n" +
                           "• Category Two: R35/km\n" +
                           "• Category Three: R50/km",
                           dict["Help"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateAllInputs()
        {
            ClearErrorHighlights();
            bool isValid = true;
            StringBuilder errorMessage = new StringBuilder();

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

            if (!rdbMale.Checked && !rdbFemale.Checked)
            {
                HighlightError(grpGender);
                errorMessage.AppendLine("• Please select a gender");
                isValid = false;
            }

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
                txt.BackColor = Color.LightPink;
            else if (control is ComboBox cmb)
                cmb.BackColor = Color.LightPink;
            else if (control is GroupBox grp)
                grp.BackColor = Color.LightPink;
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
            var dict = translations[currentLanguage];
            string gender = rdbFemale.Checked ? dict["Female"] : dict["Male"];
            string category = cmbCategory.SelectedItem.ToString();
            double distance = double.Parse(txtDistance.Text);
            int age = int.Parse(txtAge.Text);

            StringBuilder summaryText = new StringBuilder();
            summaryText.AppendLine($"{dict["Passenger"]} {txtName.Text}");
            summaryText.AppendLine($"{dict["Gender"]}: {gender}");
            summaryText.AppendLine($"{dict["Age"]}: {age} {dict["Years"]}");
            summaryText.AppendLine($"{dict["Category"]}: {category}");
            summaryText.AppendLine($"{dict["Distance"]}: {distance:N0} km");
            summaryText.AppendLine(new string('-', 35));

            if (finalPrice == 0)
            {
                summaryText.AppendLine();
                summaryText.AppendLine(dict["FreeTicket"]);
                if (age < 12)
                    summaryText.AppendLine(dict["FreeReason"]);
                else if (discountInfo.Contains("Female"))
                    summaryText.AppendLine(dict["FemaleDiscount"]);
            }
            else
            {
                summaryText.AppendLine();
                summaryText.AppendLine($"{dict["OriginalPrice"]} R{originalPrice:N2}");
                if (!string.IsNullOrEmpty(discountInfo))
                    summaryText.AppendLine($"{dict["Discount"]}: {discountInfo}");
                summaryText.AppendLine(new string('-', 35));
                summaryText.AppendLine($"{dict["Total"]} R{finalPrice:N2}");
            }

            lblSummaryDetails.Text = summaryText.ToString();
            summaryPanel.Visible = true;
            summaryPanel.BringToFront();

            // Hide history panel when showing summary
            if (historyPanel != null) historyPanel.Visible = false;

            SaveToHistory(txtName.Text, finalPrice);

            if (autoHideTimer != null)
                autoHideTimer.Dispose();

            autoHideTimer = new Timer();
            autoHideTimer.Interval = 8000;
            autoHideTimer.Tick += (s, e) => { summaryPanel.Visible = false; autoHideTimer.Stop(); };
            autoHideTimer.Start();
        }

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
            txtName.Clear();
            txtAge.Clear();
            txtDistance.Clear();
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
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
            DialogResult result = MessageBox.Show(dict["ConfirmExit"],
                                                   "Confirm Exit",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion
    }

    // TicketRecord class for history
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