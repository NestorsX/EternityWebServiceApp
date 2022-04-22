using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EternityWebServiceApp.Interfaces
{
    public interface IImageRepository<T>
    {
        IEnumerable<T> Get();

        T Get(int id);

        void Create(T obj, IFormFileCollection uploadedFiles);

        void Update(T obj, IFormFileCollection uploadedFiles);

        void Delete(int id);
    }
}
