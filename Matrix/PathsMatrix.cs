using System.Collections.Generic;
using FlowOptimization.Data.Pipeline;
using FlowOptimization.Math;

namespace FlowOptimization.Matrix
{
    class PathsMatrix : Matrix
    {
        private readonly IntersectionMatrix _intersectionMatrix;
        private int[][] _routesMatrix;
        private readonly int[][] _encodedMatrix;

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
            int[][] decoded = InitializeMatrix(_routesMatrix.Length, Nodes.Count);

            for (int i = 0; i < _routesMatrix.Length; i++)
            {
                int startNode = _routesMatrix[i][0];
                int endNode = _routesMatrix[i][1];

                decoded[i] = BuildPath(startNode, endNode);
            }
            return decoded;
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
