using System;
using System.IO;

public class Tag
{
    public static int PayCaseCounter;
    public static int PayCaseNumber;

    public static void CallPrg(string[] args)
    {
        //Checking for arguments
        if (args.Length == 0)
            Console.WriteLine("Использование: XmlCheck.exe file.xml");
        else if (args.Length == 1)
        {
            Console.WriteLine("1 arg");
        }
    }

    public static void ExitsingFile(string[] args)
    {
        foreach (string xmlFile in args)
            try
            {
                if (!File.Exists(xmlFile))
                    throw new FileNotFoundException();
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
}