﻿using FluentAssertions;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V121.FedCm;
//using java.util.concurrent.TimeUnit;
//using org.openqa.selenium.JavascriptExecutor;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtests_Krasheninnikov;


public class SeleniumTestForPractice
{
    
    public void Authorization_method(string args)
    {
        var options = new ChromeOptions();// опции хрома новый метод типо
        options.AddArguments("--no-sandbox",args, "--disable-extensions");//Здесь передаем аргументынастройк
        //window-size=1200x600 start-maximized - для полного экрана
        //зайти в хром (через вебдрайвер)
        driver = new ChromeDriver(options);
        
        //неявное ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);
        
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
        
    }
    public ChromeDriver driver;
    
    [Test]
    public void Authorization()
    {   
        //Авторизируюсь
        Authorization_method("--start-maximized");
        var currentUrl = driver.Url;
        //Смотрю, что есть нужный тег со страницы
        var currentLocation = driver.FindElement(By.CssSelector("[data-tid='DateNews']"));
        currentLocation.Should().NotBeNull();
    }
    
    [Test]
    public void GoToCommunity()
    {
        Authorization_method("--window-size=1000,600)");//разрешение чет не особо работает
        //открываю сообщества кнопкой бокового меню
        var menu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        menu.Click(); 
        var communities = driver.FindElements(By.CssSelector("[data-tid='Community']"))[1];
        communities.Click();
        //смотрю что мы оказались на странице сообщества
        var currentURL = driver.Url;
        currentURL.Should().Be("https://staff-testing.testkontur.ru/communities", "Current url = " + currentURL);
    }
    
    
    [Test]
    public void EditProfile()
    {
        Authorization_method("--start-maximized");
        //Иду в выпадающее меню
        var drop_menu = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']")); //Avatar 
        drop_menu.Click();
        //тыкаю редактировать профиль
        var profile_edit = driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']"));
        profile_edit.Click();
        //смотрю, что мы на нужном Urle
        var currentURL = driver.Url;
        currentURL.Should().Be("https://staff-testing.testkontur.ru/profile/settings/edit", "Current url = " + currentURL);
    }

    //[Test]
    // public void Search()//ПОКА НЕ РАБОТАЕТ КАК ЗАЛЕЗТЬ В ПОИСК ЧЕРЕЗ ЖАВА СКРИПТ
    // {
    //     Authorization_method("--start-maximized");
    //     //var searchBar = driver.FindElement(By.CssSelector("[data-tid='InputLikeText__input']"));
    //     //Thread.Sleep(500);
    //     var fullname = "Поиск сотрудника, подразделения, сообщества, мероприятия";
    //     var searchBar = driver.FindElement(By.CssSelector("div[data-tid='InputLikeText__input']"))
    //         .FindElement(By.XPath("//*[contains(text(),'Поиск сотрудника, подразделения, сообщества, мероприятия')]"));
    //         //var searchBar = driver.FindElement(By.XPath("//input[contains(text(), 'Поиск сотрудника, подразделения, сообщества, мероприятия']"));
    //     //Thread.Sleep(500);
    //     //searchBar.Click();
    //     //searchBar.SendKeys("Крашенинников");
    //     Thread.Sleep(500);
    // }
    
    [Test]
    public void CreateCommunity()
    {
        Authorization_method("--start-maximized");
        //пошел в сообщества
        var communities = driver.FindElement(By.CssSelector("[data-tid='Community']"));
        communities.Click();
        //создаю сообщество
        var createnewcommunity = driver.FindElement(By.XPath("/html/body/div/section/section[2]/section/div[2]/span/button"));
        createnewcommunity.Click();
        var namefield = driver.FindElement(By.CssSelector("[data-tid='Name']"));
        namefield.SendKeys("Название!123nameuniqal&^$#");
        var messegefield = driver.FindElement(By.CssSelector("[data-tid='Message']"));
        messegefield.SendKeys("1f3ы№"); 
        var createbutton = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        createbutton.Click();
        //удаляю сообщество
        var deletebutton = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        deletebutton.Click();
        var deletebutton2 = driver.FindElements(By.CssSelector("[data-tid='DeleteButton']"))[1];
        deletebutton2.Click();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        //ЯВНОЕ ОЖИДАНИЕ
        IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Item']")));
        //иду в сообщества, где я модератор
        var mycommunities = driver.FindElements(By.CssSelector("[data-tid='Item']"))[2];
        mycommunities.Click();
        //Проверяю, что сообществ где я модератор нету, значит оно успешно удалилось
        var nocommunities = driver.FindElement(By.XPath("/html/body/div/section/section[2]/div[1]/div/section/div/div/div/h2"));
        var cat = nocommunities.Text;
        cat.Should().Be("Подходящих сообществ нет");
    }

    [Test]
    public void AdditionalMail()//Не могу залезть в дату
    {
        EditProfile();
        //Чищу поле Дополнительный и ввожу мейл
        var cleartext = driver.FindElement(By.XPath(
            "/html/body/div/section/section[2]/section[3]/div[2]/div[3]/label[2]/span[2]/input"));
        cleartext.SendKeys("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
        var text = driver.FindElement(By.XPath(
            "/html/body/div/section/section[2]/section[3]/div[2]/div[3]/label[2]/span[2]/input"));
        var inputtext = "randommail1234@mail.ru";
        text.SendKeys(inputtext);
        //сохраняю
        var saveopt = driver.FindElement(By.CssSelector("[data-tid='PageHeader']")).FindElement(By.TagName("button"));
        saveopt.Click();
        //иду в редактирвоание профиля
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        //Смотрю что мейл тот который вписал
        var readtext = driver.FindElement(By.XPath(
            "/html/body/div/section/section[2]/section[3]/div[2]/div[3]/label[2]/span[2]/input")).GetAttribute("Value");
        readtext.Should().Be(inputtext);
        
        
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
        Console.WriteLine("pivo");
    }
}   