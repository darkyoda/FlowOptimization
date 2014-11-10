using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Text;
using FlowOptimization.Data;
using FlowOptimization.Data.Pipeline;

namespace FlowOptimization.Utilities.IO
{
    /// <summary>
    /// Экспорт информации о узлах и связях в текстовый файл в формате CSV
    /// </summary>
    class CSVExport
    {
        private readonly List<Node> _nodes;
        private readonly List<Pipe> _pipes;
        private readonly List<ICV> _icvs; 

        public CSVExport(List<Node> nodes, List<Pipe> pipes, List<ICV> icvs)
        {
            _nodes = nodes;
            _pipes = pipes;
            _icvs = icvs;
        }

        public string Export()
        {
            return Export(true);
        }

        public string Export(bool includeHeaderLine)
        {
            StringBuilder sb = new StringBuilder();
            //Получить свойства через Reflection
            IList<PropertyInfo> propertyInfos = typeof(Node).GetProperties();
            IList<PropertyInfo> propertyInfos2 = typeof(Pipe).GetProperties();
            IList<PropertyInfo> propertyInfos3 = typeof(ICV).GetProperties();

            if (includeHeaderLine)
            {
                sb.AppendLine("====Nodes====");
                //Добавить header
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    sb.Append(propertyInfo.Name).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            //add value for each property.
            foreach (var obj in _nodes)
            {               
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            if (includeHeaderLine)
            {
                sb.AppendLine("====Pipes====");
                 //Добавить header
                 foreach (PropertyInfo propertyInfo in propertyInfos2)
                 {
                     sb.Append(propertyInfo.Name).Append(",");
                 }
                 sb.Remove(sb.Length - 1, 1).AppendLine();
             }
            foreach (var obj in _pipes)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos2)
                {
                    sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(",");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }
            if (includeHeaderLine)
            {
                sb.AppendLine("====ICVs====");
                //Добавить header
                foreach (PropertyInfo propertyInfo in propertyInfos3)
                {
                    sb.Append(propertyInfo.Name).Append(",");
                }
                sb.Append("Node,").Append("Volume").AppendLine();
            }
            foreach (var icv in _icvs)
            {
                foreach (var obj in icv.GetICVNodes())
                {
                    foreach (PropertyInfo propertyInfo in propertyInfos3)
                    {
                        sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(icv, null))).Append(",");
                    }

                    sb.Append(MakeValueCsvFriendly(obj.ID)).Append(",").Append(MakeValueCsvFriendly(obj.Volume)).AppendLine();
                }               
            }

            return sb.ToString();
        }

        public void ExportToFile(string path)
        {
            File.WriteAllText(path, Export());
        }

        public byte[] ExportToBytes()
        {
            return Encoding.UTF8.GetBytes(Export());
        }

        private string MakeValueCsvFriendly(object value)
        {
            if (value == null) return "";
            if (value is Nullable && ((INullable)value).IsNull) return "";

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string output = value.ToString();

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;
        }
    }
}
