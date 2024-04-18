using FluentAssertions;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtests_Krasheninnikov;

public class SeleniumTestForPractice
{
    public ChromeDriver driver;
    
    [Test]
    public void Authorization()
    {
       
        //изменение через консоль еще изменение для gitextensions
        var options = new ChromeOptions();// опции хрома новый метод типо
        options.AddArguments("--no-sandbox","--window-size=1200x600", "--disable-extensions");//Здесь передаем аргументынастройк
        //window-size=1200x600 start-maximized - для полного экрана
        //зайти в хром (через вебдрайвер)
        driver = new ChromeDriver(options);
        
        //неявное ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        
        // перейте в URL https://staff-testing.testkontur.ru/
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        //ЯВНОЕ ОЖИДАНИЕ
        // IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Username")));
        //Thread.Sleep(2000);
        
        //ввести логин sa.krasheninnikov@yandex.ru
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("sa.krasheninnikov@yandex.ru");
        
        //ввести пароль q1W2e!Semen 
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("q1W2e!Semen");
        
        //нажать войти
        var submit = driver.FindElement(By.Name("button"));
        submit.Click(); 
        //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        //Thread.Sleep(2000);
        //проверить что мы на нужной странице
        var currentUrl = driver.Url;
        
        var currentURL = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        currentURL.Should().NotBeNull();
        //Assert.That(currentUrl  == "https://staff-testing.testkontur.ru/news",
        //"Current url = " + currentUrl + " а должен быть https://staff-testing.testkontur.ru/news");

        //закрыть хром и убить процесс драйвера
        //currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
        
        var menu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        menu.Click(); 
        var communities = driver.FindElements(By.CssSelector("[data-tid='Community']"))[1];
        communities.Click();
        Thread.Sleep(20000);
    }

    [TearDown]
    public void TearDown()
    {
        
        driver.Quit();
        Console.WriteLine("pivo");
    }
}   