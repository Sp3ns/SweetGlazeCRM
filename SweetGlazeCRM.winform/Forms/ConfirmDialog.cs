using System.Drawing;
using System.Windows.Forms;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Forms
{
    /// <summary>
    /// Generic confirmation dialog. Returns DialogResult.OK when the destructive
    /// action button is clicked, DialogResult.Cancel otherwise.
    /// </summary>
    public class ConfirmDialog : Form
    {
        public ConfirmDialog(string title, string message, string subtext, string confirmText)
        {
            BuildUi(title, message, subtext, confirmText);
        }

        private void BuildUi(string title, string message, string subtext, string confirmText)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(360, 190);
            BackColor = Theme.CardBackground;
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(24, 20)
            };

            var lblMessage = new Label
            {
                Text = message,
                Font = Theme.FontBody,
                ForeColor = Theme.TextPrimary,
                AutoSize = false,
                Size = new Size(312, 20),
                Location = new Point(24, 56)
            };

            var lblSubtext = new Label
            {
                Text = subtext,
                Font = Theme.FontBody,
                ForeColor = Theme.TextSecondary,
                AutoSize = false,
                Size = new Size(312, 20),
                Location = new Point(24, 78)
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(110, 34),
                Location = new Point(24, ClientSize.Height - 56),
                FlatStyle = FlatStyle.Flat,
                BackColor = Theme.CardBackground,
                ForeColor = Theme.TextPrimary,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderColor = Theme.Border;

            var btnConfirm = new Button
            {
                Text = confirmText,
                Size = new Size(140, 34),
                Location = new Point(ClientSize.Width - 164, ClientSize.Height - 56),
                FlatStyle = FlatStyle.Flat,
                BackColor = Theme.Danger,
                ForeColor = Color.White,
                DialogResult = DialogResult.OK
            };
            btnConfirm.FlatAppearance.BorderSize = 0;

            Controls.Add(lblTitle);
            Controls.Add(lblMessage);
            Controls.Add(lblSubtext);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);

            AcceptButton = btnConfirm;
            CancelButton = btnCancel;
        }
    }
}
