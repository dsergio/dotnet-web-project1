using AutoMapper;
using Interview.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Interview.Business
{
    public class AutomapperConfigurationProfile : Profile
    {
        public AutomapperConfigurationProfile()
        {
            CreateMap<Dto.ModelObjectInput, ModelObject>();
            CreateMap<ModelObject, Dto.ModelObject>();
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
