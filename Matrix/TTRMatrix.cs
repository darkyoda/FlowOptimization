using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
    }
}
