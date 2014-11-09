using System;
using System.Collections.Generic;
using FlowOptimization.Data.Pipeline;
using FlowOptimization.Math;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Полное описание пути от начальной точки до точки распределения
    /// </summary>
    class PathsMatrix : Matrix
    {
        private readonly IntersectionMatrix _intersectionMatrix;
        private readonly int[][] _routesMatrix;
        private readonly int[][] _encodedMatrix;
        private int[][] _decodedMatrix;

        public PathsMatrix(List<Node> nodes, IntersectionMatrix intersectionMatrix, int[][] routesMatrix)
            : base(nodes)
        {
            _intersectionMatrix = intersectionMatrix;
            _routesMatrix = routesMatrix;

            _encodedMatrix = GetEncodedMatrix();
        }

        public override int[][] GetMatrix()
        {
            return GetDecodedMatrix();
        }

        /// <summary>
        /// Вернуть строку из матрицы путей, содержащую необходимый путь
        /// </summary>
        /// <param name="start">Стартовый узел</param>
        /// <param name="end">Конечный узел</param>
        /// <returns></returns>
        public int[] GetPath(int start, int end)
        {
            int pathIndex = -1;
            for (int i = 0; i < _decodedMatrix.Length; i++)
            {
                int firstNode = _decodedMatrix[i][0];
                if (firstNode == start)
                {
                    for (int j = _decodedMatrix[i].Length - 1; j > 0; j--)
                    {
                        int endNode = _decodedMatrix[i][j];
                        if (endNode == end)
                            pathIndex = i;
                        else if (endNode == 0)
                            continue;
                        else if (endNode != 0)
                            break;
                    }
                }
            }
            if (pathIndex != -1)
            {
                int[] path = _decodedMatrix[pathIndex];
                return path;
            }
            return new int[] {};
        }

        /// <summary>
        /// Получить общую матрицу путей
        /// </summary>
        /// <returns></returns>
        private int[][] GetEncodedMatrix()
        {
            int[][] encodedPathMatrix = InitializeMatrix(StartNodesIDs.Count, Nodes.Count);
            var dijkstraAlgorithm = new DijkstraAlgorithm(Nodes, _intersectionMatrix);
            for (int i = 0; i < StartNodesIDs.Count; i++)
                encodedPathMatrix[i] = dijkstraAlgorithm.GetRoute(Nodes[StartNodesIDs[i] - 1],
                    Nodes[EndNodesIDs[EndNodesIDs.Count - 1] - 1]);
            return encodedPathMatrix;
        }

        /// <summary>
        /// Получить матрицу путей для каждой связи вход-выход
        /// </summary>
        /// <returns></returns>
        private int[][] GetDecodedMatrix()
        {
            _decodedMatrix = InitializeMatrix(_routesMatrix.Length, Nodes.Count);

            for (int i = 0; i < _routesMatrix.Length; i++)
            {
                int startNode = _routesMatrix[i][0];
                int endNode = _routesMatrix[i][1];

                _decodedMatrix[i] = BuildPath(startNode, endNode);
            }
            return _decodedMatrix;
        }

        /// <summary>
        /// Построить путь на основании общей матрицы путей
        /// </summary>
        /// <param name="startNode">Номер входного узла</param>
        /// <param name="endNode">Номер выходного узла</param>
        /// <returns></returns>
        private int[] BuildPath(int startNode, int endNode)
        {
            var path = new int[Nodes.Count];
            path[0] = startNode;
            int counter = 1;
            int temp = endNode - 1;

            while (true)
            {
                temp = _encodedMatrix[startNode - 1][temp];
                if (temp != startNode - 1)
                {
                    path[counter] = temp + 1;
                    counter++;
                }
                else                
                    break;                                    
            }

            SortPath(path, counter);
            path[counter] = endNode;

            return path;
        }

        /// <summary>
        /// Отсортировать вектор пути
        /// </summary>
        /// <param name="path"></param>
        /// <param name="counter"></param>
        private static void SortPath(int[] path, int counter)
        {
            if (counter < 2) return;
            int counter2 = 1;
            while (true)
            {
                if (counter2 < counter - counter2)
                {
                    int temp = path[counter - counter2];
                    path[counter - counter2] = path[counter2];
                    path[counter2] = temp;
                    counter2++;
                }
                else
                    break;
            }
        }
    }
}
