using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using InventoryPOS.DataStore.Models;

namespace InventoryPOSApp.Core.Comparers
{
    //compares promotions based on price Per Product
    public class PromotionComparer : IComparer<Promotion>
    {
        public int Compare([AllowNull] Promotion x, [AllowNull] Promotion y)
        {
            int xPricePerProduct = x.PromotionPrice / x.Quantity;
            int yPricePerProduct = y.PromotionPrice / y.Quantity;

            return xPricePerProduct.CompareTo(yPricePerProduct);
        }
    }
}
