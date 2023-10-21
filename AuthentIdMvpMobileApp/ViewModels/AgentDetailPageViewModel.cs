using AuthentIdMvpMobileApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.ViewModels
{
    [QueryProperty(nameof(Agent), nameof(Agent))]
    public partial class AgentDetailPageViewModel : BaseViewModel
    {

        [ObservableProperty]
        AuthentIdAgent agent;
        [ObservableProperty]
        int agentId;

        public AgentDetailPageViewModel()
        {
        }
    }
}
