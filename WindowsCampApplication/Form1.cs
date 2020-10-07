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
using System.Threading;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace WindowsCampApplication
{
    public partial class webCampingWindows : Form
    {
        public static String[] userAgent = new String[] {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.87 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.92 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36",
//			"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.87 Safari/537.36",
//			"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36",
//			"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36",
//			"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.92 Safari/537.36",
//			"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36"
			};

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
                        sub_order.Country = Regex.Replace(info[3], @"\t|\n|\r", "");
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
            LoadDriver(orderList[0]);
        }

        // Load and get order
        public void LoadDriver(OrderInfo orderInfo)
        {

            ChromeOptions options = new ChromeOptions();

            // set up agent
            Random rand = new Random();
            int agent = rand.Next(0, userAgent.Length);
            options.AddArgument("--user-agent=" + userAgent[agent]);
            options.AddArguments("--disable-extensions");
            options.AddArguments("--incognito");
            
            // set driver
            IWebDriver driver = new ChromeDriver(Path.Combine(Directory.GetParent(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName, 
                @"WindowsCampApplication"),options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl(orderInfo.OrderLink);
            Console.WriteLine("Loaded page");
            Thread.Sleep(10000);
            bool isDisplay = false;
            try
            {
                isDisplay = driver.FindElement(
                    By.XPath("//button[@class='size-grid-dropdown size-grid-button']")).Displayed;
                Console.WriteLine("Load button size");
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            if (!isDisplay)
            {
                driver.FindElement(By.XPath(
                "//button[@class='locale-button u-full-height p0-sm d-sm-b d-md-ib ncss-btn-transparent']")).Click();
                Thread.Sleep(5000);

                driver.FindElement(By.XPath(
                    "//div[@class='ncss-container p6-sm p12-md u-full-width u-full-height']"));
                Thread.Sleep(2000);
                Console.WriteLine("Load select location");

                driver.FindElement(By.XPath(
                    $"//span[contains(text(), '{orderInfo.Country}')]")).Click();
                Thread.Sleep(2000);
                Console.WriteLine("Load click location");

                driver.Navigate().GoToUrl(orderInfo.OrderLink);
                Thread.Sleep(2000);
                Console.WriteLine("Load page");
                driver.FindElement(
                    By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Click();
                Console.WriteLine("Load button size");
                Thread.Sleep(2000);

                driver.FindElement(By.XPath("//button[@class='ncss-btn-primary-dark btn-lg']")).Click();
                Thread.Sleep(2000);
            }
            //driver.FindElement(By.XPath(
            //    "//button[@class='locale-button u-full-height p0-sm d-sm-b d-md-ib ncss-btn-transparent']")).Click();
            //Thread.Sleep(5000);

            //driver.FindElement(By.XPath(
            //    "//div[@class='ncss-container p6-sm p12-md u-full-width u-full-height']"));
            //Thread.Sleep(2000);
            //Console.WriteLine("Load select location");

            //driver.FindElement(By.XPath(
            //    $"//span[contains(text(), '{orderInfo.Country}')]")).Click();
            //Thread.Sleep(2000);
            //Console.WriteLine("Load click location");

            //driver.Navigate().GoToUrl(orderInfo.OrderLink);
            //Thread.Sleep(2000);

            //Console.WriteLine("Load page"); driver.FindElement(
            //    By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Click();
            //Console.WriteLine("Load button size");
            //Thread.Sleep(2000);

            //driver.FindElement(By.XPath("//button[@class='ncss-btn-primary-dark btn-lg']")).Click();
            //Thread.Sleep(2000);
        }
    }
}
