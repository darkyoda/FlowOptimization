
namespace FlowOptimization.Data
{
    /// <summary>
    /// Узел, обрабатываемый тем или иным независимым поставщиком
    /// </summary>
    class ICVNode
    {
        private int _id;
        private int _volume;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public ICVNode(int nodeID, int nodeVolume)
        {
            _id = nodeID;
            _volume = nodeVolume;
        }
    }
}
