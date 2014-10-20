using System.Data;

namespace FlowOptimization.Matrix
{
    interface IDataTable
    {
        void InitializeTable(DataTable table, int[][] matrix);
        void AddRow(DataTable table, int[][] matrix, int rowNumber);
        DataTable GetTable(int[][] matrix);
    }
}
