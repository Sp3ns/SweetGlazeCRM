using System.Drawing;

namespace SweetGlazeCRM.winform.UI
{
    /// <summary>
    /// Single source of truth for colors and fonts so the sidebar, buttons,
    /// forms, tables and dialogs all look consistent.
    /// </summary>
    public static class Theme
    {
        public static readonly Color SidebarBackground = Color.FromArgb(28, 31, 43);
        public static readonly Color SidebarText = Color.FromArgb(198, 201, 214);
        public static readonly Color SidebarActiveBackground = Color.FromArgb(255, 111, 97);
        public static readonly Color SidebarActiveText = Color.White;

        public static readonly Color PageBackground = Color.FromArgb(246, 247, 250);
        public static readonly Color CardBackground = Color.White;

        public static readonly Color Accent = Color.FromArgb(255, 111, 97);
        public static readonly Color AccentDark = Color.FromArgb(224, 92, 79);

        public static readonly Color TextPrimary = Color.FromArgb(35, 38, 47);
        public static readonly Color TextSecondary = Color.FromArgb(120, 124, 138);
        public static readonly Color Border = Color.FromArgb(226, 228, 234);

        public static readonly Color Success = Color.FromArgb(46, 160, 91);
        public static readonly Color Danger = Color.FromArgb(214, 69, 65);

        public static readonly Font FontHeading = new Font("Segoe UI", 16f, FontStyle.Bold);
        public static readonly Font FontSubheading = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static readonly Font FontBody = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        public static readonly Font FontBold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font FontNav = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static readonly Font FontButton = new Font("Segoe UI", 9.5f, FontStyle.Bold);
    }
}
