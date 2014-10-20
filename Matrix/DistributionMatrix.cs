using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Класс для расчета распределения потоков и ТТР
    /// </summary>
    class DistributionMatrix : Matrix
    {
        public int[][] Ttr;    // ТТР по итерациям
        public int[][] IcvTtr;
        private readonly int[][] _distributionMatrix;
        private int[][] _icvDistributionMatrix;
        private RoutesMatrix _routesMatrix;

        public DistributionMatrix(List<Node> nodes)
            : base(nodes)
        {
            _distributionMatrix = InitializeMatrix(Nodes.Count, EndNodesIDs.Count * StartNodesIDs.Count);
            _icvDistributionMatrix = InitializeMatrix(Nodes.Count, EndNodesIDs.Count * StartNodesIDs.Count);
        }

        /// <summary>
        /// Построить матрицу распределения объемов и матрицу ТТР
        /// </summary>
        /// <returns></returns>
        private void CalculateMatrix()
        {
            int[][] routesMatrix = _routesMatrix.GetMatrix();
            int counter = 0;

            // Вектор текущих объемов на каждом узле
            int[] volumeVector = new int[Nodes.Count];       
            foreach (var node in Nodes)
                volumeVector[node.ID - 1] = node.Volume;

            Ttr = InitializeMatrix(Nodes.Count, EndNodesIDs.Count*StartNodesIDs.Count);

            for (int i = 0; i < routesMatrix.Length; i++)
            {
                int beginIndex = routesMatrix[i][0] - 1; // Номер входного узла
                int endIndex = routesMatrix[i][1] - 1;   // Номер узла, который необходимо заполнить 
                int routeLength = routesMatrix[i][2];    // Длина маршрута до узла, который необходимо заполнить

                if (volumeVector[beginIndex] != 0)
                {
                    // Если поток способен полностью заполнить узел
                    if (volumeVector[beginIndex] - volumeVector[endIndex] >= 0 && routesMatrix[i][3] != 1)
                    {
                        // Вычитаем из поток входного узла и добавляем в назначенный узел           
                        _distributionMatrix[beginIndex][counter] = volumeVector[beginIndex] - volumeVector[endIndex];
                        // Необходимо для подсветки в datagridview
                        if (_distributionMatrix[beginIndex][counter] == 0)
                            _distributionMatrix[beginIndex][counter] = -1;

                        _distributionMatrix[endIndex][counter] += volumeVector[endIndex];
                        // Добавить промежуточные узлы для последующего заполнения
                        //routesList.Add(FillNodes(distance, beginIndex + 1, endIndex + 1));

                        volumeVector[beginIndex] -= volumeVector[endIndex];
                        Ttr[endIndex][counter] = volumeVector[endIndex] * routeLength;
                        Nodes[routesMatrix[i][1] - 1].Ttr += volumeVector[endIndex] * routeLength;
                        volumeVector[endIndex] -= volumeVector[endIndex];

                        // Если узел дозаполнен, то делаем пометку для следующих путей, что он заполнен
                        for (int j = 0; j < routesMatrix.Length; j++)
                        {
                            if (routesMatrix[j][1] == routesMatrix[i][1])
                                routesMatrix[j][3] = 1;
                        }

                        counter++;
                    }
                    // Если поток не способен полностью заполнить узел
                    else if (volumeVector[beginIndex] - volumeVector[endIndex] < 0 && routesMatrix[i][3] != 1)
                    {
                        if (_distributionMatrix[beginIndex][counter] - volumeVector[beginIndex] < 0)
                            _distributionMatrix[beginIndex][counter] = -1;
                        //TempMatrix[beginIndex][counter] -= volumeVector[beginIndex];
                        
                        Ttr[endIndex][counter] = volumeVector[beginIndex] * routeLength;
                        Nodes[routesMatrix[i][1] - 1].Ttr += volumeVector[beginIndex] * routeLength;

                        //routesList.Add(FillNodes(distance, beginIndex, endIndex));
                        volumeVector[endIndex] -= volumeVector[beginIndex];
                        _distributionMatrix[endIndex][counter] += volumeVector[beginIndex];

                        volumeVector[beginIndex] = 0;

                        counter++;
                    }
                }
            }
        }

        private void CalculateMatrixWithIcv(ICVsMatrix icvs)
        {            
            int[][] icvsMatrix = icvs.GetMatrix();

            int[][] routesMatrix = _routesMatrix.GetMatrix();
            for (int i = 0; i < routesMatrix.Length; i++)
                routesMatrix[i][3] = -1;

            int counter = 0;

            // Матрица объемов
            int[][] volumeMatrix = InitializeMatrix(Nodes.Count, icvs.Count);

            for (int i = 0; i < icvsMatrix.Length; i++)
            {
                int icvNumber = icvsMatrix[i][1] - 1;
                int nodeNumber = icvsMatrix[i][2] - 1;
                int nodeVolume = icvsMatrix[i][3];
                volumeMatrix[nodeNumber][icvNumber] += nodeVolume;
            }

            //int[][] pathMatrix = distance.GetRoutesMatrix();

            IcvTtr = InitializeMatrix(Nodes.Count, EndNodesIDs.Count*StartNodesIDs.Count);

            List<List<int>> routesList = new List<List<int>>();

            for (int i = 0; i < routesMatrix.Length; i++)
            {
                int beginIndex = routesMatrix[i][0] - 1; // Номер входного узла
                int endIndex = routesMatrix[i][1] - 1;   // Номер узла, который необходимо заполнить 
                int routeLength = routesMatrix[i][2];    // Длина маршрута до узла, который необходимо заполнить

                for (int icvColumn = 0; icvColumn < volumeMatrix[beginIndex].Length; icvColumn++)
                {
                    if (volumeMatrix[beginIndex][icvColumn] != 0)
                    {
                        if (volumeMatrix[endIndex][icvColumn] != 0)
                        {
                            // Если поток способен полностью заполнить узел
                            if (volumeMatrix[beginIndex][icvColumn] - volumeMatrix[endIndex][icvColumn] > 0 && routesMatrix[i][3] != 1)
                            {
                                // Вычитаем из поток входного узла и добавляем в назначенный узел
                                _icvDistributionMatrix[beginIndex][counter] = volumeMatrix[beginIndex][icvColumn] - volumeMatrix[endIndex][icvColumn];
                                _icvDistributionMatrix[endIndex][counter] += volumeMatrix[endIndex][icvColumn];
                                // Добавить промежуточные узлы для последующего заполнения
                                //routesList.Add(FillNodes(distance, beginIndex + 1, endIndex + 1));

                                volumeMatrix[beginIndex][icvColumn] -= volumeMatrix[endIndex][icvColumn];
                                IcvTtr[endIndex][counter] = volumeMatrix[endIndex][icvColumn] * routeLength;
                                Nodes[routesMatrix[i][1] - 1].Ttr += volumeMatrix[endIndex][icvColumn] * routeLength;
                                volumeMatrix[endIndex][icvColumn] -= volumeMatrix[endIndex][icvColumn];

                                // Если узел дозаполнен, то делаем пометку для следующих путей, что он заполнен
                                for (int j = 0; j < routesMatrix.Length; j++)
                                {
                                    if (routesMatrix[j][1] == routesMatrix[i][1])
                                        routesMatrix[j][3] = 1;
                                }

                                counter++;
                            }
                            // Если поток не способен полностью заполнить узел
                            else if (volumeMatrix[beginIndex][icvColumn] - volumeMatrix[endIndex][icvColumn] < 0 && routesMatrix[i][3] != 1)
                            {
                                _icvDistributionMatrix[beginIndex][counter] -= volumeMatrix[beginIndex][icvColumn];

                                IcvTtr[endIndex][counter] = volumeMatrix[beginIndex][icvColumn] * routeLength;
                                Nodes[routesMatrix[i][1] - 1].Ttr += volumeMatrix[beginIndex][icvColumn] * routeLength;

                                //routesList.Add(FillNodes(distance, beginIndex, endIndex));
                                volumeMatrix[endIndex][icvColumn] -= volumeMatrix[beginIndex][icvColumn];
                                _icvDistributionMatrix[endIndex][counter] += volumeMatrix[beginIndex][icvColumn];

                                volumeMatrix[beginIndex][icvColumn] = 0;

                                counter++;
                            }
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="routesMatrix"></param>
        /// <returns></returns>
        public DataTable GetTable(RoutesMatrix routesMatrix)
        {
            _routesMatrix = routesMatrix;
            if (_routesMatrix == null) return null;
            try
            {
                CalculateMatrix();
                var table = GetTable(_distributionMatrix);

                return table;
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Невозможно произвести расчет
" + e.Message + @"
" + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routesMatrix"></param>
        /// <param name="icvMatrix"></param>
        /// <returns></returns>
        public DataTable GetTable(RoutesMatrix routesMatrix, ICVsMatrix icvMatrix)
        {
            _routesMatrix = routesMatrix;
            if (_routesMatrix == null) return null;
            try
            {
                CalculateMatrixWithIcv(icvMatrix);
                var table = GetTable(_icvDistributionMatrix);

                return table;
            }

            catch (Exception e)
            {
                MessageBox.Show(@"Невозможно произвести расчет
" + e.Message + @"
" + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Определяем у каких узлов (Default и Volume = 0) между входной точкой и точкой, которую необходимо заполнить, требуется учесть проходящие через них потоки
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="startID">Номер входного узла</param>
        /// <param name="endID">Номер узла, который должен быть заполнен</param>
        /// <returns></returns>
        private List<int> FillNodes(DistanceMatrix distance, int startID, int endID)
        {
            List<int> nodesToFill = new List<int>();
            int[][] pathMatrix = distance.GetRoutesMatrix();


            for (int i = 0; i < pathMatrix.Length; i++)
            {
                for (int j = 0; j < pathMatrix[i].Length; j++)
                {
                    if (pathMatrix[i][j] == endID)
                    {
                        for (int k = j + 1; k < pathMatrix[i].Length; k++)
                        {
                            if (pathMatrix[i][k] == startID)
                            {
                                for (int l = j + 1; l < k; l++)
                                {
                                    nodesToFill.Add(pathMatrix[i][l]);
                                }
                                return nodesToFill;
                            }
                        }
                        break;
                    }
                }
            }
            return null;
        }
    }
}
