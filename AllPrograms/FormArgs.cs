using System.Windows.Forms;

namespace AllPrograms
{
    public partial class FormArgs : Form
    {
        public FormArgs()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FormMain.newAppArgs = textBox1.Text;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
