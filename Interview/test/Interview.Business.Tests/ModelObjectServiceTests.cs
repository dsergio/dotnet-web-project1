using AutoMapper;
using Interview.Business.Dto;
using Interview.Business.Services;
using Interview.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interview.Business.Tests
{
    [TestClass]
    public class ModelObjecterviceTests : EntityServiceTests<Dto.ModelObject, Dto.ModelObjectInput, Data.ModelObject>
    {
        [TestInitialize]
        public void TestSetup()
        {
            //using var dbContext = new ApplicationDbContext(Options);
            //dbContext.ModelObjects.Add(new Data.ModelObject("bob@fbi.gov", "linktofile"));
            //dbContext.SaveChanges();
        }

        protected override Data.ModelObject CreateEntity()
            => new Data.ModelObject(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                DateTime.Now
                );

        protected override ModelObjectInput CreateInputDto()
        {
            //return new ModelObjectInput
            //{
            //    EmailAddress = Guid.NewGuid().ToString(),
            //    LinkToFile = Guid.NewGuid().ToString()
            //};
            return new ModelObjectInput
            {
                EmailAddress = Guid.NewGuid().ToString()
            };
        }

        protected override IEntityService<Dto.ModelObject, Dto.ModelObjectInput> GetService(ApplicationDbContext dbContext, IMapper mapper)
            => new ModelObjectService(dbContext, mapper);
    }
}
