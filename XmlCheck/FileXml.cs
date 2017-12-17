#define DEMO
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace XmlCheck
{
    public class FileXml
    {
        public FileXml(string FileName)
        {
            this.FileName = FileName;
        }

        public static int payCaseCounter = 0;
        public static Regex regex = new Regex(@"^([А-ЯЁ]+)([\s\-]?[А-ЯЁ]+)*$", RegexOptions.Compiled);

        public int payCaseNumber;
        public string FileName { get; set; }

        public string Check()
        {
#if DEMO
            var watch = System.Diagnostics.Stopwatch.StartNew();
#endif
            using (XmlReader xmlReader = XmlReader.Create(FileName))
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.Name)
                    {
                        case "НомерВыплатногоДела":
                            payCaseNumber = xmlReader.ReadElementContentAsInt();
                            payCaseCounter++;
                            break;
                        case "Фамилия":
                            string secondName = xmlReader.ReadElementContentAsString();
                            if (regex.IsMatch(secondName))
                                break;
                            else
                            {
                                FileStream file = new FileStream(FileName + ".log", FileMode.Append);
                                StreamWriter writer = new StreamWriter(file);
                                writer.Write("{0}\n", payCaseNumber);
                                writer.Close();
                                Console.WriteLine(secondName);
                                break;
                            }
                        case "Имя":
                            string firstName = xmlReader.ReadElementContentAsString();
                            if (regex.IsMatch(firstName))
                                break;
                            else
                            {
                                FileStream file = new FileStream(FileName + ".log", FileMode.Append);
                                StreamWriter writer = new StreamWriter(file);
                                writer.Write("{0}\n", payCaseNumber);
                                writer.Close();
                                Console.WriteLine(firstName);
                                break;
                            }
                        case "Отчество":
                            string middleName = xmlReader.ReadElementContentAsString();
                            if (middleName.Length == 0 || regex.IsMatch(middleName))
                                break;
                            else
                            {
                                FileStream file = new FileStream(FileName + ".log", FileMode.Append);
                                StreamWriter writer = new StreamWriter(file);
                                writer.Write("{0}\n", payCaseNumber);
                                writer.Close();
                                Console.WriteLine(middleName);
                                break;
                            }
                    }
                }
                xmlReader.Close();
#if DEMO
                watch.Stop();
                Console.WriteLine("Checking {0} takes {1} miliseconds", FileName, watch.ElapsedMilliseconds);
#endif
            }
            return $"Проверено {payCaseCounter} дел в файле {FileName}.";
        }
    }
}
