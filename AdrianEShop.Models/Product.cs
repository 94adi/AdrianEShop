using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AdrianEShop.Models
{
    public class Product : Entity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }
        public string DiscountCode { get; set; }

        public int ManufacturerId { get; set; }
        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public DateTime YearOfManufacture { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public DateTime Published { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
