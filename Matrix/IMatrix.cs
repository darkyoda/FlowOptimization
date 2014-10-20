using System.Data;

namespace FlowOptimization.Matrix
{
    interface IMatrix
    {
        int[][] InitializeMatrix(int x, int y);
        void AddRow(int[][] matrix, int rowNumber, int columnsCount);
        int[][] GetMatrix();
    }
}
