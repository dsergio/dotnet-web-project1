using AutoMapper;
using Project1.Business.Dto;
using Project1.Business.Services;
using Project1.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Business.Tests
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
