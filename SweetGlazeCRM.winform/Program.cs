using SweetGlazeCRM.winform.Forms;

namespace SweetGlazeCRM.winform
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.Run(
                new MainForm());
        }
    }
}