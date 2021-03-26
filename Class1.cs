using System;
using System.Collections.Generic;
using System.Xml;

namespace LabWork1
{
    class Program
    {
        public static void Main()
        {
            var p = "";
            var q = "";
            var textToWorkWith = "";
            var File = new XmlDocument();
            File.Load("C:\\Users\\IVAN\\Desktop\\AlgLab_1\\test1.xml");
            var Root = File.DocumentElement;
            foreach (XmlNode Node in Root)
            {
                if (Node.Name == "p")
                    p = Node.InnerText;
                if (Node.Name == "q")
                    q = Node.InnerText;
                if (Node.Name == "text")
                    textToWorkWith = Node.InnerText;
            }
            var RSA = new RSA(new BigInt(p), new BigInt(q), textToWorkWith);
            RSA.Test();
            Console.WriteLine(RSA.ReturnResultToLower());
        }
    }
}
