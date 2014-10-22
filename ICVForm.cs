using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FlowOptimization.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization
{
    partial class ICVForm : Form
    {
        private List<IcvNode> _icvNodes;    // Узлы обрабатываемые данным независимым поставщиком
        private static Graph _objects;
        public ICV Icv;                     // Независимый поставщик

        public ICVForm(Graph objects)
        {
            InitializeComponent();
            _icvNodes = new List<IcvNode>();
            _objects = objects;

            foreach (var node in _objects.GetNodes())
            {
                if (node.Volume != 0)
                    _icvNodes.Add(new IcvNode(node.ID, 0));
            }

            objectListView1.SetObjects(_icvNodes);           
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            // Удаляем из списка узлы, которые не были внесены
            _icvNodes.RemoveAll(IsNullVolume);
            //_icvNodes.Exists()
            Icv = new ICV(textBox1.Text,_icvNodes);
            Close();
        }

        // Критерий для удаления узлов, объем которых равен нулю
        private static bool IsNullVolume(IcvNode icvNode)
        {
            if (icvNode.Volume == 0)
                return true;
            
            return false;
        }

     /*   private static bool IsStartNode(ICVNode icvNode)
        {
            if (_objects.GetNodes().Contains(new Node()))
        }*/
    }
}
