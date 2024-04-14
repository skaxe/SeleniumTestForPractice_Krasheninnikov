using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Seleniumtests_Krasheninnikov;

public class SeleniumTestForPractice
{
    [Test]
    public void Authorization()
    {
        
        var options = new ChromeOptions();// опции хрома новый метод типо
        options.AddArguments("--no-sandbox","--start-maximized", "--disable-extensions");//Здесь передаем аргументынастройк
        
        //зайти в хром (через вебдрайвер)
        var driver = new ChromeDriver(options);
        
        // перейте в URL https://staff-testing.testkontur.ru/
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        Thread.Sleep(2000);
        
        //ввести логин sa.krasheninnikov@yandex.ru
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("sa.krasheninnikov@yandex.ru");
        
        //ввести пароль q1W2e!Semen 
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("q1W2e!Semen");
        
        //нажать войти
        var submit = driver.FindElement(By.Name("button"));
        submit.Click();
        
        Thread.Sleep(2000);
        //проверить что мы на нужной странице
        var currentUrl = driver.Url;
        Assert.That(currentUrl  == "https://staff-testing.testkontur.ru/news");
        
        //закрыть хром и убить процесс драйвера
        driver.Quit();  
    }
}   