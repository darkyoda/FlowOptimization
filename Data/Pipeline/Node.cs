using System.Collections.Generic;
using System.Linq;

namespace FlowOptimization.Data.Pipeline
{
    /// <summary>
    /// Класс для работы с узлами
    /// </summary>
    class Node : Element
    {
        public readonly List<Pipe> ConnectedPipes = new List<Pipe>();  // Список связей узла

        /// <summary>
        /// Типы узлов
        /// Default - узел по умолчанию
        /// Enter - входной узел
        /// Exit - выходной узел
        /// </summary>
        public enum Type { Default, Enter, Exit }; 

        private Type _nodeType;
        public string TypeForTable; // Типы узлов на русском для ObjectListView
        public List<Node> ConnectedNodes = new List<Node>();

        /// <summary>
        /// Координаты узла по X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Координаты узла по Y
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Тип узла
        /// </summary>
        public Type NodeType
        {
            get
            { return _nodeType; }
            set
            {
                _nodeType = value;
                switch (value)
                {
                    case Type.Enter:
                    {
                        TypeForTable = "Входной";
                        break;
                    }
                    case Type.Exit:
                    {
                        TypeForTable = "Выходной";
                        break;
                    }
                    case Type.Default:
                    {
                        TypeForTable = "-";
                        break;
                    }
                }                  
            }
        }
        /// <summary>
        /// Название узла
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Объем узла
        /// </summary>
        public int Volume { get; set; }
        /// <summary>
        /// ТТР узла
        /// </summary>
        public int Ttr { get; set; }

        public Node()
        {
        }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Добавляем объект pipe (связь) в коллекцию pipes
        /// </summary>
        /// <param name="pipe">Объект класса pipe (связь)</param>
        public void AddConnectedPipe(Pipe pipe)
        {
            if (pipe != null)
            {
                ConnectedPipes.Add(pipe);
            }
        }

        public int GetPipeLength(Node endNode)
        {
            return (from pipe in ConnectedPipes where pipe.StartNode == this && pipe.EndNode == endNode select pipe.Length).FirstOrDefault();
        }

        public int GetPipeLength(int endNodeId)
        {
            return (from pipe in ConnectedPipes where pipe.StartNode.ID == this.ID && pipe.EndNode.ID == endNodeId select pipe.Length).FirstOrDefault();
        }
    }
}
