using System.Windows.Forms;

namespace FlowOptimization
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            label1.Text = "Расчет фактических маршрутов транспортирования газа\n" +
                          "Версия: 1.0.3\n" +
                          "ООО Газпром Информ\n" +
                          "Разработчик: Устиц Р.А.\n" +
                          "Руководитель: Тетерев В.В.";
        }
    }
}
