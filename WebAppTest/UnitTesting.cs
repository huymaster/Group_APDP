using Microsoft.Extensions.Logging.Abstractions;
 using WebApp.Data;
 
 namespace WebAppTest;
 
 public class UnitTesting(PSQLFixture fixture) : IClassFixture<PSQLFixture>
 {
     [Fact]
     public void TestPSQLConnect()
     {
         using var context = new PSQLDbContext(fixture.GetTestDbOptions<PSQLDbContext>());
         Assert.NotNull(context);
     }
 
     [Fact]
     public void TestIdentityConnect()
     {
         using var context = new ApplicationIdentityDbContext(fixture.GetTestDbOptions<ApplicationIdentityDbContext>(),
             NullLogger<ApplicationIdentityDbContext>.Instance);
         Assert.NotNull(context);
     }
 }