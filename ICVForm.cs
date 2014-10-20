using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FlowOptimization.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization
{
    partial class ICVForm : Form
    {
        private List<ICVNode> _icvNodes;    // Узлы обрабатываемые данным независимым поставщиком
        private static Objects _objects;
        public ICV Icv;                     // Независимый поставщик

        public ICVForm(Objects objects)
        {
            InitializeComponent();
            _icvNodes = new List<ICVNode>();
            _objects = objects;

            foreach (var node in _objects.GetNodes())
            {
                if (node.Volume != 0)
                    _icvNodes.Add(new ICVNode(node.ID, 0));
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
        private static bool IsNullVolume(ICVNode icvNode)
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
