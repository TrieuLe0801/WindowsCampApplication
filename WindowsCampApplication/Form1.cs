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
using SeleniumExtras.WaitHelpers;
using Flurl;
using System.Collections.ObjectModel;
using NodaTime.TimeZones;
using NodaTime;
using TimeZoneConverter;
using OpenQA.Selenium.Interactions;

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
        public static int PROCESSING = 0;
        public static int TAB = 0;
        public static string CHROMEDRIVER_PATH = Path.Combine(Directory.GetParent(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName,
                @"WindowsCampApplication");
        public static Object _lock = new Object();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static List<CountryInfo> countryCodeList = new List<CountryInfo>();
        public webCampingWindows()
        {
            String[] sub_array;
            InitializeComponent();

            // Get time zone code initial
            var countryCodeFilePath = Path.Combine(CHROMEDRIVER_PATH, "timezoneCode.txt");
            using (StreamReader reader = new StreamReader(countryCodeFilePath))
            {
                var content = reader.ReadToEnd();
                sub_array = content.Split('\n');
            }
            foreach(string zone in sub_array)
            {
                CountryInfo sub_tz = new CountryInfo();
                String[] info = zone.Split('|');
                sub_tz.CountryCode = info[0];
                sub_tz.CountryName = Regex.Replace(info[1], @"\t|\n|\r", ""); 
                countryCodeList.Add(sub_tz);
            }
        }

        private async void loadFileBtn_Click(object sender, EventArgs e)
        {
            if (PROCESSING == 1)
            {
                String message = "App is processing...";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                return;
            }
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
                    foreach (String s in sub_array)
                    {
                        OrderInfo sub_order = new OrderInfo();
                        String[] info = s.Split('|');
                        sub_order.OrderLink = info[0];
                        sub_order.Size = info[1];
                        sub_order.Time = DateTime.SpecifyKind(Convert.ToDateTime(info[2],
                            System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat), DateTimeKind.Utc);
                        sub_order.Country = Regex.Replace(info[3], @"\t|\n|\r", "");
                        orderList.Add(sub_order);
                    }
                }
                orderInforTextBox.Text = fileContent + Environment.NewLine;
            }
        }

        private async void campBtn_Click(object sender, EventArgs e)
        {
            if(PROCESSING == 1)
            {
                String message = "App is processing...";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                return;
            }
            PROCESSING = 1;
            if (tokenSource.Token.IsCancellationRequested)
            {
                tokenSource.Dispose();
                tokenSource = new CancellationTokenSource();
            }
            //Get tab will be launched
            if (tabBox.Text.Equals(""))
            {
                String message = "Please insert number of tab";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
                return;
            }
            if (!tabBox.Text.All(c => Char.IsNumber(c)))
            {
                String message = "Tab should be number";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
                return;
            }
            TAB = Int32.Parse(tabBox.Text);
            if (TAB >= 5 || TAB <= 0)
            {
                //alert box
                String message = "Number of tab should be over 0 and under 5";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
                return;
            }

            if (orderList.Count == 0)
            {
                String message = "Need add order";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
            }
            else
            {
                List<string> ordered;
                // Wait loop
                while (orderList.Count > 0)
                {
                    //start thread
                    var t = Task.Run(() => Process(), tokenSource.Token);
                    await t;
                    Console.WriteLine($"Order list count : {orderList.Count}");
                    // Check when stop threads
                    if (tokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                }
                String message = "Finsh Process";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Information);
                PROCESSING = 0;
            }
        }

        // Load and get order
        public string  LoadDriver(OrderInfo orderInfo)
        {
            string result = "";

            var chromeDriverService = ChromeDriverService.CreateDefaultService(CHROMEDRIVER_PATH);
            chromeDriverService.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();

            // set up agent
            Random rand = new Random();
            int agent = rand.Next(0, userAgent.Length);
            options.AddArgument("--user-agent=" + userAgent[agent]);
            options.AddArguments("--disable-gpu");
            //options.AddArguments("--window-size=1920,1080");
            options.AddArguments("--disable-gpu");
            options.AddArguments("--disable-extensions");
            options.AddUserProfilePreference("disable-popup-blocking", "true");
            options.AddArguments("--proxy-server='direct://'");
            options.AddArguments("--proxy-bypass-list=*");
            //options.AddArguments("--start-maximized");
            options.AddArguments("--no-sandbox");
            options.AddArguments("--silent");
            options.AddArguments("--disable-web-security");
            options.AddArguments("--log-level=3");
            options.AddArgument("--test-type");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArguments("--allow-running-insecure-content");
            if (HEADLESS == 1)
            {
                options.AddArguments("--incognito", "--headless", "--window-size=1280,1024", "--start-maximized");
            }
            else
            {
                options.AddArguments("--incognito");
            }

            // set driver
            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            driver.Navigate().GoToUrl("https://www.nike.com/");
            Console.WriteLine("Loaded NIKE page");
            Thread.Sleep(15000);

            try
            {
                IWebElement closePanel = driver.FindElement(By.XPath("//button[@class='pre-modal-btn-close']"));
                if(closePanel.Displayed)
                    closePanel.Click();
                    Console.WriteLine("Close the alert");
            }
            catch (NoSuchElementException ex)
            {

            }

            // get location
            driver.FindElement(By.XPath("//a[@class='fs10-nav-sm nav-color-white country-pin']")).Click();
            Thread.Sleep(2000);

            IWebElement alertLocation = null;
            try
            {
                alertLocation = wait.Until(SeleniumExtras.WaitHelpers.
                    ExpectedConditions.ElementExists(By.XPath("//div[@class='hf-geomismatch-btn-container']")));
            }
            catch(TimeoutException e)
            {

            }

            if (alertLocation.Displayed)
            {
                alertLocation.Click();
                Thread.Sleep(2000);
            }
            else
            {
                driver.FindElement(By.XPath("//p[@class='nav-bold' and contains(text(),'United States')]")).Click();
                Thread.Sleep(2000);
            }

            driver.Navigate().GoToUrl("https://www.nike.com/launch/");
            Console.WriteLine("Loaded NIKE Launch page");
            Thread.Sleep(10000);

            // Load item page
            driver.Navigate().GoToUrl(orderInfo.OrderLink);
            Console.WriteLine($"Load page {orderInfo.OrderLink}");
            Thread.Sleep(10000);

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
                result = $"Product at link {orderInfo.OrderLink} was SOLD OUT|FAILED";
                Console.WriteLine(result);
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
                result = $"This size is unavailable at link {orderInfo.OrderLink}|FAILED";
                Console.WriteLine(result);
                driver.Quit();
            }
            else
            {
                if (!driver.FindElement(
                   By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Enabled)
                {
                    //add result
                    result = $"This size is run out off at link {orderInfo.OrderLink}|FAILED";
                    Console.WriteLine(result);
                    driver.Quit();
                }
                else
                {
                    // Click button size
                    driver.FindElement(
                    By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Click();
                    Console.WriteLine("Choose button size "+ driver.FindElement(
                    By.XPath($"//button[contains(text(),'{orderInfo.Size}')]")).Text);
                    Thread.Sleep(2000);

                    // Click add to cart
                    driver.FindElement(By.XPath("//button[@data-qa='add-to-cart']")).Click();
                    Console.WriteLine("Click add to Cart "+ driver.FindElement(By.XPath("//button[@data-qa='add-to-cart']")).Text);
                    Thread.Sleep(3000);

                    // CLick the cart
                    if (HEADLESS == 1)
                    {
                        Thread.Sleep(2000);
                        var li_All = driver.FindElements(By.XPath("//ul[@class='right-nav prl7-sm ']/li"));
                        var li_cart = li_All.FirstOrDefault(i => i.GetAttribute("data-qa") == "top-nav-cart-link").
                            FindElement(By.XPath("//a[@class='hover-color-black text-color-grey bg-transparent prl3-sm " +
                            "pt2-sm pb2-sm m0-sm fs12-sm d-sm-b jewel-cart-container']"));
                        Console.WriteLine("Load the cart");
                        li_cart.Click();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        // Click to check the cart
                        driver.FindElement(
                           By.XPath("//a[" +
                           "@class='hover-color-black text-color-grey bg-transparent " +
                           "prl3-sm pt2-sm pb2-sm m0-sm fs12-sm d-sm-b jewel-cart-container']")).Click();
                        Console.WriteLine("Load the cart");
                        Thread.Sleep(2000);
                    }

                    // Click to checkout
                    try
                    {
                        driver.FindElement(
                           By.XPath("//button[@data-automation='guest-checkout-button']")).Click();
                        Thread.Sleep(2000);
                    }
                    catch (Exception e)
                    {
                        result = $"Failed order {orderInfo.OrderLink}|FAILED";
                        Console.WriteLine(result);
                        driver.Quit();
                        return result;
                    }
                    result = $"This product is ordered {orderInfo.OrderLink}|SUCCESSED";
                    Console.WriteLine(result);
                    driver.Quit();
                }
            }
            orderList.Remove(orderInfo);
            return result;
        }

        private void Process()
        {
            ParallelOptions parlOps = new ParallelOptions();
            var token = tokenSource.Token;
            parlOps.CancellationToken = token;
            parlOps.MaxDegreeOfParallelism = TAB;
            //var remove_order = new ConcurrentBag<string>();
            string result = "";
            try
            {
                try
                {
                    Parallel.ForEach
                    (orderList,
                       // Limit load page per time
                       parlOps, (order, state) =>
                       {
                           parlOps.CancellationToken.ThrowIfCancellationRequested();
                           DateTime present = ConvertLocalDateTime(order);
                           int compare_datetime = DateTime.Compare(present, order.Time);
                           if (compare_datetime >= 0) // change to datetime to select order to pickup and order
                            {

                               result = LoadDriver(order);
                                //remove_order.Add(order.OrderLink);
                                Console.WriteLine("Link: {0}, at Thread = {1}",
                                   order.OrderLink,
                                   Thread.CurrentThread.ManagedThreadId);
                                // Update result
                                resultTextBox.Invoke(new MethodInvoker(delegate
                               {
                                   resultTextBox.Text += result + Environment.NewLine;
                               }
                               ));
                               Thread.Sleep(10000);

                           }
                           else
                           {
                               result = $"Wait until {order.Time} of {order.Country}";
                               Console.WriteLine(result);
                                // Update result
                                resultTextBox.Invoke(new MethodInvoker(delegate
                               {
                                   resultTextBox.Text += result + Environment.NewLine;
                               }
                               ));
                               Thread.Sleep(10000);
                           }
                       }
                   );
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine(e);
                }
            }
            catch(AggregateException ex)
            {
                Console.WriteLine(ex);
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

        private async void stopBtn_Click(object sender, EventArgs e)
        {
            if(PROCESSING == 0)
            {
                String message = "Application is not Running";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Information);
            }
            else
            {
                String message = "Application is Running. If you stop, you have to wait runing drivers. Do you want to stop?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        tokenSource.Cancel();
                    });
                }
            }
        }

        private DateTime ConvertLocalDateTime(OrderInfo order)
        {

            // Get current time utc
            DateTime currentDateTime = DateTime.UtcNow;
            string countryName = order.Country;
            Console.WriteLine(countryName);
            string value = countryCodeList.Find(item => item.CountryName == countryName).CountryCode;

            var sourceZone = TimeZoneInfo.GetSystemTimeZones();

            // Get Location time
            var source = TzdbDateTimeZoneSource.Default;
            IEnumerable<string> windowsZoneIds = source.ZoneLocations
                .Where(x => x.CountryCode == value)
                .Select(tz => source.WindowsMapping.MapZones
                    .FirstOrDefault(x => x.TzdbIds.Contains(
                                         source.CanonicalIdMap.First(y => y.Value == tz.ZoneId).Key)))
                .Where(x => x != null)
                .Select(x => x.WindowsId)
                .Distinct();

            //Get zone destination
            // Egypt Irland do not in system
            try
            {
                TimeZoneInfo zoneDestination = TimeZoneInfo.FindSystemTimeZoneById(windowsZoneIds.ElementAt(0));
                // Convert time 
                DateTime newDateTimeZone = TimeZoneInfo.ConvertTimeFromUtc(currentDateTime, zoneDestination);
                Console.WriteLine($"{order.Country} time: " + newDateTimeZone);
                return newDateTimeZone;
            }
            catch (ArgumentOutOfRangeException e)
            {
                String message = "Cannot find out timezone, use current UTC Time?";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Information);
                return currentDateTime;
            }
        }
    }
}
