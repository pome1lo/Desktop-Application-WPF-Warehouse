﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{
    public class Category
    {
        [Key] public int Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
    }
}
