using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FlowOptimization.UI
{
    static class MainFormPresets
    {
        /// <summary>
        /// Настройка горизонтальных label
        /// </summary>
        /// <param name="label">label для настройки</param>
        /// <param name="text">Текст label</param>
        public static void SetHorizontalLabel(System.Windows.Forms.Label label, string text)
        {
            //const int x = 15;
            //const int y = 5;
            //label.Location = new Point(x, y);
            //label.Location = new Point();
            label.Text = text;
        }

        /// <summary>
        /// Настройка вертикального label
        /// </summary>
        /// <param name="label">label для настройки</param>
        /// <param name="text">Текст label</param>
        public static void SetVerticalLabel(System.Windows.Forms.Label label, string text)
        {
            //const int x = 3;
            //const int y = 21;
            //label.Location = new Point(x, y);
            string temp = "";
            for (int i = 0; i < text.Length; i++)
                temp += text[i] + "\n";

            label.Text = temp;
            label.Anchor = AnchorStyles.Left;
        }

        /// <summary>
        /// Настройка datagridview
        /// </summary>
        /// <param name="dataGridView">datagridview для настройки</param>
        private static void SetDataGridView(System.Windows.Forms.DataGridView dataGridView)
        {
            var cellStyle = new DataGridViewCellStyle
            {
                Font = new Font(new FontFamily("Microsoft Sans Serif"), (float) 5.25)
            };
            dataGridView.DefaultCellStyle = cellStyle;
        }

        public static void SetDataGridViews(List<System.Windows.Forms.DataGridView> dataGridViews)
        {
            foreach (var dataGridView in dataGridViews)
                SetDataGridView(dataGridView);
        }
    }
}
