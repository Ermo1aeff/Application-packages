using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace InsuranceValidationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPushed(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("insurance.xml");
            XmlNode elements = doc.DocumentElement;
            XmlNode ch1 = elements.ChildNodes[0].ChildNodes[1];
            XmlNode ch2 = elements.ChildNodes[0].ChildNodes[2];
            XmlNode ch3 = elements.ChildNodes[0].ChildNodes[3];

            NewClass s100 = Fill(ch1.ChildNodes[0]);
            NewClass s110 = Fill(ch1.ChildNodes[1]);
            NewClass s120 = Fill(ch1.ChildNodes[2]);
            NewClass s130 = Fill(ch1.ChildNodes[3]);
            NewClass s140 = Fill(ch1.ChildNodes[4]);
            NewClass s150 = Fill(ch1.ChildNodes[5]);

            int resultpension = 0;
            int resultmedicine = 0;
            int i = 0;
            while (ch2.ChildNodes[i] != null)
            {
                resultpension += int.Parse(ch2.ChildNodes[i].ChildNodes[6].InnerText);
                resultmedicine += int.Parse(ch2.ChildNodes[i].ChildNodes[7].InnerText);
                i++;
            }

            ShowText(z1t1, s110.c1, resultpension);
            ShowText(z1t2, s110.c4, resultmedicine);

            if (s110.c1 == resultpension && s110.c4 == resultmedicine) 
            {
                str110.Background = Brushes.Green;
                str110.FontWeight = FontWeights.Normal;
            }
            else
            {
                str110.Background = Brushes.Red;
                str110.FontWeight = FontWeights.Bold;
            }
            
            int resultz21 = 0;
            int resultz22 = 0;
            int resultz23 = 0;
            int resultz24 = 0;
            i = 0;
            while (ch3.ChildNodes[i] != null)
            {
                resultz21 += int.Parse(ch3.ChildNodes[i].ChildNodes[7].InnerText);
                resultz22 += int.Parse(ch3.ChildNodes[i].ChildNodes[8].InnerText);
                resultz23 += int.Parse(ch3.ChildNodes[i].ChildNodes[9].InnerText);
                resultz24 += int.Parse(ch3.ChildNodes[i].ChildNodes[10].InnerText);
                i++;
            };

            ShowText(z2t1, s120.c1, resultz21);
            ShowText(z2t2, s120.c2, resultz22);
            ShowText(z2t3, s120.c3, resultz23);
            ShowText(z2t4, s120.c4, resultz24);

            if (s120.c1 == resultz21 && s120.c2 == resultz22 && s120.c3 == resultz23 && s120.c4 == resultz24) { 
                str120.Background = Brushes.Green;
                str120.FontWeight = FontWeights.Normal;
            }
            else
            {
                str120.Background = Brushes.Red;
                str120.FontWeight = FontWeights.Bold;
            }

            int resultz31 = s130.c1 - s140.c1 + s100.c1;
            int resultz32 = s130.c2 - s140.c2 + s100.c2;
            int resultz33 = s130.c3 - s140.c3 + s100.c3;
            int resultz34 = s130.c4 - s140.c4 + s100.c4;

            ShowText(z3t1, s150.c1, resultz31);
            ShowText(z3t2, s150.c2, resultz32);
            ShowText(z3t3, s150.c3, resultz33);
            ShowText(z3t4, s150.c4, resultz34);

            bool bresultz31 = s150.c1 == s130.c1 - s140.c1 + s100.c1;
            bool bresultz32 = s150.c2 == s130.c2 - s140.c2 + s100.c2;
            bool bresultz33 = s150.c3 == s130.c3 - s140.c3 + s100.c3;
            bool bresultz34 = s150.c4 == s130.c4 - s140.c4 + s100.c4;

            if (bresultz31 && bresultz32 && bresultz33 && bresultz34) 
            {
                str150.Background = Brushes.Green;
                str150.FontWeight = FontWeights.Normal;
            }
            else
            {
                str150.Background = Brushes.Red;
                str150.FontWeight = FontWeights.Bold;
            }
        }

        private void ShowText(RichTextBox textBox, int strContent, int resultSum)
        {
            FlowDocument ObjF = new FlowDocument();
            Paragraph ObjP = new Paragraph();
            bool result = strContent == resultSum;
            ObjP.Inlines.Add(new Run(
                $"Раздел 1: {strContent}\n" +
                $"Расчёт: {resultSum}\n" +
                $"Результат совпадения: {result}"));
            ObjF.Blocks.Add(ObjP);
            textBox.Document = ObjF;

            TextRange TRange = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd);

            string text = TRange.Text;
            string subText = result.ToString();
            TextSelection(text, subText, result, textBox);
        }

        public void TextSelection(string text, string subText, bool Result, RichTextBox rich)
        {
            int indText = text.IndexOf(subText);

            int subLenText = subText.Length;

            TextPointer textStart = rich.Document.ContentStart;

            //Поиск начала строки, т.к. длинна считывается вместе с тегами.
            while (textStart != null &&
                   textStart.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
            {
                textStart = textStart.GetNextContextPosition(LogicalDirection.Forward);
            }

            var indStart = rich.Document.ContentStart.GetOffsetToPosition(textStart);

            TextPointer subTextStart = rich.Document.ContentStart.GetPositionAtOffset(indStart + indText, LogicalDirection.Forward);
            TextPointer subTextEnd = rich.Document.ContentStart.GetPositionAtOffset(indStart + indText + subLenText, LogicalDirection.Forward);

            TextRange textRange = new TextRange(subTextStart, subTextEnd);

            if (Result)
                textRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            else
                textRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        }

        public NewClass Fill(XmlNode item)
        {
            NewClass newClass = new NewClass();
            foreach (XmlNode item1 in item)
            {
                switch (item1.Name)
                {
                    case "col3":
                        newClass.c1 = int.Parse(item1.InnerText);
                        break;
                    case "col4":
                        newClass.c2 = int.Parse(item1.InnerText);
                        break;
                    case "col5":
                        newClass.c3 = int.Parse(item1.InnerText);
                        break;
                    case "col6":
                        newClass.c4 = int.Parse(item1.InnerText);
                        break;
                }
            }
            return newClass;
        }
    }

    public class NewClass
    {
        public int c1 { get; set; }
        public int c2 { get; set; }
        public int c3 { get; set; }
        public int c4 { get; set; }
    }
}
