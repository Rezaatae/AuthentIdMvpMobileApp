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
    public class ScanDataService : IScanService
    {
        // will need to implement a service method for getting scans by userId
        AuthentIdScan _scan = new();
        private readonly IGenericRepository _genericRepository;

        public ScanDataService(IGenericRepository genericRepository) 
        {
            _genericRepository = genericRepository;
        }

        public async Task AddScanAsync(AuthentIdScan authentIdScan)
        {
            Uri uri = new Uri("https://authentidmvp-eastus-dev-001.azurewebsites.net/api/Scan");
            _scan = await _genericRepository.PostAsync<AuthentIdScan>(uri, authentIdScan);
            return;
        }
    }
}
