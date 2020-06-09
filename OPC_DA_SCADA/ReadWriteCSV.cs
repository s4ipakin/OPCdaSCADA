using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace OPC_DA_SCADA
{
    class ReadWriteCSV
    {
        string path;
        public ReadWriteCSV(string path)
        {
            this.path = path;
        }

        public void WriteToCSV(string time, string value)
        {
            using (var w = new StreamWriter(path, true))
            {
                var line = string.Format("{0};{1}", time, value);
                w.WriteLine(line);
                w.Flush();
            }
        }

        public OrderedDictionary  ReadFromCSV ()
        {
            using (var reader = new StreamReader(path))
            {
                OrderedDictionary fileData = new OrderedDictionary();
                string[] values = { "" };

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    values = line.Split(';');
                    try
                    {
                        fileData.Add(values[0], values[1]);
                    }
                    catch (Exception ex) { }
                }               
                return fileData;
            }
        }
    }
}
