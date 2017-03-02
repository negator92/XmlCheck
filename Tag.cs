using System;

public class Tag
{
    public static int PayCaseCounter;
    public static int PayCaseNumber;

    public static void CallPrg(string[] args)
    {
        //Checking for arguments
        if (args.Length == 0)
            Console.WriteLine("Использование: Check.exe file.xml");
        else if (args.Length == 1)
        {
            Console.WriteLine("1 arg");
        }
    }
}