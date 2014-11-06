using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using FlowOptimization.Data.Pipeline;
using FlowOptimization.Matrix;

namespace FlowOptimization.UI.DataGridView
{
    /// <summary>
    /// Хранит информацию о всех DataTable
    /// Необходимы для дальнейшего связывания с dataGridView
    /// </summary>
    class DataTableView
    {
        private readonly IntersectionMatrix _intersectionMatrix; // Матрица пересечений
        private readonly List<Node> _nodes;
        private readonly ICVsMatrix _icvsMatrix;

        /// <summary>
        /// Матрица расстояний
        /// </summary>
        public DataTable DistanceMatrix { get; private set; }
        /// <summary>
        /// Матрица маршрутов
        /// </summary>
        public DataTable RoutesMatrix { get; private set; }
        /// <summary>
        /// Матрица распределения
        /// </summary>
        public DataTable DistributionMatrix { get; private set; }
        /// <summary>
        /// Матрица ТТР
        /// </summary>
        public DataTable TtrMatrix { get; private set; }
        /// <summary>
        /// Матрица распределения с учетом независимых поставщиков
        /// </summary>
        public DataTable IcvDistributionMatrix { get; private set; }
        /// <summary>
        /// Матрица ТТР с учетом независимых поставщиков
        /// </summary>
        public DataTable IcvTtrMatrix { get; private set; }
        public DataTable PassabilityMatrix { get; private set; }
        
        public DataTableView(IntersectionMatrix intersectionMatrix, List<Node> nodes, ICVsMatrix icvsMatrix)
        {
            _intersectionMatrix = intersectionMatrix;
            _nodes = nodes;
            _icvsMatrix = icvsMatrix;

            BindTable();
        }

        /// <summary>
        /// Привязка расчетов к таблицам
        /// </summary>
        private void BindTable()
        {
            var distanceMatrix = new DistanceMatrix(_nodes);
            var routesMatrix = new RoutesMatrix(_nodes);
            var distributionMatrix = new DistributionMatrix(_nodes);
            var ttrMatrix = new TTRMatrix(_nodes);
            
            
            DistanceMatrix = distanceMatrix.GetTable(_intersectionMatrix);
            RoutesMatrix = routesMatrix.GetTable(distanceMatrix);
            DistributionMatrix = distributionMatrix.GetTable(routesMatrix);
            TtrMatrix = ttrMatrix.GetTable(distributionMatrix);
            var passabilityMatrix = distributionMatrix.PassabilityMatrix;
            PassabilityMatrix = passabilityMatrix.GetTable();

            // Если матрицы независимых поставщиков не существует
            if (_icvsMatrix == null) return;
            IcvDistributionMatrix = distributionMatrix.GetTable(routesMatrix, _icvsMatrix);
            IcvTtrMatrix = ttrMatrix.GetTable(distributionMatrix, _icvsMatrix);
        }
    }
}
