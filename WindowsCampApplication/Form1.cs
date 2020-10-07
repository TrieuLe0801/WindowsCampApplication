using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using WindowsCampApplication.Model;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WindowsCampApplication
{
    public partial class webCampingWindows : Form
    {
        List<OrderInfo> orderList = new List<OrderInfo>();
        public webCampingWindows()
        {
            InitializeComponent();
        }


        private void loadFileBtn_Click(object sender, EventArgs e)
        {
            String[] sub_array;
            var filePath = string.Empty;
            var fileContent = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        sub_array = fileContent.Split('\n');
                        
                    }

                    // Add order to array
                    foreach(String s in sub_array)
                    {
                        OrderInfo sub_order = new OrderInfo();
                        String[] info = s.Split('|');
                        sub_order.OrderLink = info[0];
                        sub_order.Size = info[1];
                        sub_order.Time = Convert.ToDateTime(info[2],
                            System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                        orderList.Add(sub_order);
                    }
                    //foreach(OrderInfo o in orderList)
                    //{
                    //    Console.WriteLine(o.OrderLink);
                    //    Console.WriteLine(o.Size);
                    //    Console.WriteLine(o.Time);
                    //}
                }
                orderInforTextBox.Text += fileContent + Environment.NewLine;
            }
        }

        private void campBtn_Click(object sender, EventArgs e)
        {
            loadDriver(orderList[0].OrderLink);
        }

        public void loadDriver(string driverLink)
        {
            IWebDriver driver = new ChromeDriver(Path.Combine(Directory.GetParent(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, 
                @"WindowsCampApplication"));
            driver.Navigate().GoToUrl(driverLink);
            Console.WriteLine("Loaded page");
        }
    }
}
