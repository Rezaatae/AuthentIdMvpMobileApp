﻿using AuthentIdMvpMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Interfaces.Services
{
    public interface IScanService
    {
        Task AddScanAsync(AuthentIdScan authentIdScan);
    }
}
