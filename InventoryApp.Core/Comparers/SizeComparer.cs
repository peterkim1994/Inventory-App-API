using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using InventoryPOS.DataStore.Daos;

namespace InventoryPOSApp.Core
{
    public class SizeComparer : IComparer<Size>
    {
        public int Compare([AllowNull] Size y, [AllowNull] Size x)
        {        
            Regex r = new Regex(@"[0-9]+");
            if (r.IsMatch(y.Value) || r.IsMatch(x.Value))
            {
                return x.Value.CompareTo(y.Value);
            }
            //So sizes can be ordered eg XL and XXS           
            char[] thisSize = x.Value.ToCharArray();
            char[] otherSize = y.Value.ToCharArray();
            Array.Reverse(thisSize);
            Array.Reverse(otherSize);
            string s1 = new string(thisSize);
            string s2 = new string(otherSize);
            System.Diagnostics.Debug.WriteLine("\n\n\n" + thisSize);         

            //comparing this to other, so order will be small-med-large
            return s1.CompareTo(s2);
        }
    }
}
