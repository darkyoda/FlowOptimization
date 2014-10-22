using System;
using System.Collections.Generic;
using FlowOptimization.Data;
using FlowOptimization.Data.Pipeline;
using Microsoft.VisualBasic.FileIO;

namespace FlowOptimization.Utilities.IO
{
    /// <summary>
    /// Импортирует List-ы Node и Pipe в текстовый файл в формате CSV
    /// </summary>
    class CSVImport
    {
        private readonly string _csvFilePath;   // Файловый путь

        public CSVImport(string filePath)
        {
            _csvFilePath = filePath;
        }

        /// <summary>
        /// Импортируем данные из файла в память
        /// </summary>
        /// <param name="objects">Объект в который будем передавать новые данные</param>
        /// <param name="icvs">Список независимых поставщиков</param>
        /// <returns></returns>
        public void Import(ref Graph objects, ref List<ICV> icvs)
        {
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(_csvFilePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    // Проверка на окончание импорта того или иного объекта
                    bool nodeImport = false;    
                    bool pipeImport = false;
                    bool icvImport = false;
                    
                    int icvID = 1;
                    string icvName = "";
                    var icvNodes = new List<IcvNode>();
 
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        if (fieldData[0] == "X")
                        {
                            nodeImport = true;
                        }
                        else if (fieldData[0] == "Length")
                        {
                            nodeImport = false;
                            pipeImport = true;
                        }
                        else if (fieldData[0] == "ID")
                        {
                            pipeImport = false;
                            icvImport = true;
                        }
                        else if (nodeImport)
                        {
                            var node = new Node {X = Convert.ToInt32(fieldData[0]), Y = Convert.ToInt32(fieldData[1])};
                            Node.NodesType nodesType;
                            Enum.TryParse(fieldData[2], out nodesType);
                            node.NodeType = nodesType;
                            node.Name = fieldData[3];
                            node.Volume = Convert.ToInt32(fieldData[4]);
                            node.ID = Convert.ToInt32(fieldData[6]);
                            node.Ttr = Convert.ToInt32(fieldData[5]);
                            objects.AddNode(node);
                        }
                        else if (pipeImport)
                        {
                            var startNode = objects.GetNodeByID(Convert.ToInt32(fieldData[2]));
                            var endNode = objects.GetNodeByID(Convert.ToInt32(fieldData[3]));
                            var pipe = new Pipe(startNode, endNode);
                            pipe.Length = Convert.ToInt32(fieldData[0]);
                            pipe.Name = fieldData[1];
                            pipe.ID = Convert.ToInt32(fieldData[4]);
                            pipe.StartNodeID = Convert.ToInt32(fieldData[2]);
                            pipe.EndNodeID = Convert.ToInt32(fieldData[3]);
                            objects.AddPipe(pipe);
                        }
                        else if (icvImport)
                        {
                            // Если перешли на другого независимого поставщика
                            if (icvID != Convert.ToInt32(fieldData[0]))
                            {
                                icvs.Add(new ICV(icvID, icvName, icvNodes));
                                icvNodes = new List<IcvNode>();
                                icvID = Convert.ToInt32(fieldData[0]);
                            }

                            int nodeID = Convert.ToInt32(fieldData[2]);
                            int nodeVolume = Convert.ToInt32(fieldData[3]);
                            icvNodes.Add(new IcvNode(nodeID, nodeVolume));
                            icvName = fieldData[1];
                        }
                    }
                    // Добавляем последнего независимого поставщика
                    if (icvs.Count > 1)
                        icvs.Add(new ICV(icvID, icvName, icvNodes));
                }
            }
            catch (Exception)
            {
            }
            //return objects;
        }
    }
}
