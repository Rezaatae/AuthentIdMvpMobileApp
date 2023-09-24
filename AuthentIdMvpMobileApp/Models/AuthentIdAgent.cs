using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Models
{
    public class AuthentIdAgent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sector { get; set; }
        public string PositionHeld { get; set; }
        public string PhotoUrl { get; set; }
    }
}
