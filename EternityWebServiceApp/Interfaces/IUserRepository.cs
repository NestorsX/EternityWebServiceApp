using EternityWebServiceApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EternityWebServiceApp.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> Get();

        User Get(int id);

        void Create(User obj, IFormFile uploadedFiles);

        void Update(User obj, IFormFile uploadedFiles);

        void Delete(int id);
    }
}
