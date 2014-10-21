using System.Collections.Generic;
using System.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization.Matrix
{
    class TTRMatrix : Matrix
    {
        public TTRMatrix(List<Node> nodes) : base(nodes)
        {
        }
        /// <summary>
        /// Получить DataTable ТТР 
        /// </summary>
        /// <param name="distributionMatrix"></param>
        /// <returns></returns>
        public DataTable GetTable(DistributionMatrix distributionMatrix)
        {
            if (distributionMatrix != null)
            {
                var table = GetTable(distributionMatrix.Ttr);

                return table;
            }
            return null;
        }
        /// <summary>
        /// Получить DataTable ТТР с учетом независимых поставщиков
        /// </summary>
        /// <param name="distributionMatrix"></param>
        /// <param name="icvsMatrix"></param>
        /// <returns></returns>
        public DataTable GetTable(DistributionMatrix distributionMatrix, ICVsMatrix icvsMatrix)
        {
            if (icvsMatrix.Count != 0 && distributionMatrix != null)
            {
                var table = GetTable(distributionMatrix.Ttr);

                return table;
            }
            return null;
        }
    }
}
