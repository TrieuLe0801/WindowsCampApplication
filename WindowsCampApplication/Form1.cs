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
        //public static string INITIAL_PATH = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName;
            //Path.Combine(Directory.GetParent(
            //    Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName,
            //    @"WindowsCampApplication");
       
        public static Object _lock = new Object();
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static List<CountryInfo> countryCodeList = new List<CountryInfo>();
        public static Timer t = new Timer();
        public static string countryCode = @"AD|Andorra
AE|United Arab Emirates
AF|Afghanistan
AG|Antigua & Barbuda
AI|Anguilla
AL|Albania
AM|Armenia
AO|Angola
AQ|Antarctica
AR|Argentina
AS|Samoa (American)
AT|Austria
AU|Australia
AW|Aruba
AX|Aaland Islands
AZ|Azerbaijan
BA|Bosnia & Herzegovina
BB|Barbados
BD|Bangladesh
BE|Belgium
BF|Burkina Faso
BG|Bulgaria
BH|Bahrain
BI|Burundi
BJ|Benin
BL|St Barthelemy
BM|Bermuda
BN|Brunei
BO|Bolivia
BQ|Caribbean Netherlands
BR|Brazil
BS|Bahamas
BT|Bhutan
BW|Botswana
BY|Belarus
BZ|Belize
CA|Canada
CC|Cocos (Keeling) Islands
CD|Congo (Dem. Rep.)
CF|Central African Rep.
CG|Congo (Rep.)
CH|Switzerland
CI|Cote d'Ivoire
CK|Cook Islands
CL|Chile
CM|Cameroon
CN|China
CO|Colombia
CR|Costa Rica
CU|Cuba
CV|Cape Verde
CW|Curacao
CX|Christmas Island
CY|Cyprus
CZ|Czech Republic
DE|Germany
DJ|Djibouti
DK|Denmark
DM|Dominica
DO|Dominican Republic
DZ|Algeria
EC|Ecuador
EE|Estonia
EG|Egypt
EH|Western Sahara
ER|Eritrea
ES|Spain
ET|Ethiopia
FI|Finland
FJ|Fiji
FK|Falkland Islands
FM|Micronesia
FO|Faroe Islands
FR|France
GA|Gabon
GB|Britain (UK)
GD|Grenada
GE|Georgia
GF|French Guiana
GG|Guernsey
GH|Ghana
GI|Gibraltar
GL|Greenland
GM|Gambia
GN|Guinea
GP|Guadeloupe
GQ|Equatorial Guinea
GR|Greece
GS|South Georgia & the South Sandwich Islands
GT|Guatemala
GU|Guam
GW|Guinea-Bissau
GY|Guyana
HK|Hong Kong
HN|Honduras
HR|Croatia
HT|Haiti
HU|Hungary
ID|Indonesia
IE|Ireland
IL|Israel
IM|Isle of Man
IN|India
IO|British Indian Ocean Territory
IQ|Iraq
IR|Iran
IS|Iceland
IT|Italy
JE|Jersey
JM|Jamaica
JO|Jordan
JP|Japan
KE|Kenya
KG|Kyrgyzstan
KH|Cambodia
KI|Kiribati
KM|Comoros
KN|St Kitts & Nevis
KP|Korea (North)
KR|Korea (South)
KW|Kuwait
KY|Cayman Islands
KZ|Kazakhstan
LA|Laos
LB|Lebanon
LC|St Lucia
LI|Liechtenstein
LK|Sri Lanka
LR|Liberia
LS|Lesotho
LT|Lithuania
LU|Luxembourg
LV|Latvia
LY|Libya
MA|Morocco
MC|Monaco
MD|Moldova
ME|Montenegro
MF|St Martin (French part)
MG|Madagascar
MH|Marshall Islands
MK|Macedonia
ML|Mali
MM|Myanmar (Burma)
MN|Mongolia
MO|Macau
MP|Northern Mariana Islands
MQ|Martinique
MR|Mauritania
MS|Montserrat
MT|Malta
MU|Mauritius
MV|Maldives
MW|Malawi
MX|Mexico
MY|Malaysia
MZ|Mozambique
NA|Namibia
NC|New Caledonia
NE|Niger
NF|Norfolk Island
NG|Nigeria
NI|Nicaragua
NL|Netherlands
NO|Norway
NP|Nepal
NR|Nauru
NU|Niue
NZ|New Zealand
OM|Oman
PA|Panama
PE|Peru
PF|French Polynesia
PG|Papua New Guinea
PH|Philippines
PK|Pakistan
PL|Poland
PM|St Pierre & Miquelon
PN|Pitcairn
PR|Puerto Rico
PS|Palestine
PT|Portugal
PW|Palau
PY|Paraguay
QA|Qatar
RE|Reunion
RO|Romania
RS|Serbia
RU|Russia
RW|Rwanda
SA|Saudi Arabia
SB|Solomon Islands
SC|Seychelles
SD|Sudan
SE|Sweden
SG|Singapore
SH|St Helena
SI|Slovenia
SJ|Svalbard & Jan Mayen
SK|Slovakia
SL|Sierra Leone
SM|San Marino
SN|Senegal
SO|Somalia
SR|Suriname
SS|South Sudan
ST|Sao Tome & Principe
SV|El Salvador
SX|St Maarten (Dutch part)
SY|Syria
SZ|Swaziland
TC|Turks & Caicos Is
TD|Chad
TF|French Southern & Antarctic Lands
TG|Togo
TH|Thailand
TJ|Tajikistan
TK|Tokelau
TL|East Timor
TM|Turkmenistan
TN|Tunisia
TO|Tonga
TR|Turkey
TT|Trinidad & Tobago
TV|Tuvalu
TW|Taiwan
TZ|Tanzania
UA|Ukraine
UG|Uganda
UM|US minor outlying islands
US|United States
UY|Uruguay
UZ|Uzbekistan
VA|Vatican City
VC|St Vincent
VE|Venezuela
VG|Virgin Islands (UK)
VI|Virgin Islands (US)
VN|Vietnam
VU|Vanuatu
WF|Wallis & Futuna
WS|Samoa (western)
YE|Yemen
YT|Mayotte
ZA|South Africa
ZM|Zambia
ZW|Zimbabwe";
        public webCampingWindows()
        {
            String[] sub_array;

            //timer
            StartTimer();
 
            InitializeComponent();

            // Get time zone code initial
            //var countryCodeFilePath = Path.Combine(INITIAL_PATH, "timezoneCode.txt");
            //using (StreamReader reader = new StreamReader(countryCodeFilePath))
            //{
            //    var content = reader.ReadToEnd();
            //    sub_array = content.Split('\n');
            //}
            sub_array = countryCode.Split('\n');
            foreach (string zone in sub_array)
            {
                Console.WriteLine(zone);
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
            orderList = new List<OrderInfo>();
            orderInforTextBox.Text = "";
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

                    String message = $"App will remove orders which do not have important attributes (link, time, country, size) or lack of attributes.";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, "Start message", buttons, MessageBoxIcon.Information);

                    // Add order to array
                    foreach (String s in sub_array)
                    {
                        if (!String.IsNullOrEmpty(s) && !s.Equals("\r") && !s.Equals("\n") && !s.Equals("\t") && !s.Equals(""))
                        {
                            OrderInfo sub_order = new OrderInfo();
                            String[] info = s.Split('|');
                            if(info.Length < 21 || 
                                String.IsNullOrWhiteSpace(info[0]) || 
                                String.IsNullOrWhiteSpace(info[1]) || 
                                String.IsNullOrWhiteSpace(info[2]) ||
                                String.IsNullOrWhiteSpace(info[3]))
                            {
                                Console.WriteLine("This elmement does not have enough attributes");
                                continue;
                            }
                            sub_order.OrderLink = info[0];
                            sub_order.Size = info[1];
                            sub_order.Time = DateTime.SpecifyKind(Convert.ToDateTime(info[2],
                                System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat), DateTimeKind.Utc);
                            sub_order.Country = Regex.Replace(info[3], @"\t|\n|\r", "");
                            sub_order.FirstName = Regex.Replace(info[4], @"\t|\n|\r", "");
                            sub_order.LastName = Regex.Replace(info[5], @"\t|\n|\r", "");
                            sub_order.Address = Regex.Replace(info[6], @"\t|\n|\r", "");
                            sub_order.City = Regex.Replace(info[7], @"\t|\n|\r", "");
                            sub_order.StateCode = Regex.Replace(info[8], @"\t|\n|\r", "");
                            sub_order.PostalCode = Regex.Replace(info[9], @"\t|\n|\r", "");
                            sub_order.Email = @"" + Regex.Replace(info[10], @"\t|\n|\r", ""); ;
                            sub_order.Phone = Regex.Replace(info[11], @"\t|\n|\r", "");
                            sub_order.Card = Regex.Replace(info[12], @"\t|\n|\r", "");
                            sub_order.ExDate = Regex.Replace(info[13], @"\t|\n|\r", "");
                            sub_order.Security = Regex.Replace(info[14], @"\t|\n|\r", "");
                            sub_order.SecondFistName = Regex.Replace(info[15], @"\t|\n|\r", "");
                            sub_order.SecondLastName = Regex.Replace(info[16], @"\t|\n|\r", "");
                            sub_order.SecondAddress = Regex.Replace(info[17], @"\t|\n|\r", "");
                            sub_order.SecondCity = Regex.Replace(info[18], @"\t|\n|\r", "");
                            sub_order.SecondStateCode = Regex.Replace(info[19], @"\t|\n|\r", "");
                            sub_order.SecondPostalCode = Regex.Replace(info[20], @"\t|\n|\r", "");
                            orderList.Add(sub_order);

                            //test
                            //Console.WriteLine(sub_order.OrderLink);
                            //Console.WriteLine(sub_order.ExDate);

                            foreach (var i in info)
                            {
                                if (!String.IsNullOrEmpty(i) && !i.Equals("\r") && !i.Equals("\n") && !i.Equals("\t") && !i.Equals(""))
                                {
                                    orderInforTextBox.Text += i.ToString() + Environment.NewLine;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            orderInforTextBox.Text += Environment.NewLine;
                        }
                    }

                    //Check file is empty or not
                    if(orderList.Count == 0)
                    {
                        message = "List order is empty. Please add file again.";
                        buttons = MessageBoxButtons.OK;
                        MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        message = $"List order has {orderList.Count()} items.";
                        buttons = MessageBoxButtons.OK;
                        MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Information);
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

            var firefoxDriverService = FirefoxDriverService.CreateDefaultService();
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
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

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
            Thread.Sleep(5000);

            // Load item page
            //if(orderInfo.OrderLink.Equals(null)||orderInfo.OrderLink.Equals(""))
            //{
            //    result = "There are no link|FAILED";
            //    Console.WriteLine(result);
            //    driver.Quit();
            //    orderList.Remove(orderInfo);
            //    return result;
            //}

            driver.Navigate().GoToUrl(orderInfo.OrderLink);
            Console.WriteLine($"Load page {orderInfo.OrderLink}");
            Thread.Sleep(3000);

            // Check sold out
            bool soldOut = false;
            try
            {
                soldOut = driver.FindElement(By.XPath(
                    "//div[@class='ncss-btn-primary-dark btn-lg disabled d-sm-b d-lg-ib buyable-full-width' " +
                    "and contains(text(),'Sold Out')]")).Displayed;
                //Thread.Sleep(2000);
                if (soldOut)
                {
                    // add result
                    result = $"Product at link {orderInfo.OrderLink} was SOLD OUT|FAILED";
                    Console.WriteLine(result);
                    driver.Quit();
                    orderList.Remove(orderInfo);
                    return result;
                }
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e);
            }

            // Check size available
            bool sizeAvailable = false;
            try
            {
                sizeAvailable = driver.FindElement(
                    By.XPath($"//button[text()='{orderInfo.Size}']")).Displayed;
                //Thread.Sleep(2000);
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
            //address.SendKeys(Keys.Enter);
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

            //action = new Actions(driver);
            //action.SendKeys(OpenQA.Selenium.Keys.End).Build().Perform();

            if (!String.IsNullOrWhiteSpace(order.SecondFistName) &&
               !String.IsNullOrWhiteSpace(order.SecondLastName) &&
               !String.IsNullOrWhiteSpace(order.SecondAddress) &&
               !String.IsNullOrWhiteSpace(order.SecondCity) &&
               !String.IsNullOrWhiteSpace(order.SecondStateCode) &&
               !String.IsNullOrWhiteSpace(order.SecondPostalCode))
            {
                //Click tick box
                driver.FindElement(By.XPath("//label[@for='billingAddress']")).Click();
                Thread.Sleep(2000);

                //add second first name
                var secondFirstName = driver.FindElement(By.Id("firstName"));
                secondFirstName.SendKeys(order.SecondFistName);
                secondFirstName.Submit();
                Thread.Sleep(2000);

                // add last name
                var secondLastName = driver.FindElement(By.Id("lastName"));
                secondLastName.SendKeys(order.SecondLastName);
                secondLastName.Submit();
                Thread.Sleep(2000);

                // add second address
                var secondAddress = driver.FindElement(By.Id("address1"));
                secondAddress.SendKeys(order.SecondAddress);
                secondAddress.Submit();
                //address.SendKeys(Keys.Enter);
                Thread.Sleep(2000);

                // add second city
                var secondCity = driver.FindElement(By.Id("city"));
                secondCity.SendKeys(order.SecondCity);
                secondCity.Submit();
                Thread.Sleep(2000);

                // add second state
                driver.FindElement(By.Id("state")).Click();
                Thread.Sleep(2000);
                var secondState = driver.FindElement(By.XPath($"//option[@value='{order.SecondStateCode}']"));
                secondState.Click();
                Thread.Sleep(2000);

                // add second postalcode
                var secondPostalCode = driver.FindElement(By.Id("postalCode"));
                secondPostalCode.SendKeys(order.SecondPostalCode);
                secondPostalCode.Submit();
                Thread.Sleep(2000);
            }

            try
            {
                var placeOrder = driver.FindElement(By.XPath("//button[@class='d-lg-ib fs14-sm ncss-brand " +
                "ncss-btn-accent pb2-lg pb3-sm prl5-sm " +
                "pt2-lg pt3-sm u-uppercase' and text() = 'Place Order']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", placeOrder);
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
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", placeOrder1);
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