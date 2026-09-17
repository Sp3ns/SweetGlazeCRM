using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SweetGlazeCRM.winform.Models;
using SweetGlazeCRM.winform.Services;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Forms
{
    /// <summary>
    /// Modal dialog used for BOTH adding and editing a customer.
    /// Closes with DialogResult.OK only after the API call succeeds.
    /// </summary>
    public class CustomerDialog : Form
    {
        public enum Mode { Add, Edit }

        private readonly CustomerApiService _api;
        private readonly Mode _mode;
        private readonly Customer? _existing;

        private readonly TextBox _txtFirstName = new TextBox();
        private readonly TextBox _txtLastName = new TextBox();
        private readonly TextBox _txtEmail = new TextBox();
        private readonly TextBox _txtPhone = new TextBox();
        private readonly TextBox _txtAddress = new TextBox();
        private readonly TextBox _txtNotes = new TextBox();

        private readonly Label _errFirstName = new Label();
        private readonly Label _errLastName = new Label();
        private readonly Label _errEmail = new Label();
        private readonly Label _lblFormError = new Label();

        private readonly Button _btnSave = new Button();
        private readonly Button _btnCancel = new Button();

        public CustomerDialog(CustomerApiService api, Mode mode, Customer? existing)
        {
            _api = api;
            _mode = mode;
            _existing = existing;
            BuildUi();
            if (mode == Mode.Edit && existing != null) LoadExisting(existing);
        }

        private void BuildUi()
        {
            Text = _mode == Mode.Add ? "Add Customer" : "Edit Customer";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(420, 560);
            BackColor = Theme.CardBackground;
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            var lblTitle = new Label
            {
                Text = _mode == Mode.Add ? "Add Customer" : "Edit Customer",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(24, 20)
            };

            var lblSubtitle = new Label
            {
                Text = _mode == Mode.Add
                    ? "Enter the new customer's information"
                    : "Update this customer's information",
                Font = Theme.FontSubheading,
                ForeColor = Theme.TextSecondary,
                AutoSize = true,
                Location = new Point(24, 48)
            };

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);

            int y = 84;
            y = AddField("First Name *", _txtFirstName, y, _errFirstName);
            y = AddField("Last Name *", _txtLastName, y, _errLastName);
            y = AddField("Email *", _txtEmail, y, _errEmail);
            y = AddField("Phone Number", _txtPhone, y, null);
            y = AddField("Address", _txtAddress, y, null);

            var lblNotes = new Label
            {
                Text = "Notes",
                Font = Theme.FontBold,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(24, y)
            };
            Controls.Add(lblNotes);
            y += 20;

            _txtNotes.Multiline = true;
            _txtNotes.Height = 70;
            _txtNotes.Width = 372;
            _txtNotes.Location = new Point(24, y);
            _txtNotes.Font = Theme.FontBody;
            Controls.Add(_txtNotes);
            y += 82;

            _lblFormError.AutoSize = false;
            _lblFormError.ForeColor = Theme.Danger;
            _lblFormError.Font = Theme.FontBody;
            _lblFormError.Location = new Point(24, y);
            _lblFormError.Size = new Size(372, 34);
            _lblFormError.Visible = false;
            Controls.Add(_lblFormError);

            _btnCancel.Text = "Cancel";
            _btnCancel.Size = new Size(110, 34);
            _btnCancel.Location = new Point(24, ClientSize.Height - 56);
            _btnCancel.FlatStyle = FlatStyle.Flat;
            _btnCancel.FlatAppearance.BorderColor = Theme.Border;
            _btnCancel.BackColor = Theme.CardBackground;
            _btnCancel.ForeColor = Theme.TextPrimary;
            _btnCancel.DialogResult = DialogResult.Cancel;

            _btnSave.Text = _mode == Mode.Add ? "Add Customer" : "Save Changes";
            _btnSave.Size = new Size(180, 34);
            _btnSave.Location = new Point(ClientSize.Width - 204, ClientSize.Height - 56);
            _btnSave.FlatStyle = FlatStyle.Flat;
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.BackColor = Theme.Accent;
            _btnSave.ForeColor = Color.White;
            _btnSave.Font = Theme.FontButton;
            _btnSave.Click += async (s, e) => await SaveAsync();

            Controls.Add(_btnCancel);
            Controls.Add(_btnSave);

            AcceptButton = _btnSave;
            CancelButton = _btnCancel;
        }

        private int AddField(string labelText, TextBox box, int y, Label? errorLabel)
        {
            var lbl = new Label
            {
                Text = labelText,
                Font = Theme.FontBold,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(24, y)
            };
            Controls.Add(lbl);
            y += 20;

            box.Location = new Point(24, y);
            box.Width = 372;
            box.Font = Theme.FontBody;
            Controls.Add(box);
            y += 28;

            if (errorLabel != null)
            {
                errorLabel.AutoSize = true;
                errorLabel.ForeColor = Theme.Danger;
                errorLabel.Font = new Font("Segoe UI", 8f);
                errorLabel.Location = new Point(24, y);
                errorLabel.Visible = false;
                Controls.Add(errorLabel);
                y += 16;
            }
            else
            {
                y += 4;
            }

            return y;
        }

        private void LoadExisting(Customer c)
        {
            _txtFirstName.Text = c.FirstName;
            _txtLastName.Text = c.LastName;
            _txtEmail.Text = c.Email;
            _txtPhone.Text = c.PhoneNumber;
            _txtAddress.Text = c.Address;
            _txtNotes.Text = c.Notes;
        }

        private bool ValidateForm()
        {
            bool valid = true;
            _errFirstName.Visible = false;
            _errLastName.Visible = false;
            _errEmail.Visible = false;
            _lblFormError.Visible = false;

            if (string.IsNullOrWhiteSpace(_txtFirstName.Text))
            {
                _errFirstName.Text = "First name is required.";
                _errFirstName.Visible = true;
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(_txtLastName.Text))
            {
                _errLastName.Text = "Last name is required.";
                _errLastName.Visible = true;
                valid = false;
            }

            var email = _txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                _errEmail.Text = "Email is required.";
                _errEmail.Visible = true;
                valid = false;
            }
            else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _errEmail.Text = "Enter a valid email address.";
                _errEmail.Visible = true;
                valid = false;
            }

            return valid;
        }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            if (!ValidateForm()) return;

            var request = new CustomerRequest
            {
                FirstName = _txtFirstName.Text.Trim(),
                LastName = _txtLastName.Text.Trim(),
                Email = _txtEmail.Text.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(_txtPhone.Text) ? null : _txtPhone.Text.Trim(),
                Address = string.IsNullOrWhiteSpace(_txtAddress.Text) ? null : _txtAddress.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(_txtNotes.Text) ? null : _txtNotes.Text.Trim(),
                IsActive = _existing?.IsActive ?? true
            };

            SetBusy(true);
            try
            {
                if (_mode == Mode.Add)
                    await _api.AddCustomerAsync(request);
                else
                    await _api.UpdateCustomerAsync(_existing!.Id, request);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                string message = ex is ApiException apiEx
                    ? apiEx.Message
                    : _mode == Mode.Add
                        ? "Unable to add customer. Please try again."
                        : "Unable to update customer. Please try again.";

                _lblFormError.Text = message;
                _lblFormError.Visible = true;
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            _btnSave.Enabled = !busy;
            _btnCancel.Enabled = !busy;
            _btnSave.Text = busy
                ? (_mode == Mode.Add ? "Adding customer..." : "Saving changes...")
                : (_mode == Mode.Add ? "Add Customer" : "Save Changes");
        }
    }
}
