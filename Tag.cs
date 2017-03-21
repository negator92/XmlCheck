using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

public class Tag
{
    public static int payCaseCounter;
    public static int payCaseNumber;
    public static string[] array;

    public static void CallPrg(string[] args)
    {
        //Checking for arguments
        if (args.Length == 0)
        {
            Console.WriteLine("Использование: XmlCheck.exe file.xml");
        }
        else
        {
            foreach (string arg in args)
            {
                array = (args.SelectMany(arg1 => Directory.GetFiles(".", arg)).Distinct()).ToArray();
                //Show files to check
                /*for (int i = 0; i < array.Length; i++)
                    {
                        Console.WriteLine("Argument {0} link to {1} file.", arg, array[i]);
                    }*/
            }
        ExitsingFile(array);
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
                    getAvailableRam(xmlFile);
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

    public static void getAvailableRam(string xmlFile)
    {
        PerformanceCounter ramFree = new PerformanceCounter("Memory", "Available MBytes");
        FileInfo someFileInfo = new FileInfo(xmlFile);
        long fileByteSize = someFileInfo.Length;
        //Console.WriteLine("{0} file is {1}MB and free RAM is {2}MB.", xmlFile, someFileInfo.Length / 1024 / 1024, ramFree.NextValue());
        if (ramFree.NextValue() > xmlFile.Length / 1024 / 1024 * 2)
        {
            Console.WriteLine("Xpath. {0} file is {1}MB and free RAM is {2}MB.", xmlFile, someFileInfo.Length / 1024 / 1024, ramFree.NextValue());
            CheckingXPath(xmlFile);
        }
        else
        {
            Console.WriteLine("XmlReader. {0} file is {1}MB and free RAM is {2}MB.", xmlFile, someFileInfo.Length / 1024 / 1024, ramFree.NextValue());
            CheckingXmlReader(xmlFile);
        }
    }

    public static void CheckingXPath(string xmlFile)
    {
        List<int> numbers = new List<int>();
        //"загрузка" проверяемого файла
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(xmlFile);
        XmlElement xRoot = xDoc.DocumentElement;

        //условия для проверки регулярным выражением
        Regex regex = new Regex(@"^([А-ЯЁ]+)([\s\-]?[А-ЯЁ]+)*$");

        // выбор всех(повторений) тегов, вложенных по определенной структуре
        XmlNodeList NumPayId = xRoot.SelectNodes(".//НомерВыплатногоДела");
        //m.InnerText.Length == 0
        foreach (XmlNode PayCaseNumber in NumPayId)
        {
            numbers.Add(Int32.Parse(PayCaseNumber.InnerText));
        }
        Console.WriteLine(numbers.Count);

        try
        {
            XmlNodeList SurnameTag = xRoot.SelectNodes(".//Фамилия");
            payCaseCounter = 0;
            foreach (XmlNode SecondName in SurnameTag)
            //Console.WriteLine(n.InnerText);
            {
                if (regex.IsMatch(SecondName.InnerText))
                    payCaseCounter++;
                else
                {
                    FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(numbers[payCaseCounter] + "\n");
                    writer.Close();
                    //Console.WriteLine(numbers[c]);
                    Console.WriteLine(SecondName.InnerText);
                    payCaseCounter++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        try
        {
            XmlNodeList NameTag = xRoot.SelectNodes(".//Имя");
            payCaseCounter = 0;
            foreach (XmlNode FirstName in NameTag)
            //Console.WriteLine(n.InnerText);
            {
                if (regex.IsMatch(FirstName.InnerText))
                    payCaseCounter++;
                else
                {
                    FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(numbers[payCaseCounter] + "\n");
                    writer.Close();
                    //Console.WriteLine(numbers[c]);
                    Console.WriteLine(FirstName.InnerText);
                    payCaseCounter++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        try
        {
            XmlNodeList FnameTag = xRoot.SelectNodes(".//Отчество");
            payCaseCounter = 0;
            foreach (XmlNode MiddleName in FnameTag)
            //Console.WriteLine(n.InnerText);
            {
                if (MiddleName.InnerText.Length == 0 || regex.IsMatch(MiddleName.InnerText))
                    payCaseCounter++;
                else
                {
                    FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(numbers[payCaseCounter] + "\n");
                    writer.Close();
                    //Console.WriteLine(numbers[c]);
                    Console.WriteLine(MiddleName.InnerText);
                    payCaseCounter++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

    public static void CheckingXmlReader(string xmlFile)
    {
        using (XmlReader reader = XmlReader.Create(xmlFile))
                    {
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        Console.WriteLine(xmlFile);
                        payCaseCounter = 0;
                        //Open stream with "read" status
                        while (reader.Read())
                        {
                            //Regular expr
                            Regex regex = new Regex(@"^([А-ЯЁ]+)([\s\-]?[А-ЯЁ]+)*$");
                            switch (reader.Name)
                            {
                                case "НомерВыплатногоДела":
                                    payCaseNumber = reader.ReadElementContentAsInt();
                                    payCaseCounter++;
                                    break;
                                case "Фамилия":
                                    string secondName = reader.ReadElementContentAsString();
                                    if (regex.IsMatch(secondName))
                                        break;
                                    else
                                    {
                                        FileStream file = new FileStream(xmlFile + ".log", FileMode.Append);
                                        StreamWriter writer = new StreamWriter(file);
                                        writer.Write("{0}\n", payCaseNumber);
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
                                        writer.Write("{0}\n", payCaseNumber);
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
                                        writer.Write("{0}\n", payCaseNumber);
                                        writer.Close();
                                        Console.WriteLine(middleName);
                                        break;
                                    }
                            }
                        }
                        Console.WriteLine(payCaseCounter);
                        reader.Close();
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine("Checking {0} takes {1} miliseconds", xmlFile, elapsedMs);
                    }
    }
}