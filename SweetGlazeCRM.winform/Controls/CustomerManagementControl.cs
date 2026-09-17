using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SweetGlazeCRM.winform.Forms;
using SweetGlazeCRM.winform.Models;
using SweetGlazeCRM.winform.Services;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Controls
{
    /// <summary>
    /// The only functional CRUD page right now. Loads the full customer list once,
    /// then filters it locally for search (no request-per-keystroke).
    /// </summary>
    public class CustomerManagementControl : UserControl
    {
        private readonly MainForm _owner;
        private readonly CustomerApiService _api = new CustomerApiService();

        private readonly TextBox _txtSearch = new TextBox();
        private readonly Button _btnAdd = new Button();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Label _lblStatus = new Label();
        private readonly Label _lblEmpty = new Label();
        private readonly Button _btnEmptyAdd = new Button();

        private List<Customer> _allCustomers = new();
        private const string SearchPlaceholder = "Search customers...";

        public CustomerManagementControl(MainForm owner)
        {
            _owner = owner;
            BuildUi();
            _ = LoadCustomersAsync();
        }

        private void BuildUi()
        {
            Dock = DockStyle.Fill;
            BackColor = Theme.PageBackground;

            var lblTitle = new Label
            {
                Text = "Customers",
                Font = Theme.FontHeading,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var lblSubtitle = new Label
            {
                Text = "Manage your customer records",
                Font = Theme.FontSubheading,
                ForeColor = Theme.TextSecondary,
                AutoSize = true,
                Location = new Point(0, 30)
            };

            _txtSearch.Location = new Point(0, 64);
            _txtSearch.Width = 320;
            _txtSearch.Font = Theme.FontBody;
            SetupPlaceholder(_txtSearch, SearchPlaceholder);
            _txtSearch.TextChanged += (s, e) => ApplyFilter();

            _btnAdd.Text = "+  Add Customer";
            _btnAdd.Font = Theme.FontButton;
            _btnAdd.Size = new Size(150, 32);
            _btnAdd.FlatStyle = FlatStyle.Flat;
            _btnAdd.FlatAppearance.BorderSize = 0;
            _btnAdd.BackColor = Theme.Accent;
            _btnAdd.ForeColor = Color.White;
            _btnAdd.Cursor = Cursors.Hand;
            _btnAdd.Click += (s, e) => OpenAddDialog();

            _lblStatus.AutoSize = true;
            _lblStatus.Font = Theme.FontBody;
            _lblStatus.ForeColor = Theme.TextSecondary;
            _lblStatus.Location = new Point(0, 100);
            _lblStatus.Visible = false;

            _lblEmpty.AutoSize = false;
            _lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            _lblEmpty.Font = Theme.FontBody;
            _lblEmpty.ForeColor = Theme.TextSecondary;
            _lblEmpty.Visible = false;

            _btnEmptyAdd.Text = "+ Add Customer";
            _btnEmptyAdd.Size = new Size(150, 32);
            _btnEmptyAdd.FlatStyle = FlatStyle.Flat;
            _btnEmptyAdd.FlatAppearance.BorderSize = 0;
            _btnEmptyAdd.BackColor = Theme.Accent;
            _btnEmptyAdd.ForeColor = Color.White;
            _btnEmptyAdd.Visible = false;
            _btnEmptyAdd.Click += (s, e) => OpenAddDialog();

            BuildGrid();

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(_txtSearch);
            Controls.Add(_btnAdd);
            Controls.Add(_lblStatus);
            Controls.Add(_grid);
            Controls.Add(_lblEmpty);
            Controls.Add(_btnEmptyAdd);

            Resize += (s, e) => LayoutControls();
            LayoutControls();
        }

        private void LayoutControls()
        {
            _btnAdd.Top = 64;
            _btnAdd.Left = Math.Max(_txtSearch.Right + 20, Width - _btnAdd.Width);

            int gridTop = 108;
            _grid.Location = new Point(0, gridTop);
            _grid.Size = new Size(Math.Max(200, Width), Math.Max(100, Height - gridTop));

            _lblEmpty.Bounds = new Rectangle(0, gridTop, Math.Max(200, Width), 80);
            _btnEmptyAdd.Location = new Point((Width - _btnEmptyAdd.Width) / 2, gridTop + 90);
        }

        private void BuildGrid()
        {
            _grid.AutoGenerateColumns = false;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AllowUserToResizeRows = false;
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.MultiSelect = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.BackgroundColor = Theme.CardBackground;
            _grid.BorderStyle = BorderStyle.None;
            _grid.GridColor = Theme.Border;
            _grid.Font = Theme.FontBody;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Theme.PageBackground;
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Theme.TextPrimary;
            _grid.ColumnHeadersDefaultCellStyle.Font = Theme.FontBold;
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _grid.ColumnHeadersHeight = 36;
            _grid.RowTemplate.Height = 34;
            _grid.EnableHeadersVisualStyles = false;

            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "FirstName", HeaderText = "First Name", Width = 130 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastName", HeaderText = "Last Name", Width = 130 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", Width = 200 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Phone", Width = 120 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 90 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Created", HeaderText = "Created", Width = 110 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "View", HeaderText = "", Text = "View", UseColumnTextForButtonValue = true, Width = 70 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "Edit", HeaderText = "", Text = "Edit", UseColumnTextForButtonValue = true, Width = 70 });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "Delete", HeaderText = "", Text = "Delete", UseColumnTextForButtonValue = true, Width = 80 });

            _grid.CellClick += Grid_CellClick;
            _grid.CellDoubleClick += Grid_CellDoubleClick;
        }

        private void SetupPlaceholder(TextBox box, string placeholder)
        {
            box.Text = placeholder;
            box.ForeColor = Theme.TextSecondary;
            box.Enter += (s, e) =>
            {
                if (box.Text == placeholder)
                {
                    box.Text = "";
                    box.ForeColor = Theme.TextPrimary;
                }
            };
            box.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = placeholder;
                    box.ForeColor = Theme.TextSecondary;
                }
            };
        }

        private string SearchText => _txtSearch.Text == SearchPlaceholder ? "" : _txtSearch.Text.Trim();

        private async Task LoadCustomersAsync()
        {
            SetLoading(true, "Loading customers...");
            try
            {
                _allCustomers = await _api.GetCustomersAsync();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                ShowLoadError(ex);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void ApplyFilter()
        {
            var term = SearchText;
            IEnumerable<Customer> filtered = _allCustomers;

            if (!string.IsNullOrEmpty(term))
            {
                filtered = _allCustomers.Where(c =>
                    (c.FirstName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (c.LastName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (c.Email ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (c.PhoneNumber ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            var list = filtered.ToList();
            PopulateGrid(list);

            if (_allCustomers.Count == 0)
            {
                ShowEmptyState("No customers found.", "There are currently no customer records.", true);
            }
            else if (list.Count == 0)
            {
                ShowEmptyState("No customers match your search.", "", false);
            }
            else
            {
                HideEmptyState();
            }
        }

        private void PopulateGrid(List<Customer> customers)
        {
            _grid.Rows.Clear();
            foreach (var c in customers)
            {
                int rowIndex = _grid.Rows.Add(
                    c.Id, c.FirstName, c.LastName, c.Email, c.PhoneNumber,
                    c.StatusText, c.CreatedAt.ToString("MMM d, yyyy"));
                _grid.Rows[rowIndex].Tag = c;
            }
        }

        private void ShowEmptyState(string title, string subtitle, bool showAddButton)
        {
            _grid.Visible = false;
            _lblEmpty.Text = string.IsNullOrEmpty(subtitle) ? title : $"{title}\n{subtitle}";
            _lblEmpty.Visible = true;
            _btnEmptyAdd.Visible = showAddButton;
        }

        private void HideEmptyState()
        {
            _grid.Visible = true;
            _lblEmpty.Visible = false;
            _btnEmptyAdd.Visible = false;
        }

        private void SetLoading(bool loading, string message = "")
        {
            _lblStatus.Visible = loading;
            _lblStatus.Text = message;
            _btnAdd.Enabled = !loading;
            _grid.Enabled = !loading;
            _txtSearch.Enabled = !loading;
        }

        private void ShowLoadError(Exception ex)
        {
            string message = ex is ApiException apiEx
                ? apiEx.Message
                : "Unable to connect to SweetGlaze CRM.\nPlease make sure the API is running and try again.";

            ShowEmptyState("Unable to load customers.", message, false);
        }

        private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = _grid.Rows[e.RowIndex];
            if (row.Tag is not Customer customer) return;

            var columnName = _grid.Columns[e.ColumnIndex].Name;

            if (columnName == "View") OpenViewDialog(customer);
            else if (columnName == "Edit") OpenEditDialog(customer);
            else if (columnName == "Delete") ConfirmAndDelete(customer);
        }

        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (_grid.Rows[e.RowIndex].Tag is Customer customer) OpenViewDialog(customer);
        }

        private void OpenAddDialog()
        {
            using var dialog = new CustomerDialog(_api, CustomerDialog.Mode.Add, null);
            if (_owner.ShowModalDialog(dialog) == DialogResult.OK)
            {
                MessageBox.Show(this, "Customer added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ = LoadCustomersAsync();
            }
        }

        private void OpenEditDialog(Customer customer)
        {
            using var dialog = new CustomerDialog(_api, CustomerDialog.Mode.Edit, customer);
            if (_owner.ShowModalDialog(dialog) == DialogResult.OK)
            {
                MessageBox.Show(this, "Customer updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ = LoadCustomersAsync();
            }
        }

        private void OpenViewDialog(Customer customer)
        {
            using var dialog = new CustomerDetailsDialog(customer);
            _owner.ShowModalDialog(dialog);
        }

        private async void ConfirmAndDelete(Customer customer)
        {
            using var confirm = new ConfirmDialog(
                "Delete Customer",
                "Are you sure you want to delete this customer?",
                "This action cannot be undone.",
                "Delete");

            if (_owner.ShowModalDialog(confirm) != DialogResult.OK) return;

            SetLoading(true, "Deleting customer...");
            try
            {
                await _api.DeleteCustomerAsync(customer.Id);
                MessageBox.Show(this, "Customer deleted successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadCustomersAsync();
            }
            catch (Exception ex)
            {
                string message = ex is ApiException apiEx
                    ? apiEx.Message
                    : "Unable to delete customer. Please try again.";
                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetLoading(false);
            }
        }
    }
}
