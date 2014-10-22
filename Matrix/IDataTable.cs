using System.Data;

namespace FlowOptimization.Matrix
{
    interface IDataTable<in T>
    {
        void InitializeTable(DataTable table, T[][] matrix);
        void AddRow(DataTable table, T[][] matrix, int rowNumber);
        DataTable GetTable(T[][] matrix);
    }
}
