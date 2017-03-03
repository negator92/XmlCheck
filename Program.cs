using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace XmlCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Tag.CallPrg(args);
            Tag.ExitsingFile(args);
        }
    }
}