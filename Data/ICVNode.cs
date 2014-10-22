
namespace FlowOptimization.Data
{
    /// <summary>
    /// Узел, обрабатываемый тем или иным независимым поставщиком
    /// </summary>
    class IcvNode
    {
        /// <summary>
        /// Номер узла
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Объем для распределения на узле
        /// </summary>
        public int Volume { get; set; }

        public IcvNode(int nodeId, int nodeVolume)
        {
            ID = nodeId;
            Volume = nodeVolume;
        }
    }
}
