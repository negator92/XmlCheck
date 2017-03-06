using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

public class Tag
{
    public static int PayCaseCounter;
    public static int PayCaseNumber;
    //public static string[] array;

    public static void CallPrg(string[] args)
    {
        //Checking for arguments
        if (args.Length == 0)
            Console.WriteLine("Использование: XmlCheck.exe file.xml");
        else
        {
            foreach (string arg in args)
            {
                string[] array = (args.SelectMany(arg1 => Directory.GetFiles(".", arg)).Distinct()).ToArray();
                for (int i = 0; i < array.Length; i++)
                    {
                        Console.WriteLine("Argument {0} link to {1} file.", arg, array[i]);
                    }
            }
        }
    }

    public static void ExitsingFile(string[] array)
    {
        foreach (string xmlFile in array)
            try
            {
                if (!File.Exists(xmlFile))
                    throw new FileNotFoundException();
                else if (Path.GetExtension(xmlFile) != ".xml")
                    Console.WriteLine("{0} не XML файл", xmlFile);
                else
                    //Need to add RAM check, which initialise use XmlReader or XPath
                    CheckingXmlReader(xmlFile);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", xmlFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }

    public static void CheckingXmlReader(string xmlFile)
    {
        using (XmlReader reader = XmlReader.Create(xmlFile))
                    {
                        Console.WriteLine(xmlFile);
                        PayCaseCounter = 0;
                        //Open stream with "read" status
                        while (reader.Read())
                        {
                            //Regular expr
                            Regex regex = new Regex(@"^([А-ЯЁ]+)([\s\-]?[А-ЯЁ]+)*$");
                            switch (reader.Name)
                            {
                                case "НомерВыплатногоДела":
                                    PayCaseNumber = reader.ReadElementContentAsInt();
                                    PayCaseCounter++;
                                    break;
                                case "Фамилия":
                                    string secondName = reader.ReadElementContentAsString();
                                    if (regex.IsMatch(secondName))
                                        break;
                                    else
                                    {
                                        FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                                        StreamWriter writer = new StreamWriter(file);
                                        writer.Write("{0}\n", PayCaseNumber);
                                        writer.Close();
                                        Console.WriteLine(secondName);
                                        break;
                                    }
                                case "Имя":
                                    string firstName = reader.ReadElementContentAsString();
                                    if (regex.IsMatch(firstName))
                                        break;
                                    else
                                    {
                                        FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                                        StreamWriter writer = new StreamWriter(file);
                                        writer.Write("{0}\n", PayCaseNumber);
                                        writer.Close();
                                        Console.WriteLine(firstName);
                                        break;
                                    }
                                case "Отчество":
                                    string middleName = reader.ReadElementContentAsString();
                                    if (middleName.Length == 0 || regex.IsMatch(middleName))
                                        break;
                                    else
                                    {
                                        FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                                        StreamWriter writer = new StreamWriter(file);
                                        writer.Write("{0}\n", PayCaseNumber);
                                        writer.Close();
                                        Console.WriteLine(middleName);
                                        break;
                                    }
                            }
                        }
                        Console.WriteLine(PayCaseCounter);
                        reader.Close();
                    }
    }
}