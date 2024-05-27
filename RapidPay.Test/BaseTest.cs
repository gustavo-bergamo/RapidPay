using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework.Internal;
using RapidPay.Data;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Authentication.Services;
using RapidPay.Domain.Infrastructure.Seed;

namespace RapidPay.Test;

[TestFixture]
public class BaseTest
{
    public ApplicationDbContext applicationDbContext;
    public IUserSession userSesion;
    public IConfiguration configuration;

    [SetUp]
    public void RunBeforeEachTests()
    {
        ConfigureUserSession();
        ConfigureConfigurations();
        CreateContextDatabase();
    }

    [TearDown]
    public void RunAfterEachTests()
    {
        applicationDbContext.Database.EnsureDeleted();
        applicationDbContext.Dispose();
    }

    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {

    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {

    }

    private void ConfigureUserSession()
    {
        userSesion = Substitute.For<IUserSession>();

        userSesion.UserId.Returns(1);
        userSesion.IPAddress.Returns("0.0.0.1");
    }

    private void ConfigureConfigurations()
    {
        configuration = Substitute.For<IConfiguration>();
        configuration["Cryptokey"] = "AAECAwQFBgcICQoLDA0ODw==";
    }

    private void CreateContextDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("fakeDb").Options;

        var logger = Substitute.For<ILogger<ApplicationDbContext>>();
        var seedService = Substitute.For<ISeedServices>();

        applicationDbContext = new ApplicationDbContext(options, configuration, logger, userSesion, seedService);
        SeedUser();
    }

    private void SeedUser()
    {
        var admin = new ApplicationUser
        {
            Id = 1,
            UserName = "demo@rapidpay.com",
            NormalizedUserName = "DEMO@RAPIDPAY.COM",
            Email = "demo@rapidpay.com",
            NormalizedEmail = "DEMO@RAPIDPAY.COM",
            EmailConfirmed = true
        };
        applicationDbContext.Users.Add(admin);
        applicationDbContext.SaveChanges();
    }
}
