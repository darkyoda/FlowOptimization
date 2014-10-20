using System.Globalization;
using System.Windows.Forms;

namespace FlowOptimization.Utilities
{
    /// <summary>
    /// Утилиты для работы с DataTable
    /// </summary>
    class DataTableUtilities
    {
        /// <summary>
        /// Нумерует заголовочный столбец DataGridView
        /// </summary>
        /// <param name="dgv">Объект DataGridView</param>
        public static void SetRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
                row.HeaderCell.Value = (row.Index + 1).ToString();
        }
    }
}
