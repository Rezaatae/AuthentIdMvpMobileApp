using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Interfaces.Repository
{
    public interface IGenericRepository
    {
        Task<List<T>> GetAsync<T>(Uri uri);
        Task<T> GetByIdAsync<T>(Uri uri);
        Task<T> PostAsync<T>(Uri uri, T data);
        Task<T> PutAsync<T>(Uri uri, T data);
        Task DeleteAsync(Uri uri);
        Task<R> PostAsync<T, R>(Uri uri, T data);
    }
}
