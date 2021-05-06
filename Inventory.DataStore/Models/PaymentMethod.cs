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

        public string Method { get; set; }
    }
}
