using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FlowOptimization.Data.Pipeline;
using FlowOptimization.Math;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Работа с матрицами путей и длин маршрутов
    /// Для поиска используется алгоритм Дейкстры
    /// </summary>
    internal class DistanceMatrix : Matrix
    {
        private readonly int[][] _distanceMatrix;
        private DijkstraAlgorithm _dijkstra;

        public IntersectionMatrix IntersectionMatrix;

        public DistanceMatrix(List<Node> nodes)
            : base (nodes)
        {
            _distanceMatrix = InitializeMatrix(StartNodesIDs.Count, Nodes.Count);
        }
        
        /// <summary>
        /// Строит матрицу с длиной пути от начальной точки до каждой точки, которую необходимо заполнить
        /// </summary>
        /// <param name="intersectionMatrix">Матрица пересечений</param>
        /// <returns></returns>
        public DataTable GetTable(IntersectionMatrix intersectionMatrix)
        {
            IntersectionMatrix = intersectionMatrix;
            try
            {
                CalculateMatrix(intersectionMatrix);
                var table = GetTable(_distanceMatrix);

                return table;
            }
            catch (IndexOutOfRangeException e)
            {
                MessageBox.Show(@"В");
                return null;
                
            }
            catch (Exception e)
            {
                //string error = String.Format("{0}", e);
                MessageBox.Show(@"Невозможно произвести расчет\n" + e);
                return null;
            }
        }

        public void CalculateMatrix(IntersectionMatrix intersectionMatrix)
        {
            _dijkstra = new DijkstraAlgorithm(Nodes, intersectionMatrix);
            int counter = 0;
            for (int i = 0; i < StartNodesIDs.Count; i++)
            {
                // Без разницы какой конечный узел, так как алгоритм все равно считает кратчайшие пути от начальной точки
                // В данном случае берем последний выходной узел
                int[] row = _dijkstra.GetSolution(Nodes[StartNodesIDs[i] - 1], Nodes[EndNodesIDs[EndNodesIDs.Count - 1] - 1]);

                for (int k = 0; k < Nodes.Count; k++)
                {
                    if ((Nodes[k].NodeType == Node.NodesType.Default && Nodes[k].Volume == 0) || Nodes[k].NodeType == Node.NodesType.Enter)
                        _distanceMatrix[counter][k] = 0;
                    else
                        _distanceMatrix[counter][k] = row[k];
                }
                counter++;
            }

            var lengthVector = new List<int>();

            for (int i = 0; i < StartNodesIDs.Count; i++)
            {
                for (int j = 0; j < Nodes.Count; j++)
                {
                    if (_distanceMatrix[i][j] != 0)
                        lengthVector.Add(_distanceMatrix[i][j]);
                }
            }
            lengthVector.Sort();
        }

        public override int[][] GetMatrix()
        {
            return _distanceMatrix;
        }
    }
}
