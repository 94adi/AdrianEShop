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
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }
        public string DiscountCode { get; set; }

        public Guid ManufacturerId { get; set; }
        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public DateTime DateOfManufacture { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public DateTime Published { get; set; }
        public DateTime LastEdited { get; set; }

        [Required]
        public bool IsPublished { get; set; }
    }
}
