using System.Drawing;
using System.Windows.Forms;
using SweetGlazeCRM.winform.Models;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Forms
{
    /// <summary>
    /// Read-only "customer profile" view, opened via the View action or a
    /// double-click on a grid row. Never sends any API request.
    /// </summary>
    public class CustomerDetailsDialog : Form
    {
        public CustomerDetailsDialog(Customer customer)
        {
            BuildUi(customer);
        }

        private void BuildUi(Customer c)
        {
            Text = "Customer Details";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(400, 420);
            BackColor = Theme.CardBackground;
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            var lblTitle = new Label
            {
                Text = c.FullName,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(24, 20)
            };
            Controls.Add(lblTitle);

            var statusColor = c.IsActive ? Theme.Success : Theme.TextSecondary;
            var lblStatus = new Label
            {
                Text = c.StatusText,
                Font = Theme.FontBold,
                ForeColor = statusColor,
                AutoSize = true,
                Location = new Point(24, 50)
            };
            Controls.Add(lblStatus);

            int y = 90;
            y = AddRow("Email", c.Email, y);
            y = AddRow("Phone", string.IsNullOrWhiteSpace(c.PhoneNumber) ? "-" : c.PhoneNumber!, y);
            y = AddRow("Address", string.IsNullOrWhiteSpace(c.Address) ? "-" : c.Address!, y);
            y = AddRow("Notes", string.IsNullOrWhiteSpace(c.Notes) ? "-" : c.Notes!, y, true);
            y = AddRow("Created", c.CreatedAt.ToString("MMM d, yyyy h:mm tt"), y);
            y = AddRow("Updated", c.UpdatedAt.HasValue ? c.UpdatedAt.Value.ToString("MMM d, yyyy h:mm tt") : "-", y);

            var btnClose = new Button
            {
                Text = "Close",
                Size = new Size(110, 34),
                Location = new Point(ClientSize.Width - 134, ClientSize.Height - 56),
                FlatStyle = FlatStyle.Flat,
                BackColor = Theme.Accent,
                ForeColor = Color.White,
                DialogResult = DialogResult.OK
            };
            btnClose.FlatAppearance.BorderSize = 0;
            Controls.Add(btnClose);
            AcceptButton = btnClose;
            CancelButton = btnClose;
        }

        private int AddRow(string label, string value, int y, bool multiline = false)
        {
            var lbl = new Label
            {
                Text = label,
                Font = Theme.FontBold,
                ForeColor = Theme.TextSecondary,
                AutoSize = true,
                Location = new Point(24, y)
            };
            Controls.Add(lbl);

            var val = new Label
            {
                Text = value,
                Font = Theme.FontBody,
                ForeColor = Theme.TextPrimary,
                AutoSize = false,
                Size = new Size(240, multiline ? 40 : 20),
                Location = new Point(140, y)
            };
            Controls.Add(val);

            return y + (multiline ? 50 : 30);
        }
    }
}
