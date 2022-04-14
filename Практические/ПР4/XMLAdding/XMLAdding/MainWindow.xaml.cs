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
using XMLAdding.Base;

namespace XMLAdding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            PlantGread.ItemsSource = SourceCore.MyBase.botanical.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("plant_catalog.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            Entities en = new Entities();

            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("PLANT");
                    //Listbox1.Items.Add(attr.Value);

                    botanical NewBotanical = new botanical();

                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "COMMON") //common
                        {
                            bool exit = true;

                            foreach (common Common in en.common)
                            {
                                if (Common.common_name == childnode.InnerText)
                                {
                                    NewBotanical.common_id = Common.common_id;
                                    exit = false;
                                }
                                if (!exit) break;
                            }

                            if (exit)
                            {
                                common NewCommon = new common();
                                NewCommon.common_name = childnode.InnerText;
                                SourceCore.MyBase.common.Add(NewCommon);
                            }
                        }

                        if (childnode.Name == "LIGHT") //light
                        {
                            bool exit = true;

                            foreach (light Light in en.light) 
                            { 
                                if (Light.light_name == childnode.InnerText)
                                {
                                    NewBotanical.light_id = Light.light_id;
                                    exit = false;
                                }
                                if (!exit) break;
                            }

                            if (exit)
                            {
                                light NewLight = new light();
                                NewLight.light_name = childnode.InnerText;
                                SourceCore.MyBase.light.Add(NewLight);
                            }
                        }

                        if (childnode.Name == "PRICE") //price
                        {
                            bool exit = true;

                            foreach (price Price in en.price) {
                                if (Price.price_name == childnode.InnerText)
                                {
                                    NewBotanical.price_id = Price.price_id;
                                    exit = false;
                                }
                                if (!exit) break;
                            }
                            if (exit)
                            {
                                price NewPrice = new price();
                                NewPrice.price_name = childnode.InnerText;
                                SourceCore.MyBase.price.Add(NewPrice);
                            }
                        }

                        if (childnode.Name == "BOTANICAL") //botanical
                        {
                            NewBotanical.botanical_name = childnode.InnerText;
                        }

                        if (childnode.Name == "ZONE") //botanical
                        {
                            NewBotanical.zone = childnode.InnerText;
                        }

                        if (childnode.Name == "AVAILABILITY") //botanical
                        {
                            NewBotanical.availability = int.Parse(childnode.InnerText);
                        }
                    }
                    SourceCore.MyBase.botanical.Add(NewBotanical);
                    SourceCore.MyBase.SaveChanges();
                }
                PlantGread.ItemsSource = SourceCore.MyBase.botanical.ToList();
            }
        }

        private void UpdateGrid_Click(object sender, RoutedEventArgs e)
        {
            PlantGread.ItemsSource = SourceCore.MyBase.botanical.ToList();
        }
    }
}
