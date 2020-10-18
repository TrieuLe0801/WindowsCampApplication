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
using Keys = OpenQA.Selenium.Keys;
using OpenQA.Selenium.Firefox;
using Timer = System.Windows.Forms.Timer;

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
        public static string INITIAL_PATH = Path.Combine(Directory.GetParent(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName,
                @"WindowsCampApplication");
        public static Object _lock = new Object();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static List<CountryInfo> countryCodeList = new List<CountryInfo>();
        public static Timer t = new Timer();
        public webCampingWindows()
        {
            String[] sub_array;

            //timer
            StartTimer();
 
            InitializeComponent();

            // Get time zone code initial
            var countryCodeFilePath = Path.Combine(INITIAL_PATH, "timezoneCode.txt");
            using (StreamReader reader = new StreamReader(countryCodeFilePath))
            {
                var content = reader.ReadToEnd();
                sub_array = content.Split('\n');
            }
            foreach (string zone in sub_array)
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
                        if (!s.Equals(null) && !s.Equals("\r") && !s.Equals("\n") && !s.Equals("\t") && !s.Equals(""))
                        {
                            OrderInfo sub_order = new OrderInfo();
                            String[] info = s.Split('|');
                            sub_order.OrderLink = info[0];
                            sub_order.Size = info[1];
                            sub_order.Time = DateTime.SpecifyKind(Convert.ToDateTime(info[2],
                                System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat), DateTimeKind.Utc);
                            sub_order.Country = Regex.Replace(info[3], @"\t|\n|\r", "");
                            sub_order.FirstName = info[4];
                            sub_order.LastName = info[5];
                            sub_order.Address = info[6];
                            sub_order.City = info[7];
                            sub_order.StateCode = info[8];
                            sub_order.PostalCode = info[9];
                            sub_order.Email = @"" + info[10];
                            sub_order.Phone = info[11];
                            sub_order.Card = info[12];
                            sub_order.ExDate = info[13];
                            sub_order.Security = Regex.Replace(info[14], @"\t|\n|\r", "");
                            orderList.Add(sub_order);

                            //test
                            Console.WriteLine(sub_order.OrderLink);
                            Console.WriteLine(sub_order.ExDate);

                            foreach (var i in info)
                            {
                                if(!s.Equals(null) && !s.Equals("\r") && !s.Equals("\n") && !s.Equals("\t") && !s.Equals(""))
                                {
                                    orderInforTextBox.Text += i.ToString() + Environment.NewLine;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            orderInforTextBox.Text += Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
            }
        }

        private async void campBtn_Click(object sender, EventArgs e)
        {
            if (PROCESSING == 1)
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
                MessageBox.Show(message, "Tab alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
                return;
            }
            if (!tabBox.Text.All(c => Char.IsNumber(c)))
            {
                String message = "Tab should be number";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Tab alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
                return;
            }
            TAB = Int32.Parse(tabBox.Text);
            if (TAB >= 5 || TAB <= 0)
            {
                //alert box
                String message = "Number of tab should be over 0 and under 5";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Tab alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
                return;
            }

            if (orderList.Count == 0)
            {
                String message = "Need add order";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Order alert message", buttons, MessageBoxIcon.Warning);
                PROCESSING = 0;
            }
            else
            {
                String message = $"There are {orderList.Count} orders. App is starting now...";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Start message", buttons, MessageBoxIcon.Information);

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
                if (orderList.Count == 0)
                {
                    message = $"Finsh Process. There are {orderList.Count} orders available. Clear all orders information.";
                    orderInforTextBox.Text = "";
                }
                else
                {
                    message = $"Finsh Process. There are {orderList.Count} orders available.";
                }
                MessageBoxButtons finbuttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Finish message", finbuttons, MessageBoxIcon.Information);
                PROCESSING = 0;
               
            }
        }

        // Load and get order
        public string LoadDriver(OrderInfo orderInfo)
        {
            string result = "";

            var firefoxDriverService = FirefoxDriverService.CreateDefaultService(INITIAL_PATH);
            firefoxDriverService.HideCommandPromptWindow = true;

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("browser.privatebrowsing.autostart", true);

            FirefoxOptions options = new FirefoxOptions();

            // set up agent
            Random rand = new Random();
            int agent = rand.Next(0, userAgent.Length);
            options.Profile = profile;
            options.AddArgument("--user-agent=" + userAgent[agent]);
            options.AddArguments("--disable-gpu");
            options.AddArguments("--window-size=1280,1024");
            options.AddArguments("--disable-extensions");
            //options.AddUserProfilePreference("disable-popup-blocking", "true");
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
                options.AddArguments("-private-window", "--headless"
                                     //"--window-size=1280,1024"
                                     //"--start-maximized"
                                     );
            }
            else
            {
                options.AddArguments("-private-window");
            }

            // set driver
            IWebDriver driver = new FirefoxDriver(firefoxDriverService, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            driver.Navigate().GoToUrl("https://www.nike.com/");
            Console.WriteLine("Loaded NIKE page");
            Thread.Sleep(8000);

            try
            {
                IWebElement closePanel = driver.FindElement(By.XPath("//button[@class='pre-modal-btn-close']"));
                if (closePanel.Displayed)
                    closePanel.Click();
                Console.WriteLine("Close the alert");
            }
            catch (NoSuchElementException ex)
            {

            }

            // get location
            // Handle pin element
            try
            {
                driver.FindElement(By.XPath("//a[@class='fs10-nav-sm nav-color-white country-pin']")).Click();
                Thread.Sleep(2000);
            }catch(ElementClickInterceptedException e)
            {
                result = $"cannot click pin this link {orderInfo.OrderLink}|FAILED";
                Console.WriteLine(result);
                driver.Quit();
                return result;
            }
            
            IWebElement alertLocation = null;
            try
            {
                alertLocation = wait.Until(SeleniumExtras.WaitHelpers.
                    ExpectedConditions.ElementExists(By.XPath("//div[@class='hf-geomismatch-btn-container']")));
            }
            catch (TimeoutException e)
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
            Thread.Sleep(8000);

            // Load item page
            driver.Navigate().GoToUrl(orderInfo.OrderLink);
            Console.WriteLine($"Load page {orderInfo.OrderLink}");
            Thread.Sleep(8000);

            // Check sold out
            bool soldOut = false;
            try
            {
                soldOut = driver.FindElement(By.XPath(
                    "//div[@class='ncss-btn-primary-dark btn-lg disabled d-sm-b d-lg-ib buyable-full-width' " +
                    "and contains(text(),'Sold Out')]")).Displayed;
                Thread.Sleep(2000);
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e);
            }
            if (soldOut)
            {
                // add result
                result = $"Product at link {orderInfo.OrderLink} was SOLD OUT|FAILED";
                Console.WriteLine(result);
                driver.Quit();
                orderList.Remove(orderInfo);
                return result;
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
                orderList.Remove(orderInfo);
                return result;
            }
            else
            {
                if (!driver.FindElement(
                   By.XPath($"//button[text()='{orderInfo.Size}']")).Enabled)
                {
                    //add result
                    result = $"This size is run out off at link {orderInfo.OrderLink}|FAILED";
                    Console.WriteLine(result);
                    driver.Quit();
                }
                else
                {
                    // Click button size
                    try
                    {
                        IWebElement sizebtn = driver.FindElement(By.XPath($"//button[text() = '{orderInfo.Size}']"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", sizebtn);
                        Actions action = new Actions(driver);
                        action.MoveToElement(sizebtn).Click().Perform();
                        Console.WriteLine("Choose button size " + driver.FindElement(
                        By.XPath($"//button[text() = '{orderInfo.Size}']")).Text);
                        Thread.Sleep(2000);

                        // Click add to cart
                        IWebElement addCartBtn = driver.FindElement(By.XPath("//div[@class='mt2-sm mb6-sm prl0-lg fs14-sm']"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addCartBtn);
                        action = new Actions(driver);
                        action.MoveToElement(addCartBtn).Click().Perform();
                        Console.WriteLine("Click add to Cart " + driver.FindElement(By.XPath("//button[@data-qa='add-to-cart']")).Text);
                        Thread.Sleep(2000);
                    }
                    catch(NoSuchElementException e)
                    {
                        result = $"Cannot choose size or add to cart {orderInfo.OrderLink}|FAILED";
                        Console.WriteLine(result);
                        driver.Quit();
                        return result;
                    }
                    
                    // Handle item load into cart
                    try
                    {
                        Thread.Sleep(2000);
                        wait.Until(SeleniumExtras.WaitHelpers.
                            ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='cart-item-modal-content-container " +
                            "ncss-container p6-sm bg-white']")));
                        Console.WriteLine("Add already");
                    }
                    catch (Exception e)
                    {
                        result = $"Cannot add item to cart {orderInfo.OrderLink}|FAILED";
                        Console.WriteLine(result);
                        driver.Quit();
                        return result;
                    }

                    // Click to check the cart
                    driver.FindElement(
                        By.XPath("//a[" +
                        "@class='hover-color-black text-color-grey bg-transparent " +
                        "prl3-sm pt2-sm pb2-sm m0-sm fs12-sm d-sm-b jewel-cart-container']")).Click();
                    Console.WriteLine("Load the cart");
                    Thread.Sleep(2000);

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

                    // load list button to find checkout button
                    var checkoutBtn = driver.FindElement(By.XPath("//div[@class='d-sm-h d-lg-tr']"));
                    checkoutBtn.FindElement(By.XPath("//button[text()='Guest Checkout']")).Click();
                    Console.WriteLine("Clicked Checkout");
                    Thread.Sleep(2000);

                    //Insert First name and last name
                    try
                    {
                        result = AutoFill(driver, orderInfo);
                    }catch(Exception e)
                    {
                        result = $"Cannot billing order {orderInfo.OrderLink}|FAILED";
                        driver.Quit();
                        return result;
                    }
                    
                    //result = $"This product is ordered {orderInfo.OrderLink}|SUCCESSED";
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
                               result = $"loading {order.OrderLink}...";
                               resultTextBox.Invoke(new MethodInvoker(delegate
                               {
                                   resultTextBox.Text += result + Environment.NewLine;
                               }
                              ));
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
            catch (AggregateException ex)
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
            if (PROCESSING == 0)
            {
                String message = "Application is not Running";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, "Not runing alert message", buttons, MessageBoxIcon.Information);
            }
            else
            {
                String message = "Application is Running. If you stop, you have to wait runing drivers. Do you want to stop?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, "Stop message", buttons, MessageBoxIcon.Question);
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

        private string AutoFill(IWebDriver driver, OrderInfo order)
        {
            string result = "";

            var firstName = driver.FindElement(By.Id("firstName"));
            firstName.SendKeys(order.FirstName);
            firstName.Submit();
            Console.WriteLine($"Add First Name: {order.FirstName}");
            Thread.Sleep(2000);

            var lastName = driver.FindElement(By.Id("lastName"));
            lastName.SendKeys(order.LastName);
            lastName.Submit();
            Console.WriteLine($"Add last Name: {order.LastName}");
            Thread.Sleep(2000);

            driver.FindElement(By.Id("addressSuggestionOptOut")).Click();
            Thread.Sleep(2000);

            var address = driver.FindElement(By.Id("address1"));
            address.SendKeys(order.Address);
            address.Submit();
            address.SendKeys(Keys.Enter);
            Console.WriteLine($"Add Address: {order.Address}");
            Thread.Sleep(2000);

            var city = driver.FindElement(By.Id("city"));
            city.SendKeys(order.City);
            city.Submit();
            Console.WriteLine($"Add City: {order.City}");
            Thread.Sleep(2000);

            driver.FindElement(By.Id("state")).Click();
            Thread.Sleep(2000);
            var state = driver.FindElement(By.XPath($"//option[@value='{order.StateCode}']"));
            state.Click();
            Console.WriteLine($"Choose state: {order.StateCode}");
            Thread.Sleep(2000);

            var postalCode = driver.FindElement(By.Id("postalCode"));
            postalCode.SendKeys(order.PostalCode);
            postalCode.Submit();
            Console.WriteLine($"Add PostalCode: {order.PostalCode}");
            Thread.Sleep(2000);

            var email = driver.FindElement(By.Id("email"));
            email.SendKeys(order.Email);
            email.Submit();
            Console.WriteLine($"Add Email: {order.Email}");
            Thread.Sleep(2000);

            var phoneNumber = driver.FindElement(By.Id("phoneNumber"));
            phoneNumber.SendKeys(order.Phone);
            phoneNumber.Submit();
            Console.WriteLine($"Add Number phone: {order.Phone}");
            Thread.Sleep(2000);

            IWebElement paymentBtn = driver.FindElement(By.XPath("//button[text() = 'Continue to Payment']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", paymentBtn);
            Actions action = new Actions(driver);
            //action.MoveToElement(paymentBtn).Click().Perform();
            paymentBtn.Click();
            Thread.Sleep(2000);
            Console.WriteLine($"Payment");
            Thread.Sleep(2000);

            action = new Actions(driver);
            action.SendKeys(OpenQA.Selenium.Keys.End).Build().Perform();

            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//iframe[@class='credit-card-iframe mt1 u-full-width prl2-sm']")));
            Thread.Sleep(2000);

            IWebElement creditCard = driver.FindElement(By.Id("creditCardNumber"));
            creditCard.SendKeys(order.Card);
            creditCard.Submit();
            Console.WriteLine($"Add Card: {order.Card}");
            Thread.Sleep(2000);

            var expirationDate = driver.FindElement(By.Id("expirationDate"));
            expirationDate.SendKeys(order.ExDate.Replace("/",""));
            expirationDate.Submit();
            Console.WriteLine($"Add Expiration Date: {order.ExDate}");
            Thread.Sleep(2000);

            var cvNumber = driver.FindElement(By.Id("cvNumber"));
            cvNumber.SendKeys(order.Security);
            cvNumber.Submit();
            Console.WriteLine($"Add cvNumber: {order.Security}");
            Thread.Sleep(2000);

            driver.SwitchTo().DefaultContent();
            Thread.Sleep(2000);

            try
            {
                var placeOrder = driver.FindElement(By.XPath("//button[@class='d-lg-ib fs14-sm ncss-brand " +
                "ncss-btn-accent pb2-lg pb3-sm prl5-sm " +
                "pt2-lg pt3-sm u-uppercase' and text() = 'Place Order']"));
                if (placeOrder.Enabled)
                {
                    placeOrder.Click();
                    result = $"Order successfull {order.OrderLink}|SUCCESS";
                }
                else
                {
                    result = $"Fail because fake order but stil successfull {order.OrderLink}|SUCCESS";
                }
                return result;
            }
            catch(Exception e)
            {

            }
            var placeOrder1 = driver.FindElement(By.XPath("//button[text()='Continue To Order Review']"));
            if (placeOrder1.Enabled)
            {
                placeOrder1.Click();
                result = $"Order successfull {order.OrderLink}|SUCCESS";
            }
            else
            {
                result = $"Fail because fake order but stil successfull {order.OrderLink}|SUCCESS";
            }
            return result;
        }

        private async void clear_btn_Click(object sender, EventArgs e)
        {
            if (PROCESSING == 1)
            {
                String message = "App is processing...";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
            }
            else
            {
                if (resultTextBox.Text.Equals(""))
                {
                    String message = "There are no any results";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, "Clear message", buttons, MessageBoxIcon.Information);
                }
                else
                {
                    String message = "Do you want to clear all results?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, "Clear message", buttons, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        resultTextBox.Text = "";
                    }
                }
            }
        }
        private void StartTimer()
        {
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            timerLb.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}