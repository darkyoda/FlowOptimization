
using System.Collections.Generic;
using System.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization.Matrix
{
    /// <summary>
    /// Матрица пересечений
    /// </summary>
    class IntersectionMatrix : Matrix
    {
        private int[][] _intersectionMatrix;
        public IntersectionMatrix(List<Node> nodes)
            : base (nodes)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            // Инициализируем матрицу пересечений
            _intersectionMatrix = InitializeMatrix(Nodes.Count, Nodes.Count);
            // Рассчитываем матрицу пересечений
            CalculateMatrix();
            // Преобразование в DataTable, используется метод базового класса
            DataTable table = GetTable(_intersectionMatrix);

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int[][] GetMatrix()
        {
            return _intersectionMatrix;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateMatrix()
        {
            foreach (var node in Nodes)
            {
                // Если существует связь
                foreach (var connectedNode in node.ConnectedNodes)
                {
                    // То добавляем матрицу на пересечении длину связи
                    _intersectionMatrix[node.ID - 1][connectedNode.ID - 1] = node.GetPipeLength(connectedNode.ID);
                }
            }

            // Отзеркаливаем для получения симметричной матрицы
            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes.Count; j++)
                {
                    if (_intersectionMatrix[i][j] != 0)
                        _intersectionMatrix[j][i] = _intersectionMatrix[i][j];
                }
            }
        }
    }
}
