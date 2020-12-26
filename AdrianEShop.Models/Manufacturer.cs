﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdrianEShop.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Manufacturer Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }

    }
}
