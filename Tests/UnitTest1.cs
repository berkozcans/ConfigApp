using ConfigurationLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using WebAppConfig.Controllers;
using NUnit.Framework;


namespace Tests;

public class Tests
{
    private HomeController _controller;
    private Mock<IMongoCollection<Configuration>> _mockCollection;
    private Mock<IMongoDatabase> _mockDatabase;
    private Mock<IMongoClient> _mockClient;
    private Mock<ILogger<HomeController>> _mockLogger;
    
    [SetUp]
    public void Setup()
    {
        _mockCollection = new Mock<IMongoCollection<Configuration>>();
        _mockDatabase = new Mock<IMongoDatabase>();
        _mockClient = new Mock<IMongoClient>();

        _mockDatabase
            .Setup(d => d.GetCollection<Configuration>("Configurations", null))
            .Returns(_mockCollection.Object);

        _mockClient
            .Setup(c => c.GetDatabase("ConfigurationDb", null))
            .Returns(_mockDatabase.Object);
        
        _controller = new HomeController(_mockClient.Object);

    }
    
    [Test]
    public async Task Create_ShouldInsertConfiguration()
    {
        // Arrange
        var config = new Configuration {  _id = ObjectId.GenerateNewId(),IsActive = true,Value = "test-id", Name = "Test Configuration",ApplicationName = "SERVICE-A"};
        // Act
        var result = await _controller.Create(config) as ViewResult;
        Assert.IsInstanceOf<ViewResult>(result);
        Assert.IsNotNull(result);
        Assert.AreEqual(config, result.Model); // Assert that the model returned is the same as input

    }
    [Test]
    public async Task Create_InvalidModelState_ShouldReturnViewResultWithoutInsertingDocument()
    {
        // Arrange
        var config = new Configuration(); 
        // Act
        var result = await _controller.Create(config) as ViewResult;
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(config, result.Model); 

    }
}