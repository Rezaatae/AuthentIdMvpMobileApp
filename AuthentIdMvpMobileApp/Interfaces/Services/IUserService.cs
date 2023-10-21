using AuthentIdMvpMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<AuthentIdUser>> LoginUser(string username, string password);
        Task<List<AuthentIdScan>> GetUserScans(int userId);
    }
}
