using System;
using System.Collections.Generic;
using System.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Матрица маршрутов
    /// </summary>
    class RoutesMatrix : Matrix
    {
        private readonly int[][] _routesMatrix;
        private DistanceMatrix _distanceMatrix;
        private PathsMatrix _pathsMatrix;
        public RoutesMatrix(List<Node> nodes) : base(nodes)
        {
            _routesMatrix = InitializeMatrix(EndNodesIDs.Count * StartNodesIDs.Count, 5);
        }

        public override void InitializeTable(DataTable table, int[][] matrix)
        {
            table.Columns.Add("Начальный узел", typeof(int));
            table.Columns.Add("Конечный узел", typeof(int));
            table.Columns.Add("Длина", typeof(int));
            table.Columns.Add("Посещен", typeof(int));
            table.Columns.Add("Путь", typeof(string));
        }

        public PathsMatrix GetPathsMatrix()
        {
            return _pathsMatrix;
        }

        public DataTable GetTable(DistanceMatrix distanceMatrix)
        {
            _distanceMatrix = distanceMatrix;
            CalculateMatrix();
          
            var table = GetTable(_routesMatrix);
            // Удаляем столбец "Посещен"
            table.Columns.RemoveAt(3);

            AddPaths(table);

            return table;
        }

        /// <summary>
        /// Добавить пути в матрицу маршрутов
        /// </summary>
        /// <param name="table"></param>
        private void AddPaths(DataTable table)
        {
            _pathsMatrix = new PathsMatrix(Nodes, _distanceMatrix.IntersectionMatrix, GetMatrix());

            for (int i = 0; i < GetMatrix().Length; i++)
            {
                string t = "";
                for (int j = 0; j < _pathsMatrix.GetMatrix()[i].Length; j++)
                {
                    if (_pathsMatrix.GetMatrix()[i][j] != 0)
                        t += _pathsMatrix.GetMatrix()[i][j].ToString() + "-";
                }

                table.Rows[i][3] = t.Remove(t.Length - 1);
            }
        }

        /// <summary>
        /// Получить матрицу маршрутов
        /// </summary>
        /// <returns></returns>
        public override int[][] GetMatrix()
        {
            return _routesMatrix;
        }

        /// <summary>
        /// Рассчитать матрицу маршрутов
        /// </summary>
        private void CalculateMatrix()
        {
            int[][] distanceMatrix = _distanceMatrix.GetMatrix();

            int counter = 0;
            for (int i = 0; i < StartNodesIDs.Count; i++)
            {
                for (int j = 0; j < Nodes.Count; j++)
                {
                    if (distanceMatrix[i][j] != 0)
                    {
                        _routesMatrix[counter][0] = i + 1;
                        _routesMatrix[counter][1] = j + 1;
                        _routesMatrix[counter][2] = distanceMatrix[i][j];
                        _routesMatrix[counter][3] = -1;
                        counter++;
                    }
                }
            }

            SortMatrix(_routesMatrix);
        }

        /// <summary>
        /// Отсортировать матрицу маршрутов
        /// </summary>
        /// <param name="matrix">Матрица маршрутов</param>
        private void SortMatrix(int[][] matrix)
        {
            // Временные переменные для сортировки
            int temp = 0;
            int temp1 = 0;
            int temp2 = 0;
            // Сортировка
            for (int j = 0; j < EndNodesIDs.Count * StartNodesIDs.Count - 1; j++)
            {
                for (int i = 0; i < EndNodesIDs.Count * StartNodesIDs.Count - 1; i++)
                {
                    if (matrix[i][2] != 0)
                    {
                        if (matrix[i][2] > matrix[i + 1][2])
                        {
                            temp = matrix[i + 1][2];
                            temp1 = matrix[i + 1][0];
                            temp2 = matrix[i + 1][1];
                            matrix[i + 1][2] = matrix[i][2];
                            matrix[i + 1][0] = matrix[i][0];
                            matrix[i + 1][1] = matrix[i][1];
                            matrix[i][2] = temp;
                            matrix[i][0] = temp1;
                            matrix[i][1] = temp2;
                        }
                    }
                }
            }
        }
    }
}
