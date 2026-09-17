using System.Drawing;
using System.Windows.Forms;
using SweetGlazeCRM.winform.UI;

namespace SweetGlazeCRM.winform.Controls
{
    /// <summary>
    /// Landing page. Deliberately shows no fabricated statistics -
    /// only Customer Management is wired to real data at this stage.
    /// </summary>
    public class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            Dock = DockStyle.Fill;
            BackColor = Theme.PageBackground;

            var lblWelcome = new Label
            {
                Text = "Welcome to SweetGlaze CRM",
                Font = Theme.FontHeading,
                ForeColor = Theme.TextPrimary,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            var lblInfo = new Label
            {
                Text = "Customer Management is currently available. Other modules are in development.",
                Font = Theme.FontSubheading,
                ForeColor = Theme.TextSecondary,
                AutoSize = true,
                Location = new Point(0, 34)
            };

            Controls.Add(lblWelcome);
            Controls.Add(lblInfo);

            string[,] cards =
            {
                { "Customers", "Available" },
                { "Companies", "Coming Soon" },
                { "Sales", "Coming Soon" },
                { "Reports", "Coming Soon" }
            };

            int x = 0;
            int y = 76;
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                var card = new Panel
                {
                    Size = new Size(220, 100),
                    Location = new Point(x, y),
                    BackColor = Theme.CardBackground
                };

                var lblName = new Label
                {
                    Text = cards[i, 0],
                    Font = Theme.FontBold,
                    ForeColor = Theme.TextPrimary,
                    AutoSize = true,
                    Location = new Point(16, 16)
                };

                bool available = cards[i, 1] == "Available";
                var lblBadge = new Label
                {
                    Text = cards[i, 1],
                    Font = Theme.FontBody,
                    ForeColor = available ? Theme.Success : Theme.TextSecondary,
                    AutoSize = true,
                    Location = new Point(16, 44)
                };

                card.Controls.Add(lblName);
                card.Controls.Add(lblBadge);
                Controls.Add(card);

                x += 236;
            }
        }
    }
}
