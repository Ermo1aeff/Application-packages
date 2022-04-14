using System;
using System.Xml;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("XMLData.xml");

            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                // обход всех узлов в корневом элементе
                foreach (XmlElement xnode in xRoot)
                {
                    // получаем атрибут name
                    XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                    Console.WriteLine(attr?.Value);

                    // обходим все дочерние узлы элемента user
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        // если узел - company
                        if (childnode.Name == "company")
                        {
                            Console.WriteLine($"Компания: {childnode.InnerText}");
                        }
                        // если узел age
                        if (childnode.Name == "age")
                        {
                            Console.WriteLine($"Возраст: {childnode.InnerText}");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

    }
}
