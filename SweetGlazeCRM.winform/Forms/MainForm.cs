using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SweetGlazeCRM.winform.Controls;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Forms
{
    /// <summary>
    /// The application shell: fixed left sidebar for navigation, a header bar,
    /// and a content area that swaps between Dashboard / Customers / placeholder pages.
    /// </summary>
    public class MainForm : Form
    {
        private readonly Panel _sidebar = new Panel();
        private readonly Panel _header = new Panel();
        private readonly Panel _contentPanel = new Panel();

        private readonly Dictionary<string, Button> _navButtons = new();
        private readonly Dictionary<string, Control> _pages = new();
        private string _activeNav = string.Empty;

        private static readonly string[] NavItems =
        {
            "Dashboard", "Customers", "Companies", "Contacts",
            "Products", "Sales", "Activities", "Reports", "Settings"
        };

        public MainForm()
        {
            BuildLayout();
            NavigateTo("Dashboard");
        }

        private void BuildLayout()
        {
            Text = "SweetGlaze CRM";
            MinimumSize = new Size(1150, 700);
            Size = new Size(1280, 800);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.PageBackground;
            Font = Theme.FontBody;

            // ---- Header ----
            _header.Dock = DockStyle.Top;
            _header.Height = 60;
            _header.Width = ClientSize.Width;
            _header.BackColor = Theme.CardBackground;

            var lblBrand = new Label
            {
                Text = "SweetGlaze CRM",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(24, 17)
            };

            var lblUser = new Label
            {
                Text = "Administrator",
                Font = Theme.FontBody,
                ForeColor = Theme.TextSecondary,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(_header.Width - 140, 20)
            };

            _header.Controls.Add(lblBrand);
            _header.Controls.Add(lblUser);

            // ---- Sidebar ----
            _sidebar.Dock = DockStyle.Left;
            _sidebar.Width = 220;
            _sidebar.BackColor = Theme.SidebarBackground;

            int y = 16;
            foreach (var item in NavItems)
            {
                var btn = new Button
                {
                    Text = "   " + item,
                    Tag = item,
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Height = 44,
                    Width = _sidebar.Width,
                    Top = y,
                    Left = 0,
                    Font = Theme.FontNav,
                    ForeColor = Theme.SidebarText,
                    BackColor = Theme.SidebarBackground,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 49, 64);
                btn.Click += (s, e) => NavigateTo((string)((Button)s!).Tag!);
                _sidebar.Controls.Add(btn);
                _navButtons[item] = btn;
                y += 46;
            }

            // ---- Content area ----
            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.BackColor = Theme.PageBackground;
            _contentPanel.Padding = new Padding(24);

            Controls.Add(_contentPanel);
            Controls.Add(_sidebar);
            Controls.Add(_header);
        }

        private void NavigateTo(string key)
        {
            if (_activeNav == key) return;

            if (!_pages.TryGetValue(key, out var page))
            {
                page = key switch
                {
                    "Dashboard" => new DashboardControl(),
                    "Customers" => new CustomerManagementControl(this),
                    _ => new PlaceholderControl(key)
                };
                page.Dock = DockStyle.Fill;
                _pages[key] = page;
            }

            _contentPanel.Controls.Clear();
            _contentPanel.Controls.Add(page);

            if (_navButtons.TryGetValue(_activeNav, out var previousBtn))
            {
                previousBtn.BackColor = Theme.SidebarBackground;
                previousBtn.ForeColor = Theme.SidebarText;
            }

            if (_navButtons.TryGetValue(key, out var activeBtn))
            {
                activeBtn.BackColor = Theme.SidebarActiveBackground;
                activeBtn.ForeColor = Theme.SidebarActiveText;
            }

            _activeNav = key;
        }

        /// <summary>
        /// Shows a Form as a modal dialog with a dimmed overlay behind it, so the
        /// page underneath reads as visually subdued while the dialog is open.
        /// The dialog is already non-interactive-behind by virtue of ShowDialog;
        /// this just adds the visual dimming on top of that.
        /// </summary>
        public DialogResult ShowModalDialog(Form dialog)
        {
            using var overlay = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                BackColor = Color.Black,
                Opacity = 0.35,
                Bounds = RectangleToScreen(ClientRectangle)
            };

            overlay.Show(this);
            try
            {
                return dialog.ShowDialog(this);
            }
            finally
            {
                overlay.Close();
            }
        }
    }
}
