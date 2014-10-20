using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FlowOptimization.Data.Pipeline;
using FlowOptimization.Math;
using Microsoft.SqlServer.Server;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Работа с матрицами путей и длин маршрутов
    /// Для поиска используется алгоритм Дейкстры
    /// </summary>
    internal class DistanceMatrix : Matrix
    {
        private int[][] _routesMatrix;
        private readonly int[][] _distanceMatrix;

        private DijkstraAlgorithm _dijkstra;

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
            for (int i = 0; i < StartNodesIDs.Count-3; i++)
            {
                // Без разницы какой конечный узел, так как алгоритм все равно считает кратчайшие пути от начальной точки
                // В данном случае берем последний выходной узел
                int[] row = _dijkstra.GetSolution(Nodes[StartNodesIDs[i] - 1], Nodes[EndNodesIDs[EndNodesIDs.Count - 1] - 1]);

                for (int k = 0; k < Nodes.Count; k++)
                {
                    if ((Nodes[k].NodeType == Node.Type.Default && Nodes[k].Volume == 0) || Nodes[k].NodeType == Node.Type.Enter)
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

        /// <summary>
        /// Строит матрицу, в каждой строке которого содержится кратчайший маршрут от конечной до начальной точки (не учитывая конечную точку)
        /// </summary>
        /// <returns></returns>
        public int[][] GetRoutesMatrix()
        {
            _routesMatrix = InitializeMatrix(StartNodesIDs.Count*EndNodesIDs.Count, Nodes.Count);
            
            int counter = 0;
            for (int i = 0; i < StartNodesIDs.Count; i++)
            {
                for (int j = 0; j < EndNodesIDs.Count; j++)
                {
                    int[] routeVector = _dijkstra.GetSolution(Nodes[StartNodesIDs[i] - 1], Nodes[EndNodesIDs[j] - 1]);
                    
                    for (int k = 0; k < Nodes.Count; k++)
                    {
                        if (routeVector[k] != 0)
                            _routesMatrix[counter][k] = routeVector[k];
                        else
                            _routesMatrix[counter][k] = 0;
                    }
                    counter++;
                }          
            }

            counter = 0;
            int counter2 = 0;
            int temp = 0;

            int[][] tempMatrix = InitializeMatrix(StartNodesIDs.Count*EndNodesIDs.Count, Nodes.Count);
            for (int i = 0; i < StartNodesIDs.Count; i++)
            {
                for (int j = 0; j < EndNodesIDs.Count; j++)
                {                    
                    temp = EndNodesIDs[j];
                    while (temp != StartNodesIDs[i])
                    {
                        for (int k = 0; k < Nodes.Count; k++)
                        {
                            if (k == temp - 1)
                            {
                                tempMatrix[counter][counter2] = _routesMatrix[counter][k];
                                temp = _routesMatrix[counter][k];
                                counter2++;
                            }
                        } 
                    }
                    counter++;
                    counter2 = 0;
                }                
            }

            _routesMatrix = tempMatrix;
            return _routesMatrix;
        }
    }
}
