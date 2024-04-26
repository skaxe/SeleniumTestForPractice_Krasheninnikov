﻿using System.Drawing;
 using FluentAssertions;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V121.FedCm;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtests_Krasheninnikov;


public class SeleniumTestForPractice
{
    public ChromeDriver driver;
    [SetUp]
    public void SetUP()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--disable-extensions");
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
    }
    public void ExplicitExpectationByDatatid(string datatid)
    {
        IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='"+datatid+"']")));
    }
    [Test]
    public void Authorization()
    {
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("sa.krasheninnikov@yandex.ru");
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("q1W2e!Semen");
        var submit = driver.FindElement(By.Name("button"));
        submit.Click();
        var currentLocation = driver.FindElement(By.CssSelector("[data-tid='DateNews']"));
        currentLocation.Should().NotBeNull();
    }
    
    [Test]
    public void SearchBar()
    {   
        Authorization();
        //ищу профиль в поисковике
        var search = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        search.Click();
        var inputsearch = driver.FindElement(By.CssSelector("[data-tid='SearchBar']")).FindElement(By.TagName("input"));
        var profilename = "Семен1 Крашенинников1";
        inputsearch.SendKeys(profilename);
        var profile = driver.FindElement(By.CssSelector("[data-tid='ComboBoxMenu__item']"));
        profile.Click();
        //проверяю профиль
        var profilepage = driver.FindElement(By.CssSelector("[data-tid='EmployeeName']")).Text;
        profilepage.Should().Be(profilename);
    }
    
    [Test]
    public void GoToCommunity()
    {
        Authorization();
        driver.Manage().Window.Size = new Size(1000, 1000);
        //открываю сообщества кнопкой бокового меню
        var menu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        menu.Click(); 
        var communities = driver.FindElements(By.CssSelector("[data-tid='Community']"))[1];
        communities.Click();
        //смотрю что мы оказались на странице сообщества
        var currentURL = driver.Url;
        currentURL.Should().Be("https://staff-testing.testkontur.ru/communities");
    }
    
    [Test]
    public void EditProfile()
    {
        driver.Manage().Window.Maximize();
        Authorization();
        //Иду в выпадающее меню
        var drop_menu = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']")); 
        drop_menu.Click();
        var profile_edit = driver.FindElement(By.CssSelector("[data-tid='ProfileEdit']"));
        profile_edit.Click();
        //смотрю, что мы на нужной странице
        var currentURL = driver.Url;
        currentURL.Should().Be("https://staff-testing.testkontur.ru/profile/settings/edit");
    }
    
    [Test]
    public void CreateCommunity()
    {
        driver.Manage().Window.Maximize();
        Authorization();
        //пошел в сообщества
        var communities = driver.FindElement(By.CssSelector("[data-tid='Community']"));
        communities.Click();
        //создаю сообщество
        var createnewcommunity = driver.FindElement(By.CssSelector("[data-tid='PageHeader']")).FindElement(By.TagName("button"));
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
        var acceptdelete = driver.FindElements(By.CssSelector("[data-tid='DeleteButton']"))[1];
        acceptdelete.Click();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        ExplicitExpectationByDatatid("CommunitiesCounter");
        //иду в сообщества, где я модератор
        var mycommunities = driver.FindElements(By.CssSelector("[data-tid='Item']"))[2];
        mycommunities.Click();
        //Проверяю, что сообществ где я модератор нету, значит оно успешно удалилось
        var nocommunities = driver.FindElement(By.CssSelector("[data-tid='Feed']")).FindElement(By.TagName("h2")).Text;
        nocommunities.Should().Be("Подходящих сообществ нет");
    }
    
    [Test]
    public void AdditionalMail()
    {
        Authorization();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        ExplicitExpectationByDatatid("FIO");
        //Заполняю Дополнительный email
        var additionalmail = driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail']")).FindElement(By.CssSelector("[data-tid='Input']"));
        additionalmail.SendKeys("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
        var inputtext = "randommail1234@mail.ru";
        additionalmail.SendKeys(inputtext);
        //сохраняю
        var save = driver.FindElement(By.CssSelector("[data-tid='PageHeader']")).FindElement(By.TagName("button"));
        save.Click();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        //Смотрю что мейл тот который вписал
        var readmail = driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail']")).FindElement(By.TagName("input")).GetAttribute("Value");
        readmail.Should().Be(inputtext);
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }
}   