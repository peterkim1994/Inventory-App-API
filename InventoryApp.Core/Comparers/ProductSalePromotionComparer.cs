using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core.Comparers
{
    public class ProductSalePromotionComparer : IComparer<ProductSale>
    {
        public int Compare([AllowNull] ProductSale x, [AllowNull] ProductSale y)
        {
            int xPromo = x.Promotion.Id;
            int yPromo = y.Promotion.Id;

            if(xPromo == yPromo)
            {
                return x.ProductId.CompareTo(y.ProductId); 
            }
            else
            {
                return xPromo.CompareTo(yPromo);
            }
        }
    }
}
