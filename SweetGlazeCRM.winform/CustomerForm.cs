
using System.ComponentModel;
using System.Net.Http;
using System.Net.Mail;
using SweetGlazeCRM.winform.Models;
using SweetGlazeCRM.winform.Services;

namespace SweetGlazeCRM.winform
{
    public partial class CustomerForm : Form
    {
        private readonly CustomerApiService _apiService;

        private readonly BindingList<Customer> _customers = new();

        private readonly List<Customer> _allCustomers = new();

        private int? _selectedCustomerId = null;

        private TextBox txtFirstName = null!;
        private TextBox txtLastName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private TextBox txtCompany = null!;
        private TextBox txtAddress = null!;
        private TextBox txtNotes = null!;

        private TextBox txtSearch = null!;

        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnClear = null!;
        private Button btnRefresh = null!;

        private DataGridView dgvCustomers = null!;

        private Label lblSelectedCustomer = null!;
        private Label lblStatus = null!;
        private Label lblTotalCustomers = null!;
        private Label lblActiveCustomers = null!;
        private Label lblPageTitle = null!;
        private Label lblPageSubtitle = null!;

        private Panel contentPanel = null!;

        private Color PrimaryColor =
            Color.FromArgb(42, 52, 72);

        private Color AccentColor =
            Color.FromArgb(230, 170, 75);

        private Color BackgroundColor =
            Color.FromArgb(246, 247, 250);

        private Color TextColor =
            Color.FromArgb(35, 45, 65);

        private Color MutedColor =
            Color.FromArgb(110, 120, 135);

        public CustomerForm()
        {
            InitializeComponent();

            _apiService = new CustomerApiService();

            BuildInterface();

            Load += CustomerForm_Load;
        }

        private void BuildInterface()
        {
            Text =
                "SweetGlaze CRM - Manager Dashboard";

            StartPosition =
                FormStartPosition.CenterScreen;

            MinimumSize =
                new Size(1100, 700);

            Size =
                new Size(1400, 850);

            BackColor =
                BackgroundColor;

            Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Regular);

            AutoScaleMode =
                AutoScaleMode.Dpi;

            CreateDashboard();

            ConfigureGrid();
        }

        private void CreateDashboard()
        {
            var rootPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = BackgroundColor
            };

