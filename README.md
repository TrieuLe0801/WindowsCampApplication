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
- Download Setup.rar in Setup folder.
- Extract Setup.rar.
- Choose Setup1.exe in Debug folder to install and setup to use.
- Copy file geckodriver.exe and paste to folder contain application in system. 
EX: C:\Program Files (x86)\Sneaker Head\Setup1\ (you can find it when you run Setup1.exe)
- Ready to use.
## Download Driver:
- [firefox dirver](https://github.com/mozilla/geckodriver/releases) you should choose lastest version
## Image
![alt-text](https://github.com/TrieuLe0801/WindowsCampApplication/blob/master/1.JPG)
## Buttons
- Load File: Load txt file which contains orders information. Format of information below:
  + Second_First_user_name|Second_Last_user_name|Second_Address|Second_City|Second_State_code|Second_Postal_code if you want to billing another address.
  + ||||| keep this space if you billing at the same address.
  
```bash
Nike_Product_URL|size|Time_to_start_order|Country_name|First_user_name|Last_user_name|Address|City|State_code|Postal_code|Email|Phone_number|Credit_card|Expiration_Date|CVV|Second_First_user_name|Second_Last_user_name|Second_Address|Second_City|Second_State_code|Second_Postal_code
```
Example:
```bash
https://www.nike.com/launch/t/air-max-3-eggplant|W 10.5 / M 9|2020-10-12 11:50 PM|United States|Debra|Witkop|8440 Zephyr Ct|Arvada|CO|80005|seankerhead12@gmail.com|(303) 829-4107|5148880003424949|09/20|119|Derbin|Witkop|8442 Zephyr Ct|Arvada|CA|80006

https://www.nike.com/launch/t/air-max-3-eggplant|W 10.5 / M 9|2020-10-12 11:50 PM|United States|Debra|Witkop|8440 Zephyr Ct|Arvada|CO|80005|seankerhead12@gmail.com|(303) 829-4107|5148880003424949|09/20|119||||||
```
- Camping: Start processing and wait to order
- Stop: Stop order, if an item is being ordered, user has to wait until finish.
- Clear: Clear all results
- Headless: Open headless mode
## Text box:
- Order Information: all orders'information
- Result Information: all result information
- Launched TAB: number of tabs will open per time
