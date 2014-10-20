
namespace FlowOptimization.Data.Pipeline
{
    /// <summary>
    /// Базовый класс для всех элементов (узлов и связей)
    /// </summary>
    class Element
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

    }
}
