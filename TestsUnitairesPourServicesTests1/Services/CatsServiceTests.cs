using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestsUnitairesPourServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsUnitairesPourServices.Data;
using Microsoft.EntityFrameworkCore;
using TestsUnitairesPourServices.Models;

namespace TestsUnitairesPourServices.Services.Tests
{
    [TestClass()]
    public class CatsServiceTests
    {

        DbContextOptions<ApplicationDBContext> options;

        public CatsServiceTests()
        {
            options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "CatsService")
                .UseLazyLoadingProxies(true)
                .Options;
        }

        [TestInitialize]
        public void Init()
        {
            using ApplicationDBContext db = new ApplicationDBContext(options);

            House maisonPropre = new House()
            {
                Id = 1,
                Address = "Tite maison propre et orange",
                OwnerName = "Ludwig"
            };

            House maisonSale = new House()
            {
                Id = 2,
                Address = "Grosse maison sale",
                OwnerName = "Bob"
            };

            db.House.Add(maisonPropre);
            db.House.Add(maisonSale);

            Cat chatPasPropre = new Cat()
            {
                Id = 2,
                Name = "ToutSale",
                Age = 3,
                House = maisonSale
            };
            db.Cat.Add(chatPasPropre);

            db.Cat.Add(new Cat()
            {
                Id = 1,
                Name = "Lonely",
                Age = 12
            });

            db.SaveChanges();
        }

        [TestCleanup]
        public void Dispose()
        {
            using ApplicationDBContext db = new ApplicationDBContext(options);
            db.Cat.RemoveRange(db.Cat);
            db.House.RemoveRange(db.House);
            db.SaveChanges();
        }


        [TestMethod()]
        public void MoveTest()
        {
            using ApplicationDBContext db = new ApplicationDBContext(options);

            var catService = new CatsService(db);
            var maisonPropre = db.House.Find(1);
            var maisonSale = db.House.Find(2);

            var chatChangeDeMaison = catService.Move(2, maisonPropre, maisonSale);

            Assert.IsNotNull(chatChangeDeMaison);
        }
    }
}