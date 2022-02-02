using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SiteUrl { get; set; }
        public int IsHttps { get; set; }
        public string ImageUrl { get; set; }
    }
}
