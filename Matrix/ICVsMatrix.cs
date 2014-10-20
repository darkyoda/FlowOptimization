using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FlowOptimization.Data;

namespace FlowOptimization.Matrix
{
    class ICVsMatrix : Matrix
    {
        private List<ICV> _icvs;
        private int _icvCounter;
        private int[][] _icvMatrix;

        /// <summary>
        /// Возвращает количество независимых поставщиков
        /// </summary>
        public int Count
        {
            get { return _icvs.Count; }        
        }

        public ICVsMatrix(List<ICV> icvs)
        {
            _icvs = icvs;
            int rowCount = 0;
            _icvCounter = 0;
            foreach (var icv in icvs)
            {
                _icvCounter++;
                icv.ID = _icvCounter;
                rowCount += icv.GetICVNodes().Count;
                
            }
            // Инициализируем матрицу в которой количество столбцов соответствует количеству всех узлов, которые охватывают незаывисимые поставщики
            _icvMatrix = InitializeMatrix(rowCount, 4);
        }

        public override void InitializeTable(DataTable table, int[][] matrix)
        {
            table.Columns.Add("Входы", typeof(int));
            table.Columns.Add("Н.П.", typeof(int));
            table.Columns.Add("Узел", typeof(int));
            table.Columns.Add("Объем", typeof(int));
        }

        public DataTable GetTable()
        {
            BuildTable();
            var table = GetTable(_icvMatrix);
            // Удаляем столбец "Входы"
            table.Columns.RemoveAt(0);

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int[][] GetMatrix()
        {
            return _icvMatrix;
        }

        /// <summary>
        /// Вернуть количество независимых поставщиков
        /// </summary>
        /// <returns></returns>


        private void BuildTable()
        {
            int rowCounter = 0;
            foreach (var icv in _icvs)
            {
                foreach (var icvNode in icv.GetICVNodes())
                {
                    if (icvNode.Volume != 0)
                    {
                        _icvMatrix[rowCounter][0] = 0;
                        _icvMatrix[rowCounter][1] = icv.ID;
                        _icvMatrix[rowCounter][2] = icvNode.ID;
                        _icvMatrix[rowCounter][3] = icvNode.Volume;
                        rowCounter++;
                    }
                }             
            }
        }
    }
}
