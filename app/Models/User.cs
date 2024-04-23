﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FIO { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        public string Theme { get; set; } = "Light";
        public string Language { get; set; } = ".ru-RU";

        public List<ProductFromBasket> ProductsFromBasket { get; set; } = new();
    }
}
