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
    public class AgentDataService : IAgentService
    {
        AuthentIdAgent _agent = new();
        private readonly IGenericRepository _genericRepository;

        public AgentDataService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<AuthentIdAgent> GetAgent(int agentId)
        {
            Uri uri = new Uri($"baseUrl/{agentId}");
            _agent = await _genericRepository.GetByIdAsync<AuthentIdAgent>(uri);
            return _agent;
        }
    }
}
