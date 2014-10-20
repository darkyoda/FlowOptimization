using System.Data;
using System.Drawing;
using FlowOptimization.Utilities;

namespace FlowOptimization.UI.DataGridView
{
    static class DataGridContent
    {
        public static void BuildContent(System.Windows.Forms.DataGridView dataGridView, DataTable table)
        {
            dataGridView.DataSource = table;
            DataTableUtilities.SetRowNumber(dataGridView);
        }
    }
}
