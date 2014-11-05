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
        public int[][] IcvTtr;  // ТТР по итерация с учетом независимых поставщиков
        private readonly int[][] _distributionMatrix;
        private readonly int[][] _icvDistributionMatrix;
        private RoutesMatrix _routesMatrix;
        public PassabilityMatrix PassabilityMatrix;

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
                        
                       // PassabilityMatrix.AddValue(beginIndex, endIndex, volumeVector[endIndex]);
                        // Изменяем значения объемов и ТТР
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

                        //PassabilityMatrix.AddValue(beginIndex, endIndex, volumeVector[beginIndex]);
                        
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

            IcvTtr = InitializeMatrix(Nodes.Count, EndNodesIDs.Count*StartNodesIDs.Count);

            for (int i = 0; i < routesMatrix.Length; i++)
            {
                int beginIndex = routesMatrix[i][0] - 1; // Номер входного узла
                int endIndex = routesMatrix[i][1] - 1;   // Номер узла, который необходимо заполнить 
                int routeLength = routesMatrix[i][2];    // Длина маршрута до узла, который необходимо заполнить

                for (int icvColumn = 0; icvColumn < volumeMatrix[beginIndex].Length; icvColumn++)
                {
                    if (volumeMatrix[beginIndex][icvColumn] != 0 && volumeMatrix[endIndex][icvColumn] != 0)
                    {
                        // Если поток способен полностью заполнить узел
                        if (volumeMatrix[beginIndex][icvColumn] - volumeMatrix[endIndex][icvColumn] >= 0 && routesMatrix[i][3] != 1)
                        {
                            // Вычитаем из поток входного узла и добавляем в назначенный узел
                            //_icvDistributionMatrix[beginIndex][counter] = volumeMatrix[beginIndex][icvColumn] - volumeMatrix[endIndex][icvColumn];
                            _icvDistributionMatrix[beginIndex][counter] = icvColumn + 1;
                            _icvDistributionMatrix[endIndex][counter] += volumeMatrix[endIndex][icvColumn];
                            // Добавить промежуточные узлы для последующего заполнения
                            //routesList.Add(FillNodes(distance, beginIndex + 1, endIndex + 1));

                            volumeMatrix[beginIndex][icvColumn] -= volumeMatrix[endIndex][icvColumn];
                            IcvTtr[endIndex][counter] = volumeMatrix[endIndex][icvColumn] * routeLength;
                            //Nodes[routesMatrix[i][1] - 1].Ttr += volumeMatrix[endIndex][icvColumn] * routeLength;
                            volumeMatrix[endIndex][icvColumn] -= volumeMatrix[endIndex][icvColumn];
                            int temp = 0;
                            // Если узел дозаполнен, то делаем пометку для следующих путей, что он заполнен\
                            for (int t = 0; t < volumeMatrix[beginIndex].Length; t++)
                            {
                                temp += volumeMatrix[endIndex][t];
                                
                            }

                            if (temp == 0)
                            {
                                for (int j = 0; j < routesMatrix.Length; j++)
                                {
                                    if (routesMatrix[j][1] == routesMatrix[i][1])
                                        routesMatrix[j][3] = 1;
                                }
                            }

                            counter++;
                        }
                        // Если поток не способен полностью заполнить узел
                        else if (volumeMatrix[beginIndex][icvColumn] - volumeMatrix[endIndex][icvColumn] < 0 && routesMatrix[i][3] != 1)
                        {
                            //_icvDistributionMatrix[beginIndex][counter] -= volumeMatrix[beginIndex][icvColumn];
                            _icvDistributionMatrix[beginIndex][counter] = icvColumn + 1;
                            _icvDistributionMatrix[endIndex][counter] += volumeMatrix[beginIndex][icvColumn];
                            IcvTtr[endIndex][counter] = volumeMatrix[beginIndex][icvColumn] * routeLength;
                            //Nodes[routesMatrix[i][1] - 1].Ttr += volumeMatrix[beginIndex][icvColumn] * routeLength;

                            //routesList.Add(FillNodes(distance, beginIndex, endIndex));
                            volumeMatrix[endIndex][icvColumn] -= volumeMatrix[beginIndex][icvColumn];
                            
                            volumeMatrix[beginIndex][icvColumn] = 0;

                            counter++;
                        }
                            //break;
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
            PassabilityMatrix = new PassabilityMatrix(_routesMatrix.PathsMatrix);
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
    }
}
