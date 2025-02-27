using System.Windows.Forms;

namespace AllPrograms
{
    public partial class FormAdd : Form
    {
        bool setArgs = false;

        public FormAdd(bool args)
        {
            InitializeComponent();
            label1.Text = args ? "Ключ(и) запуска:" : "Название ярлыка:";
            setArgs = args;
        }

        void button1_Click(object sender, System.EventArgs e)
        {
            if (setArgs)
            {
                FormMain.newAppArgs = textBox1.Text;
            }
            else
            {
                FormMain.newAppName = textBox1.Text;
            }
        }
    }
}
