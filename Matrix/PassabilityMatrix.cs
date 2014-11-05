using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FlowOptimization.Matrix
{
    class PassabilityMatrix : Matrix
    {
        private readonly PathsMatrix _pathsMatrix;
        private int[][] _passabilityMatrix;

        public PassabilityMatrix(PathsMatrix pathsMatrix)
        {
            _pathsMatrix = pathsMatrix;
            
            _passabilityMatrix = InitializeMatrix(_pathsMatrix.GetMatrix()[0].Length, 1);
        }

        public void AddValue(int start, int end, int value)
        {
            int[] path = _pathsMatrix.GetPath(start, end);
            for (int i = 0; i < path.Length; i++)
            {
                int pathValue = path[i];
                int pathIndex = pathValue - 1;
                if (pathValue != 0)
                    _passabilityMatrix[pathIndex][0] += value;
            }
        }

        public override int[][] GetMatrix()
        {
            return _passabilityMatrix;
        }

        public DataTable GetTable()
        {
            var table = GetTable(_passabilityMatrix);
            return table;
        }
    }
}
