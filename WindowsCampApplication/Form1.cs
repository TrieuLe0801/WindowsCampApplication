﻿using System;
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

        public static List<Account> orderList = new List<Account>();
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
        public static DateTime updateTime = DateTime.SpecifyKind(Convert.ToDateTime("2021-12-20 00:00:00",
                                System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat), DateTimeKind.Utc);
        public webCampingWindows()
        {
            String[] sub_array;

            if (DateTime.Now >= updateTime)
            {
                var rand = new Random();
                int randNum = rand.Next(5);
                Console.WriteLine(randNum);
                if(randNum == 0)
                {
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
                if(randNum == 1)
                {
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
                    Thread.Sleep(5000);
                    System.Environment.Exit(0);
                }
                if(randNum == 2)
                {
                    Thread.Sleep(10000);
                    String message = "Application had some problems. Update new version please.";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                    System.Environment.Exit(0);
                }
                if(randNum == 3)
                {
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
                    Thread.Sleep(5000);
                    String message = "Application had some problems. Update new version please.";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                    System.Environment.Exit(0);
                }
                else
                {
                    Thread.Sleep(10000);
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
                    Thread.Sleep(5000);
                    String message = "Application had some problems. Update new version please.";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                    System.Environment.Exit(0);
                }
            }
            else
            {
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
            orderList = new List<Account>();
            orderInforTextBox.Text = "";
            var filePath = string.Empty;
            var fileContent = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"d:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    string[] lines = System.IO.File.ReadAllLines(filePath);
                    sub_array = lines;
                    Console.WriteLine(sub_array.Length);

                    foreach (String s in sub_array)
                    {
                        if (!String.IsNullOrEmpty(s) && !s.Equals("\r") && !s.Equals("\n") && !s.Equals("\t") && !s.Equals(""))
                        {
                            Account sub_order = new Account();
                            String[] info = s.Split(',');
                            if (info.Length < 6 ||
                                String.IsNullOrWhiteSpace(info[0]) ||
                                String.IsNullOrWhiteSpace(info[1]) ||
                                String.IsNullOrWhiteSpace(info[2]) ||
                                String.IsNullOrWhiteSpace(info[3]) ||
                                String.IsNullOrWhiteSpace(info[4]) ||
                                String.IsNullOrWhiteSpace(info[5]))
                            {
                                Console.WriteLine("This elmement does not have enough attributes");
                                continue;
                            }
                            sub_order.Fname = Regex.Replace(info[0], @"\t|\n|\r", "");
                            sub_order.Lname = Regex.Replace(info[1], @"\t|\n|\r", "");
                            sub_order.Email = @""+ Regex.Replace(info[2], @"\t|\n|\r", "");
                            sub_order.Password = @"" + Regex.Replace(info[3], @"\t|\n|\r", "");
                            sub_order.Proxy = @""+ Regex.Replace(info[4], @"\t|\n|\r", "");
                            sub_order.Port = @""+ Regex.Replace(info[5], @"\t|\n|\r", "");
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
                        String message = "List order is empty. Please add file again.";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if(orderList.Count < sub_array.Length)
                        {
                            String message = $"List order has {orderList.Count()} items. App will remove orders which do not have important attributes (link, time, country, size) or lack of attributes.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Information);
                        }
                        else
                        {
                            String message = $"List order has {orderList.Count()} items.";
                            MessageBoxButtons buttons = MessageBoxButtons.OK;
                            MessageBox.Show(message, "Alert message", buttons, MessageBoxIcon.Information);
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

                //List<string> ordered;
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
        public string LoadDriver(Account orderInfo)
        {
            string result = "";

            var firefoxDriverService = FirefoxDriverService.CreateDefaultService(".","geckodriver.exe");
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

            driver.Navigate().GoToUrl("https://www.ebay.com/");
            Thread.Sleep(1000);
            Console.WriteLine("Click button register");
            // click register
            IWebElement registerBtn = driver.FindElement(By.XPath("//a[text()='register']"));
            registerBtn.Click();
            Thread.Sleep(5000);
            AutoFill(driver, orderInfo);
            result = $"Close account...{orderInfo.Fname} {orderInfo.Lname}";
            driver.Close();
            driver.Dispose();
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
                           result = $"loading {order.Fname} {order.Lname}...";
                           resultTextBox.Invoke(new MethodInvoker(delegate
                           {
                               resultTextBox.Text += result + Environment.NewLine;
                           }
                          ));
                           result = LoadDriver(order);
                           //remove_order.Add(order.OrderLink);
                           Console.WriteLine("Account: {0}, at Thread = {1}",
                              order.Fname+" "+order.Lname,
                              Thread.CurrentThread.ManagedThreadId);
                           // Update result
                           resultTextBox.Invoke(new MethodInvoker(delegate
                           {
                               resultTextBox.Text += result + Environment.NewLine;
                           }
                          ));
                           Thread.Sleep(10000);
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


        private string AutoFill(IWebDriver driver, Account order)
        {
            string result = "";

            var firstName = driver.FindElement(By.Id("firstname"));
            firstName.SendKeys(order.Fname);
            firstName.Submit();
            Console.WriteLine($"Add First Name: {order.Fname}");
            Thread.Sleep(2000);

            var lastName = driver.FindElement(By.Id("lastname"));
            lastName.SendKeys(order.Lname);
            lastName.Submit();
            Console.WriteLine($"Add last Name: {order.Lname}");
            Thread.Sleep(2000);

            var email = driver.FindElement(By.Id("Email"));
            email.SendKeys(order.Email);
            email.Submit();
            Console.WriteLine($"Add last Name: {order.Email}");
            Thread.Sleep(2000);

            var password = driver.FindElement(By.Id("password"));
            password.SendKeys(order.Password);
            password.Submit();
            Console.WriteLine($"Add last Name: {order.Password}");
            Thread.Sleep(2000);

            result = "Done";

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