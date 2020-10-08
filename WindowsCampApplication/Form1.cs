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
using System.Collections.Concurrent;
using OpenQA.Selenium.Support.UI;

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

        public static List<OrderInfo> orderList = new List<OrderInfo>();
        public static int HEADLESS = 0;
        public static int TAB = 0;
        public static string CHROMEDRIVER_PATH = Path.Combine(Directory.GetParent(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName,
                @"WindowsCampApplication");

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
                }
                orderInforTextBox.Text = fileContent + Environment.NewLine;
            }
        }

        private void campBtn_Click(object sender, EventArgs e)
        {
            //Get tab will be launched
            if (tabBox.Text.Equals(""))
            {
                String message = "Please insert number of tab";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message",buttons,MessageBoxIcon.Warning);
                return;
            }
            if (!tabBox.Text.All(c=>Char.IsNumber(c)))
            {
                String message = "Tab should be number";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                return;
            }
            TAB = Int32.Parse(tabBox.Text);
            if (TAB >= 5||TAB <= 0)
            {
                //alert box
                String message = "Number of tab should be over 0 and under 5";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                return;
            }
            DateTime now = DateTime.Now;
            
            if(orderList.Count == 0)
            {
                String message = "Need add order";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
            }
            else
            {
                Parallel.ForEach(orderList,
                    // Limit load page per time
                    new ParallelOptions { MaxDegreeOfParallelism = TAB }, order =>
                    {
                        LoadDriver(order);
                        Console.WriteLine("Link: {0}, at Thread = {1}",
                            order.OrderLink,
                            Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(10000);
                    }
                );
            }
        }

        // Load and get order
        public void LoadDriver(OrderInfo orderInfo)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();

            // set up agent
            Random rand = new Random();
            int agent = rand.Next(0, userAgent.Length);
            options.AddArgument("--user-agent=" + userAgent[agent]);
            options.AddArguments("--disable-gpu");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--silent");
            options.AddArguments("--disable-web-security");
            options.AddArguments("--disable-extensions");
            options.AddArguments("--log-level=3");
            options.AddArguments("--proxy-server='direct://'");
            options.AddArguments("--proxy-bypass-list=*");
            options.AddArgument("test-type");
            options.AddArgument("--ignore-certificate-errors");
            if (HEADLESS == 1)
            {
                options.AddArguments("--incognito", "--headless");
            }
            else
            {
                options.AddArguments("--incognito");
            }

            // set driver
            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl(orderInfo.OrderLink);
            Console.WriteLine("Loaded page");
            Thread.Sleep(10000);
            string locate = "";
            try
            {
                locate = driver.FindElement(
                    By.XPath("//span[@class='d-sm-ib va-sm-m small text-color-secondary']")).Text;
                Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                driver.Quit();
            }
            if (!locate.Equals(orderInfo.Country))
            {
                driver.FindElement(By.XPath(
                "//button[@data-qa='locale-selector-button']")).Click();
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

            }

            // Check sold out
            bool soldOut = false;
            try
            {
                soldOut = driver.FindElement(By.XPath(
                    "//div[@class='ncss-btn-primary-dark btn-lg disabled d-sm-b d-lg-ib buyable-full-width' " +
                    "and contains(text(),'Sold Out')]")).Displayed;
                Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (soldOut)
            {
                // add result
                Console.WriteLine("Sold Out");
                driver.Quit();
            }

            // Check size available
            bool sizeAvailable = false;
            try
            {
                sizeAvailable = driver.FindElement(
                    By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Displayed;
                Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (sizeAvailable == false)
            {
                // add result
                Console.WriteLine("There are no size");
                driver.Quit();
            }
            else
            {
                if (!driver.FindElement(
                   By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Enabled)
                {
                    //add result
                    Console.WriteLine($"Size {orderInfo.Size} out of");
                    driver.Quit();
                }
                else
                {
                    // Click button size
                    driver.FindElement(
                    By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Click();
                    Console.WriteLine("Load button size");
                    Thread.Sleep(2000);

                    // Click add to cart
                    driver.FindElement(By.XPath("//button[@class='ncss-btn-primary-dark btn-lg']")).Click();
                    Thread.Sleep(2000);

                    // CLick the cart
                    driver.FindElement(
                           By.XPath("//a[@class='hover-color-black text-color-grey bg-transparent " +
                           "prl3-sm pt2-sm pb2-sm m0-sm fs12-sm d-sm-b jewel-cart-container']")).Click();
                    Thread.Sleep(2000);
                    
                    // Click to checkout
                    try
                    {
                        driver.FindElement(
                           By.XPath("//button[@data-automation='guest-checkout-button']")).Click();
                        Thread.Sleep(2000);
                    }catch(Exception e)
                    {
                        Console.WriteLine("There are no product in cart");
                    }
                    
                    driver.Quit();
                    Console.WriteLine($"Load finish {orderInfo.OrderLink}");
                }
            }
        }

        private void headlessCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (headlessCheckbox.Checked)
            {
                HEADLESS = 1;
            }
            else
            {
                HEADLESS = 0;
            }
        }
    }
}
