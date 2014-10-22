using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FlowOptimization.Data.Pipeline;

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
        /// <summary>
        /// Подсвечивает все ненулевые элементы DataGridView
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="color"></param>
        public static void SetBackLight(DataGridView dgv, Color color)
        {
            for (int i = 0; i < dgv.RowCount - 1; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Rows[i].Cells[j].Value.ToString() != "0")
                    {
                        dgv.Rows[i].Cells[j].Style.BackColor = color;
                        
                    }
                }
            }
        }
        /// <summary>
        /// Подсвечивает элементы узла, который выбран в данный момент
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="node"></param>
        /// <param name="color"></param>
        public static void SetBackLight(DataGridView dgv, Node node, Color color)
        {
            for (int i = 0; i < dgv.RowCount - 1; i++)
            {
                if (dgv.Rows[node.ID - 1].Cells[i].Value.ToString() != "0")
                {
                    dgv.Rows[node.ID - 1].Cells[i].Style.BackColor = color;
                    dgv.Rows[i].Cells[node.ID - 1].Style.BackColor = color;
                }
            }
        }
        /// <summary>
        /// Подсвечивает 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="nodes"></param>
        /// <param name="color"></param>
        public static void SetBackLight(DataGridView dgv, List<Node> nodes, Color color)
        {
            for (int i = 0; i < dgv.RowCount - 1; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (nodes[i].NodeType == Node.NodesType.Enter)
                        dgv.Rows[i].Cells[j].Style.BackColor = color;
                }
            }
        }
    }
}