            rootPanel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    235));

            rootPanel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    100));

            Controls.Add(rootPanel);

            var sidebar =
                CreateSidebar();

            rootPanel.Controls.Add(
                sidebar,
                0,
                0);

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(28)
            };

            rootPanel.Controls.Add(
                contentPanel,
                1,
                0);

            CreateMainContent();
        }

        private Panel CreateSidebar()
        {
            var sidebar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PrimaryColor,
                Padding = new Padding(18, 25, 18, 18)
            };

            var sidebarLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = PrimaryColor
            };

            sidebarLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    75));

            sidebarLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    35));

            sidebarLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            sidebarLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    55));

            sidebar.Controls.Add(sidebarLayout);

            var logoPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PrimaryColor
            };

            var lblLogo = new Label
            {
                Text = "SWEET GLAZE",
                Font = new Font(
                    "Segoe UI",
                    18F,
                    FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var lblLogoSub = new Label
            {
                Text = "CRM MANAGEMENT",
                Font = new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold),
                ForeColor = AccentColor,
                AutoSize = true,
                Location = new Point(2, 35)
            };

            logoPanel.Controls.Add(lblLogo);
            logoPanel.Controls.Add(lblLogoSub);

            sidebarLayout.Controls.Add(
                logoPanel,
                0,
                0);

            var lblNavigation = new Label
            {
                Text = "MAIN NAVIGATION",
                Font = new Font(
                    "Segoe UI",
                    8F,
                    FontStyle.Bold),
                ForeColor = Color.FromArgb(
                    170,
                    180,
                    195),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            sidebarLayout.Controls.Add(
                lblNavigation,
                0,
                1);

            var navigationPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection =
                    FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = PrimaryColor,
                Padding = new Padding(0, 8, 0, 0)
            };

            sidebarLayout.Controls.Add(
                navigationPanel,
                0,
                2);

            AddNavigationButton(
                navigationPanel,
                "▦   Dashboard",
                false,
                () =>
                {
                    ShowDashboardPage();
                });

            AddNavigationButton(
                navigationPanel,
                "♟   Customers",
                true,
                () =>
                {
                    ShowCustomerPage();
                });

            AddNavigationButton(
                navigationPanel,
                "▣   Products",
                false,
                () =>
                {
                    ShowModuleMessage(
                        "Product Management",
                        "Product Management will be connected here.");
                });

            AddNavigationButton(
                navigationPanel,
                "★   Loyalty",
                false,
                () =>
                {
                    ShowModuleMessage(
                        "Loyalty Management",
                        "Loyalty Management will be connected here.");
                });

            AddNavigationButton(
                navigationPanel,
                "◇   Promotions",
                false,
                () =>
                {
                    ShowModuleMessage(
                        "Promotions Management",
                        "Promotions Management will be connected here.");
                });

            AddNavigationButton(
                navigationPanel,
                "☷   Feedback",
                false,
                () =>
                {
                    ShowModuleMessage(
                        "Feedback Management",
                        "Feedback Management will be connected here.");
                });

            AddNavigationButton(
                navigationPanel,
                "▤   Reports",
                false,
                () =>
                {
                    ShowModuleMessage(
                        "Reports Management",
                        "Reports Management will be connected here.");
                });

            AddNavigationButton(
                navigationPanel,
                "▤   Sales Transactions",
                false,
                () =>
                {
                    ShowModuleMessage(
                        "Sales Transactions",
                        "Sales Transactions will be connected here.");
                });

            var accountPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PrimaryColor
            };

            var separator = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.FromArgb(
                    70,
                    80,
                    100)
            };

            accountPanel.Controls.Add(separator);

            var lblManager = new Label
            {
                Text = "●  Manager",
                Font = new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(0, 17)
            };

            accountPanel.Controls.Add(lblManager);

            sidebarLayout.Controls.Add(
                accountPanel,
                0,
                3);

            return sidebar;
        }

        private void AddNavigationButton(
            FlowLayoutPanel panel,
            string text,
            bool selected,
            Action action)
        {
            var button = new Button
            {
                Text = text,
                Width = 195,
                Height = 43,
                FlatStyle = FlatStyle.Flat,
                BackColor = selected
                    ? AccentColor
                    : PrimaryColor,
                ForeColor = selected
                    ? PrimaryColor
                    : Color.FromArgb(
                        215,
                        220,
                        230),
                Font = new Font(
                    "Segoe UI",
                    10F,
                    selected
                        ? FontStyle.Bold
                        : FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 3, 0, 3),
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;

            button.Click += (s, e) =>
            {
                action();
            };

            panel.Controls.Add(button);
        }

        private void CreateMainContent()
        {
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = BackgroundColor
            };

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    65));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    120));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    65));

            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100));

            contentPanel.Controls.Add(mainLayout);

            var header = CreateHeader();

            mainLayout.Controls.Add(
                header,
                0,
                0);

            var cards = CreateSummaryCards();

            mainLayout.Controls.Add(
                cards,
                0,
                1);

            var customerHeader = CreateCustomerHeader();

            mainLayout.Controls.Add(
                customerHeader,
                0,
                2);

            var customerContent = CreateCustomerContent();

            mainLayout.Controls.Add(
                customerContent,
                0,
                3);
        }

        private Panel CreateHeader()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor
            };

            lblPageTitle = new Label
            {
                Text = "Customer Management",
                Font = new Font(
                    "Segoe UI",
                    22F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            lblPageSubtitle = new Label
            {
                Text = "Manage and organize your customer records",
                Font = new Font(
                    "Segoe UI",
                    10F),
                ForeColor = MutedColor,
                AutoSize = true,
                Location = new Point(2, 38)
            };

            panel.Controls.Add(lblPageTitle);
            panel.Controls.Add(lblPageSubtitle);

            var profile = new Label
            {
                Text = "Manager  ●",
                Font = new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Right,
                Location = new Point(
                    panel.Width - 120,
                    12)
            };

            panel.Resize += (s, e) =>
            {
                profile.Location = new Point(
                    panel.ClientSize.Width -
                    profile.Width,
                    12);
            };

            panel.Controls.Add(profile);

            return panel;
        }

        private Panel CreateSummaryCards()
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = BackgroundColor,
                Padding = new Padding(0, 5, 0, 5)
            };

            panel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    33.33F));

            panel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    33.33F));

            panel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    33.33F));

            var card1 = CreateSummaryCard(
                "TOTAL CUSTOMERS",
                "0",
                "Customer records",
                out lblTotalCustomers);

            var card2 = CreateSummaryCard(
                "ACTIVE CUSTOMERS",
                "0",
                "Active records",
                out lblActiveCustomers);

            var card3 = CreateSummaryCard(
                "MANAGER ACCESS",
                "CRM",
                "Customer management",
                out _);

            panel.Controls.Add(card1, 0, 0);
            panel.Controls.Add(card2, 1, 0);
            panel.Controls.Add(card3, 2, 0);

            return panel;
        }

        private Panel CreateSummaryCard(
            string title,
            string value,
            string subtitle,
            out Label valueLabel)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 12, 0),
                Padding = new Padding(18)
            };

            var titleLabel = new Label
            {
                Text = title,
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold),
                ForeColor = MutedColor,
                AutoSize = true,
                Location = new Point(18, 15)
            };

            valueLabel = new Label
            {
                Text = value,
                Font = new Font(
                    "Segoe UI",
                    23F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(18, 35)
            };

            var subtitleLabel = new Label
            {
                Text = subtitle,
                Font = new Font(
                    "Segoe UI",
                    8.5F),
                ForeColor = MutedColor,
                AutoSize = true,
                Location = new Point(20, 78)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(subtitleLabel);

            return card;
        }

        private Panel CreateCustomerHeader()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor
            };

            var lblList = new Label
            {
                Text = "CUSTOMER LIST",
                Font = new Font(
                    "Segoe UI",
                    13F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(0, 15)
            };

            txtSearch = new TextBox
            {
                Name = "txtSearch",
                Width = 245,
                Height = 34,
                Font = new Font(
                    "Segoe UI",
                    10F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ForeColor = TextColor,
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Right,
                Location = new Point(
                    panel.Width - 450,
                    10)
            };

            txtSearch.TextChanged += (s, e) =>
            {
                ApplySearchFilter();
            };

            btnRefresh = CreateButton(
                "⟳  Refresh",
                "btnRefresh",
                Color.FromArgb(
                    75,
                    90,
                    115));

            btnRefresh.Width = 105;
            btnRefresh.Height = 35;
            btnRefresh.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnRefresh.Location = new Point(
                panel.Width - 190,
                10);

            btnRefresh.Click += btnRefresh_Click;

            var btnNew = CreateButton(
                "+  Add Customer",
                "btnNewCustomer",
                AccentColor);

            btnNew.ForeColor = PrimaryColor;
            btnNew.Width = 145;
            btnNew.Height = 35;
            btnNew.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            btnNew.Location = new Point(
                panel.Width - 40,
                10);

            btnNew.Click += (s, e) =>
            {
                ClearForm();
                txtFirstName.Focus();
            };

            panel.Resize += (s, e) =>
            {
                btnNew.Location = new Point(
                    panel.ClientSize.Width -
                    btnNew.Width,
                    10);

                btnRefresh.Location = new Point(
                    panel.ClientSize.Width -
                    btnNew.Width -
                    btnRefresh.Width -
                    20,
                    10);

                txtSearch.Location = new Point(
                    panel.ClientSize.Width -
                    btnNew.Width -
                    btnRefresh.Width -
                    txtSearch.Width -
                    35,
                    10);
            };

            panel.Controls.Add(lblList);
            panel.Controls.Add(txtSearch);
            panel.Controls.Add(btnRefresh);
            panel.Controls.Add(btnNew);

            return panel;
        }

        private Panel CreateCustomerContent()
        {
            var outerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor
            };

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 350,
                FixedPanel = FixedPanel.Panel1,
                BackColor = BackgroundColor,
                BorderStyle = BorderStyle.None
            };

            outerPanel.Controls.Add(split);

            var formPanel = CreateCustomerFormPanel();

            split.Panel1.Controls.Add(formPanel);

            var listPanel = CreateCustomerListPanel();

            split.Panel2.Controls.Add(listPanel);

            return outerPanel;
        }

        private Panel CreateCustomerFormPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18)
            };

            var title = new Label
            {
                Text = "Customer Details",
                Font = new Font(
                    "Segoe UI",
                    14F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 35
            };

            var subtitle = new Label
            {
                Text = "Add or update customer information",
                Font = new Font(
                    "Segoe UI",
                    9F),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 28
            };

            panel.Controls.Add(subtitle);
            panel.Controls.Add(title);

            var formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                BackColor = Color.White,
                Padding = new Padding(0, 10, 0, 0)
            };

            for (int i = 0; i < 8; i++)
            {
                formLayout.RowStyles.Add(
                    new RowStyle(
                        SizeType.Percent,
                        12.5F));
            }

            panel.Controls.Add(formLayout);

            txtFirstName = CreateTextBox(
                "txtFirstName");

            txtLastName = CreateTextBox(
                "txtLastName");

            txtEmail = CreateTextBox(
                "txtEmail");

            txtPhone = CreateTextBox(
                "txtPhone");

            txtCompany = CreateTextBox(
                "txtCompany");

            txtAddress = CreateTextBox(
                "txtAddress");

            txtNotes = CreateTextBox(
                "txtNotes",
                true);

            AddVerticalField(
                formLayout,
                "First Name",
                txtFirstName,
                0);

            AddVerticalField(
                formLayout,
                "Last Name",
                txtLastName,
                1);

            AddVerticalField(
                formLayout,
                "Email",
                txtEmail,
                2);

            AddVerticalField(
                formLayout,
                "Phone Number",
                txtPhone,
                3);

            AddVerticalField(
                formLayout,
                "Company Name",
                txtCompany,
                4);

            AddVerticalField(
                formLayout,
                "Address",
                txtAddress,
                5);

            AddVerticalField(
                formLayout,
                "Notes",
                txtNotes,
                6);

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection =
                    FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = Color.White
            };

            btnAdd = CreateButton(
                "＋ Add",
                "btnAdd",
                Color.FromArgb(
                    37,
                    99,
                    235));

            btnUpdate = CreateButton(
                "Update",
                "btnUpdate",
                Color.FromArgb(
                    16,
                    125,
                    85));

            btnDelete = CreateButton(
                "Delete",
                "btnDelete",
                Color.FromArgb(
                    220,
                    65,
                    65));

            btnClear = CreateButton(
                "Clear",
                "btnClear",
                Color.FromArgb(
                    100,
                    110,
                    125));

            btnAdd.Width = 85;
            btnUpdate.Width = 85;
            btnDelete.Width = 85;
            btnClear.Width = 85;

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            buttonsPanel.Controls.Add(btnAdd);
            buttonsPanel.Controls.Add(btnUpdate);
            buttonsPanel.Controls.Add(btnDelete);
            buttonsPanel.Controls.Add(btnClear);

            formLayout.RowStyles.Clear();

            formLayout.RowCount = 8;

            for (int i = 0; i < 7; i++)
            {
                formLayout.RowStyles.Add(
                    new RowStyle(
                        SizeType.Absolute,
                        65));
            }

            formLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    55));

            formLayout.Controls.Add(
                buttonsPanel,
                0,
                7);

            formLayout.SetColumnSpan(
                buttonsPanel,
                1);

            lblSelectedCustomer = new Label
            {
                Text = "No customer selected",
                Font = new Font(
                    "Segoe UI",
                    8.5F),
                ForeColor = MutedColor,
                Dock = DockStyle.Bottom,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };

            panel.Controls.Add(lblSelectedCustomer);

            return panel;
        }

        private Panel CreateCustomerListPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            dgvCustomers = new DataGridView
            {
                Name = "dgvCustomers",
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle =
                    DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(
                    225,
                    230,
                    238),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 42,
                RowTemplate = { Height = 38 },
                Font = new Font(
                    "Segoe UI",
                    9.5F)
            };

            dgvCustomers.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = PrimaryColor,
                    ForeColor = Color.White,
                    Font = new Font(
                        "Segoe UI",
                        9.5F,
                        FontStyle.Bold),
                    Alignment =
                        DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(8)
                };

            dgvCustomers.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(
                        55,
                        65,
                        80),
                    SelectionBackColor = Color.FromArgb(
                        219,
                        234,
                        254),
                    SelectionForeColor = Color.FromArgb(
                        30,
                        45,
                        65),
                    Padding = new Padding(8),
                    WrapMode =
                        DataGridViewTriState.False
                };

            AddGridColumn(
                "Id",
                "ID",
                55);

            AddGridColumn(
                "FirstName",
                "First Name",
                120);

            AddGridColumn(
                "LastName",
                "Last Name",
                120);

            AddGridColumn(
                "Email",
                "Email",
                190);

            AddGridColumn(
                "PhoneNumber",
                "Phone",
                120);

            AddGridColumn(
                "CompanyName",
                "Company",
                150);

            AddGridColumn(
                "Address",
                "Address",
                180);

            AddGridColumn(
                "IsActive",
                "Active",
                75);

            dgvCustomers.DataSource =
                _customers;

            dgvCustomers.SelectionChanged +=
                dgvCustomers_SelectionChanged;

            panel.Controls.Add(dgvCustomers);

            return panel;
        }

        private TextBox CreateTextBox(
            string name,
            bool multiline = false)
        {
            return new TextBox
            {
                Name = name,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 3, 0, 8),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font(
                    "Segoe UI",
                    10F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(
                    40,
                    50,
                    65),
                Multiline = multiline,
                ScrollBars = multiline
                    ? ScrollBars.Vertical
                    : ScrollBars.None
            };
        }

        private void AddVerticalField(
            TableLayoutPanel table,
            string labelText,
            Control control,
            int row)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            var label = new Label
            {
                Text = labelText,
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 22
            };

            panel.Controls.Add(control);
            panel.Controls.Add(label);

            control.Dock = DockStyle.Fill;

            table.Controls.Add(
                panel,
                0,
                row);
        }

        private Button CreateButton(
            string text,
            string name,
            Color color)
        {
            var button = new Button
            {
                Text = text,
                Name = name,
                Width = 135,
                Height = 38,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(4),
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;

            return button;
        }

        private void AddGridColumn(
            string property,
            string header,
            int width)
        {
            var column =
                new DataGridViewTextBoxColumn
                {
                    Name = property,
                    HeaderText = header,
                    DataPropertyName = property,
                    Width = width,
                    FillWeight = width,
                    SortMode =
                        DataGridViewColumnSortMode.Automatic
                };

            dgvCustomers.Columns.Add(column);
        }

        private void ConfigureGrid()
        {
            dgvCustomers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvCustomers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCustomers.MultiSelect = false;

            dgvCustomers.ReadOnly = true;

            dgvCustomers.AllowUserToAddRows = false;

            dgvCustomers.AllowUserToDeleteRows = false;
        }

        private async void CustomerForm_Load(
            object? sender,
            EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                SetLoading(true);

                var customers =
                    await _apiService.GetCustomersAsync();

                _allCustomers.Clear();

                _allCustomers.AddRange(customers);

                ApplySearchFilter();

                UpdateSummaryCards();

                dgvCustomers.ClearSelection();

                _selectedCustomerId = null;

                lblSelectedCustomer.Text =
                    "No customer selected";
            }
            catch (HttpRequestException)
            {
                ShowError(
                    "Unable to connect to the SweetGlazeCRM API.\n\n" +
                    "Make sure the API is running at:\n" +
                    "https://localhost:7070");
            }
            catch (TaskCanceledException)
            {
                ShowError(
                    "The request timed out.\n\n" +
                    "Please check if the API is running.");
            }
            catch (Exception ex)
            {
                ShowError(
                    "Unable to load customers.\n\n" +
                    ex.Message);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void ApplySearchFilter()
        {
            if (txtSearch == null)
            {
                return;
            }

            string search =
                txtSearch.Text.Trim()
                .ToLowerInvariant();

            _customers.Clear();

            foreach (var customer in _allCustomers)
            {
                bool matches =
                    string.IsNullOrWhiteSpace(search) ||
                    customer.Id.ToString()
                        .Contains(search) ||
                    (customer.FirstName ?? "")
                        .ToLowerInvariant()
                        .Contains(search) ||
                    (customer.LastName ?? "")
                        .ToLowerInvariant()
                        .Contains(search) ||
                    (customer.Email ?? "")
                        .ToLowerInvariant()
                        .Contains(search) ||
                    (customer.PhoneNumber ?? "")
                        .ToLowerInvariant()
                        .Contains(search) ||
                    (customer.CompanyName ?? "")
                        .ToLowerInvariant()
                        .Contains(search);

                if (matches)
                {
                    _customers.Add(customer);
                }
            }
        }

        private void UpdateSummaryCards()
        {
            if (lblTotalCustomers == null ||
                lblActiveCustomers == null)
            {
                return;
            }

            lblTotalCustomers.Text =
                _allCustomers.Count.ToString();

            lblActiveCustomers.Text =
                _allCustomers.Count(
                    c => c.IsActive)
                .ToString();
        }

        private void SetLoading(bool loading)
        {
            if (btnAdd != null)
            {
                btnAdd.Enabled = !loading;
            }

            if (btnUpdate != null)
            {
                btnUpdate.Enabled = !loading;
            }

            if (btnDelete != null)
            {
                btnDelete.Enabled = !loading;
            }

            if (btnRefresh != null)
            {
                btnRefresh.Enabled = !loading;
            }
        }

        private void dgvCustomers_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                return;
            }

            if (dgvCustomers.SelectedRows[0].DataBoundItem
                is Customer customer)
            {
                _selectedCustomerId =
                    customer.Id;

                txtFirstName.Text =
                    customer.FirstName;

                txtLastName.Text =
                    customer.LastName;

                txtEmail.Text =
                    customer.Email;

                txtPhone.Text =
                    customer.PhoneNumber ?? "";

                txtCompany.Text =
                    customer.CompanyName ?? "";

                txtAddress.Text =
                    customer.Address ?? "";

                txtNotes.Text =
                    customer.Notes ?? "";

                lblSelectedCustomer.Text =
                    $"Selected Customer #{customer.Id}";
            }
        }

        private Customer GetCustomerFromForm()
        {
            return new Customer
            {
                Id =
                    _selectedCustomerId ?? 0,

                FirstName =
                    txtFirstName.Text.Trim(),

                LastName =
                    txtLastName.Text.Trim(),

                Email =
                    txtEmail.Text.Trim(),

                PhoneNumber =
                    string.IsNullOrWhiteSpace(
                        txtPhone.Text)
                    ? null
                    : txtPhone.Text.Trim(),

                CompanyName =
                    string.IsNullOrWhiteSpace(
                        txtCompany.Text)
                    ? null
                    : txtCompany.Text.Trim(),

                Address =
                    string.IsNullOrWhiteSpace(
                        txtAddress.Text)
                    ? null
                    : txtAddress.Text.Trim(),

                Notes =
                    string.IsNullOrWhiteSpace(
                        txtNotes.Text)
                    ? null
                    : txtNotes.Text.Trim(),

                IsActive = true
            };
        }

        private bool ValidateCustomer()
        {
            if (string.IsNullOrWhiteSpace(
                txtFirstName.Text))
            {
                ShowWarning(
                    "Please enter the customer's first name.");

                txtFirstName.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtLastName.Text))
            {
                ShowWarning(
                    "Please enter the customer's last name.");

                txtLastName.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtEmail.Text))
            {
                ShowWarning(
                    "Please enter the customer's email.");

                txtEmail.Focus();

                return false;
            }

            try
            {
                var mail =
                    new MailAddress(
                        txtEmail.Text.Trim());

                if (mail.Address !=
                    txtEmail.Text.Trim())
                {
                    throw new FormatException();
                }
            }
            catch
            {
                ShowWarning(
                    "Please enter a valid email address.");

                txtEmail.Focus();

                return false;
            }

            return true;
        }

        private async void btnAdd_Click(
            object? sender,
            EventArgs e)
        {
            if (!ValidateCustomer())
            {
                return;
            }

            try
            {
                SetLoading(true);

                var customer =
                    GetCustomerFromForm();

                await _apiService.AddCustomerAsync(
                    customer);

                MessageBox.Show(
                    "Customer added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();

                await LoadCustomersAsync();
            }
            catch (HttpRequestException ex)
            {
                ShowError(
                    "Unable to add customer.\n\n" +
                    ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(
                    "An error occurred while adding the customer.\n\n" +
                    ex.Message);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void btnUpdate_Click(
            object? sender,
            EventArgs e)
        {
            if (_selectedCustomerId == null)
            {
                ShowWarning(
                    "Please select a customer to update.");

                return;
            }

            if (!ValidateCustomer())
            {
                return;
            }

            try
            {
                SetLoading(true);

                var customer =
                    GetCustomerFromForm();

                await _apiService.UpdateCustomerAsync(
                    _selectedCustomerId.Value,
                    customer);

                MessageBox.Show(
                    "Customer updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();

                await LoadCustomersAsync();
            }
            catch (HttpRequestException ex)
            {
                ShowError(
                    "Unable to update customer.\n\n" +
                    ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(
                    "An error occurred while updating the customer.\n\n" +
                    ex.Message);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void btnDelete_Click(
            object? sender,
            EventArgs e)
        {
            if (_selectedCustomerId == null)
            {
                ShowWarning(
                    "Please select a customer to delete.");

                return;
            }

            var confirm =
                MessageBox.Show(
                    "Are you sure you want to delete this customer?\n\n" +
                    "This action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                SetLoading(true);

                await _apiService.DeleteCustomerAsync(
                    _selectedCustomerId.Value);

                MessageBox.Show(
                    "Customer deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();

                await LoadCustomersAsync();
            }
            catch (HttpRequestException ex)
            {
                ShowError(
                    "Unable to delete customer.\n\n" +
                    ex.Message);
            }
            catch (Exception ex)
            {
                ShowError(
                    "An error occurred while deleting the customer.\n\n" +
                    ex.Message);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void btnClear_Click(
            object? sender,
            EventArgs e)
        {
            ClearForm();
        }

        private async void btnRefresh_Click(
            object? sender,
            EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private void ClearForm()
        {
            txtFirstName.Clear();

            txtLastName.Clear();

            txtEmail.Clear();

            txtPhone.Clear();

            txtCompany.Clear();

            txtAddress.Clear();

            txtNotes.Clear();

            _selectedCustomerId = null;

            dgvCustomers.ClearSelection();

            lblSelectedCustomer.Text =
                "No customer selected";

            txtFirstName.Focus();
        }

        private void ShowDashboardPage()
        {
            lblPageTitle.Text =
                "Manager Dashboard";

            lblPageSubtitle.Text =
                "Welcome to your Sweet Glaze CRM";

            ShowModuleMessage(
                "Manager Dashboard",
                "Your Sweet Glaze Manager Dashboard.\n\n" +
                "Use the sidebar to access Customer Management " +
                "and other modules.");
        }

        private void ShowCustomerPage()
        {
            lblPageTitle.Text =
                "Customer Management";

            lblPageSubtitle.Text =
                "Manage and organize your customer records";

            ShowModuleMessage(
                "Customer Management",
                "Customer Management is available in this dashboard.\n\n" +
                "Use the customer form and table to manage records.");
        }

        private void ShowModuleMessage(
            string title,
            string message)
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}