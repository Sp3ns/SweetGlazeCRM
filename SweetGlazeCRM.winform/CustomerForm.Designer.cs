
namespace SweetGlazeCRM.winform
{
    partial class CustomerForm
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(96F, 96F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Dpi;

            this.ClientSize =
                new System.Drawing.Size(1400, 850);

            this.MinimumSize =
                new System.Drawing.Size(1100, 700);

            this.Name =
                "CustomerForm";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text =
                "SweetGlaze CRM - Manager Dashboard";

            this.ResumeLayout(false);
        }
    }
}