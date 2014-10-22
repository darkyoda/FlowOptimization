using System.Collections.Generic;

namespace FlowOptimization.Data
{
    /// <summary>
    /// Независимый поставщик
    /// </summary>
    class ICV
    {
        private int _id;    // Номер независимого поставщика
        private string _name;  // Название независимого поставщика
        private List<IcvNode> _nodes; // Список узлов с которыми он работает

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ICV(string name, List<IcvNode> nodes)
        {
            _name = name;
            _nodes = nodes;
        }

        public ICV(int id, string name, List<IcvNode> nodes)
        {
            _id = id;
            _name = name;
            _nodes = nodes;
        }

        public List<IcvNode> GetICVNodes()
        {
            return _nodes;
        }
    }
}
