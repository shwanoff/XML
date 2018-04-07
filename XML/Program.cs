using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XML
{
    class Program
    {
        static void Main(string[] args)
        {
            // Устанавливаем кодировку консоли.
            // Нужно только если при использовании англоязычной Windows
            // на консоль вместо кириллицы выводятся знаки вопроса (??? ????? ??????)
            Console.OutputEncoding = Encoding.Unicode;

            // Читаем Xml файл.
            ReadXmlFile("example.xml");

            // Ждем ввода пользователя.
            Console.ReadLine();

            // Создаем структуру данных.
            var catalog = new Catalog() // Корневой элемент
            {
                Phones = new List<Phone>() // Коллекция номеров телефонов.
                {
                    new Phone() {Name = "Саша", Number = 890953317, Remark = "Не бери трубку!", Important = false}, // Запись номера телефона.
                    new Phone() {Name = "Дима", Number = 890512309, Remark = "Босс", Important = false},
                    new Phone() {Name = "Рита", Number = 890198735, Remark = "Невероятная девчонка", Important = true}
                }
            };

            // Пишем в файл.
            WriteXmlFile("result.xml", catalog);

            // Сообщаем пользователю о завершении.
            Console.WriteLine("ОК");
            Console.ReadLine();
        }

        /// <summary>
        /// Сохранить данные в Xml файл.
        /// </summary>
        /// <param name="filename"> Путь к сохраняемому файлу. </param>
        /// <param name="catalog"> Сохраняемые данные. </param>
        private static void WriteXmlFile(string filename, Catalog catalog)
        {
            // Создаем новый Xml документ.
            var doc = new XmlDocument();

            // Создаем Xml заголовок.
            var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Добавляем заголовок перед корневым элементом.
            doc.AppendChild(xmlDeclaration);

            // Создаем Корневой элемент
            var root = doc.CreateElement("catalog");

            // Получаем все записи телефонной книги.
            foreach(var phone in catalog.Phones)
            {
                // Создаем элемент записи телефонной книги.
                var phoneNode = doc.CreateElement("phone");

                if (phone.Important)
                {
                    // Если установлен атрибут Важный в true,
                    // то создаем и добавляем атрибут к элементу записи телефонной книги.

                    // Создаем атрибут и нужным именем.
                    var attribute = doc.CreateAttribute("group");

                    // Устанавливаем содержимое атрибута.
                    attribute.InnerText = "important";

                    // Добавляем атрибут к элементу.
                    phoneNode.Attributes.Append(attribute);
                }

                // Создаем зависимые элементы.
                AddChildNode("name", phone.Name, phoneNode, doc);
                AddChildNode("number", phone.Number.ToString(), phoneNode, doc);
                AddChildNode("remark", phone.Remark, phoneNode, doc);

                // Добавляем запись телефонной книги в каталог.
                root.AppendChild(phoneNode);
            }

            // Добавляем новый корневой элемент в документ.
            doc.AppendChild(root);

            // Сохраняем документ.
            doc.Save(filename);
        }

        /// <summary>
        /// Прочитать Xml файл.
        /// </summary>
        /// <param name="filename"> Путь к Xml файлу. </param>
        private static void ReadXmlFile(string filename)
        {
            // Создаем экземпляр Xml документа.
            var doc = new XmlDocument();

            // Загружаем данные из файла.
            doc.Load(filename);

            // Получаем корневой элемент документа.
            var root = doc.DocumentElement;

            // Используем метод для рекурсивного обхода документа.
            PrintItem(root);
        }

        /// <summary>
        /// Метод для отображения содержимого xml элемента.
        /// </summary>
        /// <remarks>
        /// Получает элемент xml, отображает его имя, затем все атрибуты
        /// после этого переходит к зависимым элементам.
        /// Отображает зависимые элементы со смещением вправо от начала строки.
        /// </remarks>
        /// <param name="item"> Элемент Xml. </param>
        /// <param name="indent"> Количество отступов от начала строки. </param>
        private static void PrintItem(XmlElement item, int indent = 0)
        {
            // Выводим имя самого элемента.
            // new string('\t', indent) - создает строку состоящую из indent табов.
            // Это нужно для смещения вправо.
            // Пробел справа нужен чтобы атрибуты не прилипали к имени.
            Console.Write($"{new string('\t', indent)}{item.LocalName} ");

            // Если у элемента есть атрибуты, 
            // то выводим их поочередно, каждый в квадратных скобках.
            foreach(XmlAttribute attr in item.Attributes)
            {
                Console.Write($"[{attr.InnerText}]");
            }

            // Если у элемента есть зависимые элементы, то выводим.
            foreach(var child in item.ChildNodes)
            {
                if (child is XmlElement node)
                {
                    // Если зависимый элемент тоже элемент,
                    // то переходим на новую строку 
                    // и рекурсивно вызываем метод.
                    // Следующий элемент будет смещен на один отступ вправо.
                    Console.WriteLine();
                    PrintItem(node, indent + 1);
                }

                if(child is XmlText text)
                {
                    // Если зависимый элемент текст,
                    // то выводим его через тире.
                    Console.Write($"- {text.InnerText}");
                }
            }
        }

        /// <summary>
        /// Добавить зависимый элемент с текстом.
        /// </summary>
        /// <param name="childName"> Имя дочернего элемента. </param>
        /// <param name="childText"> Текст, который будет внутри дочернего элемента. </param>
        /// <param name="parentNode"> Родительский элемент. </param>
        /// <param name="doc"> Xml документ. </param>
        private static void AddChildNode(string childName, string childText, XmlElement parentNode, XmlDocument doc)
        {
            var child = doc.CreateElement(childName);
            child.InnerText = childText;
            parentNode.AppendChild(child);
        }
    }
}
