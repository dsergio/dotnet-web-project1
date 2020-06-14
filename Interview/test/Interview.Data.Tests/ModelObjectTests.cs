using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Interview.Data;
using Microsoft.EntityFrameworkCore;

namespace Interview.Data.Tests
{
	[TestClass]
	public class ModelObjectTests : TestBase
	{
        [TestMethod]
        public async Task ModelObject_CanBeSavedToDatabase()
        {
            // Arrange
            using (var dbContext = new ApplicationDbContext(Options))
            {
                dbContext.ModelObjects.Add(new ModelObject("david@email.com", "www.s3link.com", DateTime.Now));

                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            // Act
            // Assert
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var modelObjects = await dbContext.ModelObjects.ToListAsync();

                Assert.AreEqual(1, modelObjects.Count);
                Assert.AreEqual("david@email.com", modelObjects[0].EmailAddress);
                Assert.AreEqual("www.s3link.com", modelObjects[0].LinkToFile);
            }
        }
    }
}
