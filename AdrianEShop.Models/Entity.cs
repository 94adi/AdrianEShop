using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdrianEShop.Models
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
