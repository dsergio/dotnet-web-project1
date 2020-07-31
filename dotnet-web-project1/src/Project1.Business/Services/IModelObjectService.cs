using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Business.Services
{
    public interface IModelObjectService : IEntityService<Dto.ModelObject, Dto.ModelObjectInput>
    {
        
    }
}
