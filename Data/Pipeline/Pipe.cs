
namespace FlowOptimization.Data.Pipeline
{
    /// <summary>
    /// Класс для работы со связями
    /// </summary>
    class Pipe : Element
    {
        public Node StartNode; // Входной узел
        public Node EndNode;   // Конечный узел
        private int _startNodeId;
        private int _endNodeId;
        // Длина связи
        public int Length { get; set; }
        // Имя связи
        public string Name { get; set; }
        // Входной узел связи
        public int StartNodeID
        {
            get { return StartNode.ID; }
            set { _startNodeId = value; }
        }
        // Выходной узел связи
        public int EndNodeID
        {
            get { return EndNode.ID; }
            set { _endNodeId = value; }
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Pipe()
        {        
        }

        /// <summary>
        /// Конструктор
        /// Добавляем в начальный и конечный узел информацию друг о друге, а также информацию о узле, который их соединяет
        /// </summary>
        /// <param name="startNode">Объект начального узла</param>
        /// <param name="endNode">Объект конечного узла</param>
        public Pipe(Node startNode, Node endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            // Добавляем информацию узлам друг о друге
            StartNode.ConnectedNodes.Add(endNode); 
            EndNode.ConnectedNodes.Add(startNode);
            // Добавляем информацию о связи между ними
            StartNode.AddPipe(this); 
            EndNode.AddPipe(this); 
            //_length = StartNode.ID;
        }
    }
}
