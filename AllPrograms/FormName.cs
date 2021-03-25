using System.Windows.Forms;

namespace AllPrograms
{
    public partial class FormName : Form
    {
        public FormName()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FormMain.newAppName = textBox1.Text;
                DialogResult = DialogResult.OK;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Dispose();
            }
        }
    }
}
