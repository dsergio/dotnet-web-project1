using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Business.Services
{
    public interface IEntityService<TDto, TInputDto>
        where TInputDto : class
        where TDto : class, TInputDto
    {
        Task<List<TDto>> FetchAllAsync();
        Task<TDto> FetchByIdAsync(int id);
        Task<TDto> InsertAsync(TInputDto entity);
        Task<TDto?> UpdateAsync(int id, TInputDto entity);
        Task<bool> DeleteAsync(int id);

        Task<bool> UploadAsync(int id, IFormFile file);
        Task<bool> UploadAsync(int id, string link, DateTime? expiration);
    }
}
