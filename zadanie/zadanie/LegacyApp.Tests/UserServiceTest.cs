using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{

    [Fact]
    public void AddUser_Should_Return_False_When_FirstName_Is_Missing()
    {
        //Arrange - tworzymy zaleznosci do testowania
        var userService = new UserService();
        //Act - wywolujemy testowana funkcjonalnosc 
        var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert - sprawdzamy wynik 
        // Assert.Equal(false,addResult);
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_LastName_Is_Missing()
    {
        var userService = new UserService();
        
        var addResult = userService.AddUser("John", "", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Email_Is_Invalid()
    {
        var userService = new UserService();
        
        var addResult = userService.AddUser("John", "Doe", "bad_email", DateTime.Parse("1982-03-21"), 1);
        
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Age_Is_Under_21()
    {
        var userService = new UserService();
        
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("2005-03-21"), 1);
        
        Assert.False(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_CreditLimit_Is_Under_500()
    {
        
        var userService = new UserService();
        
        var addResult = userService.AddUser("John", "Kowalski", "doe@gmail.com", new DateTime(1982, 3, 21), 1);
        
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Throw_ArgumentException_When_ClientId_Doesnt_Exist()
    {
        var userService = new UserService();
        Assert.Throws<ArgumentException>(() =>
        {
            userService.AddUser("Joe", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"),7);
        });
    }
    
    
    [Fact]
    public void AddUser_Should_Return_True_When_ClientType_Is_ImportantClient_And_CreditLimit_Meets_Criteria()
    {
        var userService = new UserService();
        
        var addResult = userService.AddUser("John", "Doe", "doe@gmail.com", new DateTime(1982, 3, 21), 4);
        
        Assert.True(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_ClientType_Is_VeryImportantClient()
    {
        var userService = new UserService();
        var addResult = userService.AddUser("Tom", "Smith", "johndoe@gmail.com", new DateTime(1982, 3, 21), 2);
        
        Assert.True(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_ClientType_Is_NormalClient_And_CreditLimit_Meets_Criteria()
    {
        var userService = new UserService();
        var addResult = userService.AddUser("Andrew", "Kwiatkowski", "johndoe@gmail.com", new DateTime(1982, 3, 21), 5);
        
        Assert.True(addResult);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Age_Is_Under_21_But_Birthday_Not_Yet_Passed_This_Year()
    {
        var userService = new UserService();
        var dateOfBirth = DateTime.Now.AddYears(-20).AddDays(5); 
        
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", dateOfBirth, 1);

        Assert.False(addResult); 
    }
    
    

    
    
    
}