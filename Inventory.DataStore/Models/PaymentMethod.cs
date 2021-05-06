using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryPOS.DataStore.Models
{
    [Index(nameof(Method), IsUnique = true)]
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Method { get; set; }

        public virtual IList<Payment> Payments { get; set; }
    }
}
