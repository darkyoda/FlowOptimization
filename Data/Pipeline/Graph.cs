using System.Collections.Generic;

namespace FlowOptimization.Data.Pipeline
{
    /// <summary>
    /// Общий класс для всех объектов (узлов, связей)
    /// Хранит List-ы этих объектов
    /// </summary>
    class Graph
    {
        private readonly List<Node> _nodes;  // Список узлов
        private readonly List<Pipe> _pipes;  // Список связей

        private int _nodeCounter;   // Число всех узлов
        private int _pipeCounter;   // Число всех связей

        public int NodesCount { get { return _nodes.Count; } }
        public int PipesCount { get { return _pipes.Count; } }

        public Graph()
        {
            _nodes = new List<Node>();
            _pipes = new List<Pipe>();

            _nodeCounter = 0;
            _pipeCounter = 0;
        }

        /// <summary>
        /// Добавить новый узел в список узлов
        /// </summary>
        /// <param name="x">Координата узла по X</param>
        /// <param name="y">Координата узла по Y</param>
        public void AddNode(int x, int y)
        {
            var node = new Node(x, y);
            _nodeCounter++;
            node.ID = _nodeCounter;
            _nodes.Add(node);
        }

        /// <summary>
        /// Добавить новый узел в список узлов
        /// </summary>
        /// <param name="node">Объект класса Node</param>
        public void AddNode(Node node)
        {
            _nodeCounter = node.ID;
            _nodes.Add(node);
        }

        /// <summary>
        /// Получить список узлов
        /// </summary>
        /// <returns>Возвращает List узлов</returns>
        public List<Node> GetNodes()
        {
            return _nodes;
        }

        /// <summary>
        /// Удалить узел
        /// </summary>
        /// <param name="node">Узел для удаления</param>
        public void DeleteNode(Node node)
        {
            // Удаляем информацию о всех связях узла
            foreach (var pipe in node.ConnectedPipes)
                DeletePipe(pipe);
            
            // Удаляем информацию о узле во всех подсоединенных к нему узлах
            foreach (var connectedNode in node.ConnectedNodes)            
                connectedNode.ConnectedNodes.Remove(node);            

            // Удаляем узел из списка узлов
            _nodes.Remove(node);
            // Уменьшаем ID каждого узла на 1
            for (int i = 0; i < _nodes.Count; i++)
                _nodes[i].ID = i + 1;
            _nodeCounter--;
        }

        /// <summary>
        /// Удалить связь
        /// </summary>
        /// <param name="pipe">Связь для удаления</param>
        public void DeletePipe(Pipe pipe)
        {
            _pipes.Remove(pipe);
            
            // Уменьшаем ID каждой связи на 1
            for (int i = 0; i < _pipes.Count; i++)
                _pipes[i].ID = i + 1;
            _pipeCounter--;
        }

        /// <summary>
        /// Добавить новую связь в список связей
        /// </summary>
        /// <param name="startNode">Объект начального узла</param>
        /// <param name="endNode">Объект конечного узла</param>
        public void AddPipe(Node startNode, Node endNode)
        {
            var pipe = new Pipe(startNode, endNode);
            _pipeCounter++;
            pipe.ID = _pipeCounter;
            //startNode.AddConnectedPipe(pipe);
           // endNode.AddConnectedPipe(pipe);
            _pipes.Add(pipe);   
        }

        /// <summary>
        /// Добавить новую связь в список связей
        /// </summary>
        /// <param name="pipe">Объект класса связь</param>
        public void AddPipe(Pipe pipe)
        {
            _pipeCounter = pipe.ID;
            //startNode.AddConnectedPipe(pipe);
            //endNode.AddConnectedPipe(pipe);
            _pipes.Add(pipe);   
            
        }

        /// <summary>
        /// Получить список связей
        /// </summary>
        /// <returns>Возвращает List связей</returns>
        public List<Pipe> GetPipes()
        {
            return _pipes;
        }

        /// <summary>
        /// Получить объект узла по его id
        /// </summary>
        /// <param name="id">ID узла</param>
        /// <returns>Возвращает объект класса Node, если такой существует</returns>
        public Node GetNodeByID(int id)
        {
            foreach (var node in _nodes)
            {
                if (node.ID == id)
                    return node;
            }
            return null;    // Возвращаем null, если такого узла нет
        }

        /// <summary>
        /// Обнуляет счетчики ID
        /// Использовать только, если все списки связей и узлов в последствии будут очищены
        /// </summary>
        public void ResetIDs()
        {
            _nodeCounter = 0;
            _pipeCounter = 0;
        }
    }
}
