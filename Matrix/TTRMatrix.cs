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

        public DataTable GetTable(DistributionMatrix distributionMatrix)
        {
            if (distributionMatrix != null)
            {
                var table = GetTable(distributionMatrix.Ttr);

                return table;
            }
            return null;
        }

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
