using System.Collections.Generic;
using System.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Базовый класс
    /// </summary>
    class Matrix : IMatrix, IDataTable
    {
        protected List<Node> Nodes; // Список узлов
        protected List<Pipe> Pipes; // Список связей
        protected DataRow DataRow;  // Строка DataTable

        protected List<int> StartNodesIDs;
        protected List<int> EndNodesIDs;

        public Matrix()
        {
            
        }

        public Matrix(List<Node> nodes)
        {
            Nodes = nodes;

            StartNodesIDs = new List<int>();
            EndNodesIDs = new List<int>();

            if (Nodes != null)
            {
                foreach (var node in Nodes)
                {
                    if (node.NodeType == Node.Type.Enter)
                        StartNodesIDs.Add(node.ID);
                    else if (node.NodeType == Node.Type.Exit)
                        EndNodesIDs.Add(node.ID);
                }
            }
        }

        /// <summary>
        /// Инициализация матрицы размеров sizeX x sizeY
        /// </summary>
        /// <param name="sizeX">Количество строк</param>
        /// <param name="sizeY">Количество столбцов</param>
        public int[][] InitializeMatrix(int sizeX, int sizeY)
        {
            var tempMatrix = new int[sizeX][];
            for (int i = 0; i < sizeX; i++)
                AddRow(tempMatrix, i, sizeY);

            return tempMatrix;
        }

        /// <summary>
        /// Добавить строку в матрицу 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="rowNumber">Номер строки</param>
        /// <param name="columnsCount">Количество столбцов</param>
        public void AddRow(int[][] matrix, int rowNumber, int columnsCount)
        {
            matrix[rowNumber] = new int[columnsCount];
            for (int j = 0; j < columnsCount; j++)
            {
                matrix[rowNumber][j] = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int[][] GetMatrix()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Инициализация столбцов для DataTable
        /// </summary>
        public virtual void InitializeTable(DataTable table, int[][] matrix)
        {
            for (int i = 1; i < matrix[0].Length + 1; i++)
            {
                if (!table.Columns.Contains(i.ToString()))
                    table.Columns.Add(i.ToString(), typeof(int));
            }
        }

        /// <summary>
        /// Добавить строку в DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="matrix"></param>
        /// <param name="rowNumber"></param>
        public void AddRow(DataTable table, int[][] matrix, int rowNumber)
        {
            DataRow = table.NewRow();

            for (int i = 0; i < matrix[rowNumber].Length; i++)
                DataRow[i] = matrix[rowNumber][i];

            table.Rows.Add(DataRow);
        }

        /// <summary>
        /// Получить DataTable
        /// </summary>
        /// <param name="matrix">Расчетная матрица</param>
        /// <returns></returns>
        public DataTable GetTable(int[][] matrix)
        {
            var tempTable = new DataTable();
            InitializeTable(tempTable, matrix);
            ConvertToDataTable(matrix, tempTable);

            return tempTable;
        }

        /// <summary>
        /// Конвертация полученной таблицы в переданный DataTable
        /// Передавать DataTable необходимо в следствии того, что перед вызовом метода необходимо инициализировать
        /// столбцы DataTable
        /// </summary>
        /// <param name="matrix">Матрица, которую необходимо конвертировать</param>
        /// <param name="table">Таблица с предварительно инициализированными столбцами</param>
        /// <returns></returns>
        private void ConvertToDataTable(int[][] matrix, DataTable table)
        {
            for (int i = 0; i < matrix.Length; i++)
                AddRow(table, matrix, i);
        }
    }
}
