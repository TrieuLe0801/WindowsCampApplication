# WindowsCampApplication
Application helps camp and order automatically (Nike.com)
## Dependency
- NodaTime.3.0.1
- Flurl.2.8.2
- NodaTime.Serialization.JsonNet.3.0.0
- Selenium.Firefox.WebDriver.0.27.0
- Selenium.WebDriver.3.141.0
- Selenium.WebDriver.ChromeDriver.86.0.4240.2200
- Selenium.WebDriver.GeckoDriver.Win64.0.27.0
- DotNetSeleniumExtras.WaitHelpers.3.11.0
- SystemClock.0.0.4
- TimeZoneConverter.3.3.0
- TimeZoneConverter.Posix.2.2.0
## Install
- Download AppSetup.rar in AppSetup folder.
- Extract AppSetup.rar.
- Choose setup.exe in Debug folder to install and setup to use.
- Extract geckodriver-v0.27.0-win32.zip(x86) or geckodriver-v0.27.0-win64.zip(x64)
- Copy file geckodriver.exe and paste to folder contain application in system. 
EX: C:\Program Files (x86)\Sneaker Head\Nike Camping\ (you can find it when you run setup.exe)
- Ready to use.
## Image
![alt-text](https://github.com/TrieuLe0801/WindowsCampApplication/blob/master/1.JPG)
## Buttons
- Load File: Load txt file which contains orders information. Format of information below:
```bash
Nike_Product_URL|size|Time_to_start_order|Country_name|First_user_name|Last_user_name|Address|City|State_code|Postal_code|Email|Phone_number|Credit_card|Expiration_Date|CVV
```
Example:
```bash
https://www.nike.com/launch/t/air-max-3-eggplant|W 10.5 / M 9|2020-10-12 11:50 PM|United States|Debra|Witkop|8440 Zephyr Ct|Arvada|CO|80005|leviettrieu612@gmail.com|(303) 829-4107|5148880003424949|09/20|119
```
- Camping: Start processing and wait to order
- Stop: Stop order, if an item is being ordered, user has to wait until finish.
- Clear: Clear all results
- Headless: Open headless mode
## Text box:
- Order Information: all orders'information
- Result Information: all result information
- Launched TAB: number of tabs will open per time
