﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Models
{
    public class AuthentIdScan
    {
        public int Id { get; set; }
        public int AuthentIdUserId { get; set; }
        public int AuthentIdAgentId { get; set; }
        public DateTime ScanDate { get; set; }
        public decimal ScanLatitude { get; set; }
        public decimal ScanLongitude { get; set; }
    }
}