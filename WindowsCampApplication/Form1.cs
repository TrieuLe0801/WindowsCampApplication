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
        public static List<TimezoneCode> timezoneCodeList = new List<TimezoneCode>();
        public webCampingWindows()
        {
            String[] sub_array;
            InitializeComponent();

            // Get time zone code initial
            var zoneCodeFilePath = Path.Combine(CHROMEDRIVER_PATH, "timezoneCode.txt");
            using (StreamReader reader = new StreamReader(zoneCodeFilePath))
            {
                var content = reader.ReadToEnd();
                sub_array = content.Split('\n');
            }
            foreach(string zone in sub_array)
            {
                TimezoneCode sub_tz = new TimezoneCode();
                String[] info = zone.Split('|');
                sub_tz.ZoneCode = info[0];
                sub_tz.ZoneCode = Regex.Replace(info[1], @"\t|\n|\r", ""); 
                timezoneCodeList.Add(sub_tz);
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
            //options.AddArguments("--no-sandbox");
            //options.AddArguments("--silent");
            //options.AddArguments("--disable-web-security");
            options.AddArguments("--disable-extensions");
            //options.AddArguments("--log-level=3");
            //options.AddArguments("--proxy-server='direct://'");
            //options.AddArguments("--proxy-bypass-list=*");
            //options.AddArgument("test-type");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArguments("--allow-running-insecure-content");
            if (HEADLESS == 1)
            {
                options.AddArguments("--incognito", "headless");
            }
            else
            {
                options.AddArguments("--incognito");
            }

            // set driver
            IWebDriver driver = new ChromeDriver(chromeDriverService, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            driver.Navigate().GoToUrl("https://www.nike.com/");
            Console.WriteLine("Loaded NIKE page");
            Thread.Sleep(15000);

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
                    Console.WriteLine("Choose button size");
                    Thread.Sleep(2000);

                    // Click add to cart
                    driver.FindElement(By.XPath("//button[@data-qa='add-to-cart']")).Click();
                    Console.WriteLine("Click add to Cart");
                    Thread.Sleep(2000);
                    
                    // CLick the cart
                    if (HEADLESS == 1)
                    {
                        // go back Nike home
                        driver.Navigate().GoToUrl("https://www.nike.com/");
                        Console.WriteLine("Back to NIKE page");
                        Thread.Sleep(15000);

                        // Check alert box location
                        driver.FindElement(By.XPath("//a[@class='fs10-nav-sm nav-color-white country-pin']")).Click();
                        Thread.Sleep(2000);
                        string aria_code = driver.FindElement(By.XPath($"//a[@class='hf-language-menu-item ncss-col-sm-12 ncss-col-md-4 ncss-col-lg-3' " +
                            $"and @title ='{orderInfo.Country}']")).GetAttribute("data-country");
                        Thread.Sleep(2000);

                        // Load the cart
                        var url_cart = Url.Combine("https://www.nike.com/", aria_code, "/en/cart/");
                        driver.Navigate().GoToUrl(url_cart);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        driver.FindElement(
                           By.XPath("//a[" +
                           "@class='hover-color-black text-color-grey bg-transparent " +
                           "prl3-sm pt2-sm pb2-sm m0-sm fs12-sm d-sm-b jewel-cart-container']")).Click();
                        Thread.Sleep(2000);
                    }
                    Console.WriteLine("Load the cart");
                    

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
                Parallel.ForEach
                (orderList,
                    // Limit load page per time
                    parlOps, (order,state) =>
                    {
                        DateTime present = ConvertLocalDateTime(order);
                        int compare_datetime = DateTime.Compare(present, order.Time);
                        if (compare_datetime>=0) // change to datetime to select order to pickup and order
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
                //Console.WriteLine($"Number of order: {orderList.Count}");
                //foreach (string link in remove_order)
                //{
                //    Console.WriteLine(link);
                //    orderList.RemoveAll(cc => cc.OrderLink.Equals(link));
                //    Console.WriteLine($"Remove order {link}");
                //}
            }
            catch(OperationCanceledException ex)
            {

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
            String message = "Application is Running, do you want to stop?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                await Task.Factory.StartNew(() =>
                {
                    tokenSource.Cancel();
                });

                PROCESSING = 0;
            }
            
        }

        private DateTime ConvertLocalDateTime(OrderInfo order)
        {
            var WindowsTimes = new Dictionary<string, string>()
                {
                    { "Africa/Bangui", "W. Central Africa Standard Time" },
                    { "Africa/Cairo", "Egypt Standard Time" },
                    { "Africa/Casablanca", "Morocco Standard Time" },
                    { "Africa/Harare", "South Africa Standard Time" },
                    { "Africa/Johannesburg", "South Africa Standard Time" },
                    { "Africa/Lagos", "W. Central Africa Standard Time" },
                    { "Africa/Monrovia", "Greenwich Standard Time" },
                    { "Africa/Nairobi", "E. Africa Standard Time" },
                    { "Africa/Windhoek", "Namibia Standard Time" },
                    { "America/Anchorage", "Alaskan Standard Time" },
                    { "America/Argentina/San_Juan", "Argentina Standard Time" },
                    { "America/Asuncion", "Paraguay Standard Time" },
                    { "America/Bahia", "Bahia Standard Time" },
                    { "America/Bogota", "SA Pacific Standard Time" },
                    { "America/Buenos_Aires", "Argentina Standard Time" },
                    { "America/Caracas", "Venezuela Standard Time" },
                    { "America/Cayenne", "SA Eastern Standard Time" },
                    { "America/Chicago", "Central Standard Time" },
                    { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
                    { "America/Cuiaba", "Central Brazilian Standard Time" },
                    { "America/Denver", "Mountain Standard Time" },
                    { "America/Fortaleza", "SA Eastern Standard Time" },
                    { "America/Godthab", "Greenland Standard Time" },
                    { "America/Guatemala", "Central America Standard Time" },
                    { "America/Halifax", "Atlantic Standard Time" },
                    { "America/Indianapolis", "US Eastern Standard Time" },
                    { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
                    { "America/La_Paz", "SA Western Standard Time" },
                    { "America/Los_Angeles", "Pacific Standard Time" },
                    { "America/Mexico_City", "Mexico Standard Time" },
                    { "America/Montevideo", "Montevideo Standard Time" },
                    { "America/New_York", "Eastern Standard Time" },
                    { "America/Noronha", "UTC-02" },
                    { "America/Phoenix", "US Mountain Standard Time" },
                    { "America/Regina", "Canada Central Standard Time" },
                    { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
                    { "America/Santiago", "Pacific SA Standard Time" },
                    { "America/Sao_Paulo", "E. South America Standard Time" },
                    { "America/St_Johns", "Newfoundland Standard Time" },
                    { "America/Tijuana", "Pacific Standard Time" },
                    { "Antarctica/McMurdo", "New Zealand Standard Time" },
                    { "Atlantic/South_Georgia", "UTC-02" },
                    { "Asia/Almaty", "Central Asia Standard Time" },
                    { "Asia/Amman", "Jordan Standard Time" },
                    { "Asia/Baghdad", "Arabic Standard Time" },
                    { "Asia/Baku", "Azerbaijan Standard Time" },
                    { "Asia/Bangkok", "SE Asia Standard Time" },
                    { "Asia/Beirut", "Middle East Standard Time" },
                    { "Asia/Calcutta", "India Standard Time" },
                    { "Asia/Colombo", "Sri Lanka Standard Time" },
                    { "Asia/Damascus", "Syria Standard Time" },
                    { "Asia/Dhaka", "Bangladesh Standard Time" },
                    { "Asia/Dubai", "Arabian Standard Time" },
                    { "Asia/Irkutsk", "North Asia East Standard Time" },
                    { "Asia/Jerusalem", "Israel Standard Time" },
                    { "Asia/Kabul", "Afghanistan Standard Time" },
                    { "Asia/Kamchatka", "Kamchatka Standard Time" },
                    { "Asia/Karachi", "Pakistan Standard Time" },
                    { "Asia/Katmandu", "Nepal Standard Time" },
                    { "Asia/Kolkata", "India Standard Time" },
                    { "Asia/Krasnoyarsk", "North Asia Standard Time" },
                    { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
                    { "Asia/Kuwait", "Arab Standard Time" },
                    { "Asia/Magadan", "Magadan Standard Time" },
                    { "Asia/Muscat", "Arabian Standard Time" },
                    { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
                    { "Asia/Oral", "West Asia Standard Time" },
                    { "Asia/Rangoon", "Myanmar Standard Time" },
                    { "Asia/Riyadh", "Arab Standard Time" },
                    { "Asia/Seoul", "Korea Standard Time" },
                    { "Asia/Shanghai", "China Standard Time" },
                    { "Asia/Singapore", "Singapore Standard Time" },
                    { "Asia/Taipei", "Taipei Standard Time" },
                    { "Asia/Tashkent", "West Asia Standard Time" },
                    { "Asia/Tbilisi", "Georgian Standard Time" },
                    { "Asia/Tehran", "Iran Standard Time" },
                    { "Asia/Tokyo", "Tokyo Standard Time" },
                    { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
                    { "Asia/Vladivostok", "Vladivostok Standard Time" },
                    { "Asia/Yakutsk", "Yakutsk Standard Time" },
                    { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
                    { "Asia/Yerevan", "Armenian Standard Time" },
                    { "Atlantic/Azores", "Azores Standard Time" },
                    { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
                    { "Atlantic/Reykjavik", "Greenwich Standard Time" },
                    { "Australia/Adelaide", "Cen. Australia Standard Time" },
                    { "Australia/Brisbane", "E. Australia Standard Time" },
                    { "Australia/Darwin", "AUS Central Standard Time" },
                    { "Australia/Hobart", "Tasmania Standard Time" },
                    { "Australia/Perth", "W. Australia Standard Time" },
                    { "Australia/Sydney", "AUS Eastern Standard Time" },
                    { "Etc/GMT", "UTC" },
                    { "Etc/GMT+11", "UTC-11" },
                    { "Etc/GMT+12", "Dateline Standard Time" },
                    { "Etc/GMT+2", "UTC-02" },
                    { "Etc/GMT-12", "UTC+12" },
                    { "Europe/Amsterdam", "W. Europe Standard Time" },
                    { "Europe/Athens", "GTB Standard Time" },
                    { "Europe/Belgrade", "Central Europe Standard Time" },
                    { "Europe/Berlin", "W. Europe Standard Time" },
                    { "Europe/Brussels", "Romance Standard Time" },
                    { "Europe/Budapest", "Central Europe Standard Time" },
                    { "Europe/Dublin", "GMT Standard Time" },
                    { "Europe/Helsinki", "FLE Standard Time" },
                    { "Europe/Istanbul", "GTB Standard Time" },
                    { "Europe/Kiev", "FLE Standard Time" },
                    { "Europe/London", "GMT Standard Time" },
                    { "Europe/Minsk", "E. Europe Standard Time" },
                    { "Europe/Moscow", "Russian Standard Time" },
                    { "Europe/Paris", "Romance Standard Time" },
                    { "Europe/Sarajevo", "Central European Standard Time" },
                    { "Europe/Warsaw", "Central European Standard Time" },
                    { "Indian/Mauritius", "Mauritius Standard Time" },
                    { "Pacific/Apia", "Samoa Standard Time" },
                    { "Pacific/Auckland", "New Zealand Standard Time" },
                    { "Pacific/Fiji", "Fiji Standard Time" },
                    { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
                    { "Pacific/Guam", "West Pacific Standard Time" },
                    { "Pacific/Honolulu", "Hawaiian Standard Time" },
                    { "Pacific/Pago_Pago", "UTC-11" },
                    { "Pacific/Port_Moresby", "West Pacific Standard Time" },
                    { "Pacific/Tongatapu", "Tonga Standard Time" }
                };
            // Get current time utc
            DateTime currentDateTime = DateTime.UtcNow;

            // Get Location time
            var location = TzdbDateTimeZoneSource.Default.ZoneLocations
                 .FirstOrDefault(l => l.CountryName.Contains(order.Country));

            // Get Zone
            string zoneId = location.ZoneId;

            // Get zone destination
            TimeZoneInfo zoneDestination = TimeZoneInfo.FindSystemTimeZoneById(WindowsTimes[zoneId]);

            // Convert time 
            DateTime newDateTimeZone = TimeZoneInfo.ConvertTimeFromUtc(currentDateTime, zoneDestination);
            Console.WriteLine(newDateTimeZone);
            return newDateTimeZone;
        }
    }
}
