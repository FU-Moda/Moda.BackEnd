using Microsoft.AspNetCore.Http;
using Moda.BackEnd.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Application.IServices
{
    public interface IFirebaseService
    {
        Task<AppActionResult> UploadFileToFirebase(IFormFile file, string pathFileName);
        public Task<string> GetUrlImageFromFirebase(string pathFileName);
        public Task<AppActionResult> DeleteFileFromFirebase(string pathFileName);
    }
}
