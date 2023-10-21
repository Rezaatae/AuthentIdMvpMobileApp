using AuthentIdMvpMobileApp.Interfaces.Repository;
using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Services.Data
{
    public class UserDataService : IUserService
    {
        List<AuthentIdScan> _scanList = new();
        List<AuthentIdUser> _userList = new();
        private readonly IGenericRepository _genericRepository;

        public UserDataService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<AuthentIdScan>> GetUserScans(int userId)
        {
            Uri uri = new Uri($"https://authentidmvp-eastus-dev-001.azurewebsites.net/api/User/{userId}");
            Console.WriteLine( uri.ToString() );

            _scanList = await _genericRepository.GetAsync<AuthentIdScan>(uri);
            return _scanList;
        }

        public async Task<List<AuthentIdUser>> LoginUser(string username, string password)
        {
            Uri uri = new Uri($"https://authentidmvp-eastus-dev-001.azurewebsites.net/api/User/LoginUser/{username}/{password}");
            _userList = await _genericRepository.GetAsync<AuthentIdUser>(uri);
            return _userList;
        }
    }
}
