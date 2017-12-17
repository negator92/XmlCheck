using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XmlCheck
{
    class Program
    {
        public static string xmlHead = "<?xml version=\"1.0\" encoding=\"WINDOWS-1251\"?>";

        static void Main(string[] args)
        {
            List<string> filesNames = new List<string>();

            if (args.Length == 0)
                Console.WriteLine("Использование: XmlCheck.exe file.xml");

            foreach (string fileName in args)
            {
                if (!File.Exists(fileName))
                    Console.WriteLine($"Файл {fileName} не найден");
                else if (xmlHead != File.ReadLines(fileName).First())
                    Console.WriteLine($"{fileName} не XML файл");
                else
                    filesNames.Add(fileName);
            }

            FileXml[] filesArray = new FileXml[filesNames.Count];
            int i = 0;
            foreach (var fileName in filesNames)
            {
                filesArray[i] = new FileXml(fileName);
                i++;
            }

            foreach (FileXml fileXml in filesArray)
                fileXml.Check();
        }
    }
}
