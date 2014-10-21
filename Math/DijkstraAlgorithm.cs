using FlowOptimization.Data.Pipeline;
using FlowOptimization.Matrix;
using System.Collections.Generic;

namespace FlowOptimization.Math
{
    /// <summary>
    /// Расчет кратчайших путей алгоритмом Дейкстры
    /// </summary>
    class DijkstraAlgorithm
    {
        private const int Infinity = 1000;  // Условная бесконечность для алгоритма 
        private readonly List<Node> _nodes; // Список всех узлов
        private readonly int[][] _intersectionMatrix;   // Матрица пересечений
        private int[] _shortPath;   // Длина кратчайшего пути от вершины start в i
        private int[] _previousNode;    // Вершина, предшествующая i-й вершине на кратчайшем пути

        public DijkstraAlgorithm(List<Node> nodes, IntersectionMatrix intersectionMatrix)
        {
            _nodes = nodes;
            _intersectionMatrix = intersectionMatrix.GetMatrix();
        }

        /// <summary>
        /// Нахождение кратчайшего пути от вершины с номером startNodeID до конечной вершины endNodeID
        /// </summary>
        /// <param name="startNodeID">Начальная вершина</param>
        /// <param name="endNodeID">Конечная вершина</param>
        /// <param name="shortPath">Кратчайшие пути</param>
        /// <param name="previousNode">Предыдущие вершины</param>
        private void SolveGraph(int startNodeID, int endNodeID, out int[] shortPath, out int[] previousNode)
        {
            int nodeCount = _nodes.Count;

            int start = startNodeID - 1;
            int end = endNodeID - 1;
            int temp;

            int[] x = new int[nodeCount];   // Массив, содержащий единицы и нули для каждой вершины

            shortPath = new int[nodeCount];   
            previousNode = new int[nodeCount];   

            int nodeCounter;    // Счетчик вершин
            for (nodeCounter = 0; nodeCounter < nodeCount; nodeCounter++)
            {
                shortPath[nodeCounter] = Infinity;
                x[nodeCounter] = 0;
            }
          
            previousNode[start] = 0;
            shortPath[start] = 0;
            x[start] = 1;

            temp = start;

            while (true)
            {
                // Перебираем все вершины, смежные temp, и ищем для них кратчайший путь
                for (nodeCounter = 0; nodeCounter < nodeCount; nodeCounter++)
                {
                    if (_intersectionMatrix[temp][nodeCounter] == 0) continue;    // Вершины не смежные
                    if (x[nodeCounter] == 0 && shortPath[nodeCounter] > shortPath[temp] + _intersectionMatrix[temp][nodeCounter])
                    {
                        shortPath[nodeCounter] = shortPath[temp] + _intersectionMatrix[temp][nodeCounter];
                        previousNode[nodeCounter] = temp;
                    }
                }

                int w = Infinity;
                temp = -1;

                for (nodeCounter = 0; nodeCounter < nodeCount; nodeCounter++)
                {
                    if (x[nodeCounter] == 0 && shortPath[nodeCounter] < w)
                    {
                        temp = nodeCounter;
                        w = shortPath[nodeCounter];
                    }
                }
                if (temp == -1)
                {
                    break;
                }
                if (temp == end)
                {
                    nodeCounter = end;
                    while (nodeCounter != start)
                    {
                        nodeCounter = previousNode[nodeCounter];
                    }
                    break;
                }
                x[temp] = 1;
            }
        }

        /// <summary>
        /// Получить вектор кратчайших путей
        /// </summary>
        /// <param name="startNode">Начальная вершина</param>
        /// <param name="endNode">Конечная вершина</param>
        /// <returns></returns>
        public int[] GetSolution(Node startNode, Node endNode)
        {
            SolveGraph(startNode.ID, endNode.ID, out _shortPath, out _previousNode);
            
            return _shortPath;
        }

        /// <summary>
        /// Получить вектор прыдыдущих вершин
        /// </summary>
        /// <param name="startNode">Начальная вершина</param>
        /// <param name="endNode">Конечная вершина</param>
        /// <returns></returns>
        public int[] GetRoute(Node startNode, Node endNode)
        {
            SolveGraph(startNode.ID, endNode.ID, out _shortPath, out _previousNode);
          
            return _previousNode;
        }

    }
    
}
