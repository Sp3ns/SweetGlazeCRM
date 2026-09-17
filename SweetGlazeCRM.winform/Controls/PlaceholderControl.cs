using System.Drawing;
using System.Windows.Forms;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Controls
{
    /// <summary>
    /// Shown for every nav item that has no real backend behind it yet.
    /// Never displays fake data or fake CRUD controls.
    /// </summary>
    public class PlaceholderControl : UserControl
    {
        public PlaceholderControl(string moduleName)
        {
            BuildUi(moduleName);
        }

        private void BuildUi(string moduleName)
        {
            Dock = DockStyle.Fill;
            BackColor = Theme.PageBackground;

            var lblTitle = new Label
            {
                Text = moduleName,
                Font = Theme.FontHeading,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 60, 0, 0)
            };

            var lblMessage = new Label
            {
                Text = "This module is currently under development.\nComing soon.",
                Font = Theme.FontSubheading,
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            panel.Controls.Add(lblMessage);

            Controls.Add(panel);
            Controls.Add(lblTitle);
        }
    }
}
