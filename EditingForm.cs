using System;
using System.Windows.Forms;

namespace FlowOptimization
{
    public partial class EditingForm : Form
    {
        private string textBoxValue;

        public string TextBoxValue
        {
            get { return textBoxValue; }
            set { textBoxValue = value; }
        }

        public EditingForm(MainForm.Commands command)
        {
            InitializeComponent();

            if (command == MainForm.Commands.NodeName)
            {
                label1.Text = "Введите название узла";
            }
            else if (command == MainForm.Commands.NodeVolume)
            {
                label1.Text = "Введите объем узла";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextBoxValue = textBox1.Text;
            Close();
        }


    }
}
