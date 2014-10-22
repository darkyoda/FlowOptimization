using System.Data;

namespace FlowOptimization.Matrix
{
    interface IMatrix<T>
    {
        T[][] InitializeMatrix(int x, int y);
        void AddRow(T[][] matrix, int rowNumber, int columnsCount);
        T[][] GetMatrix();
    }
}
