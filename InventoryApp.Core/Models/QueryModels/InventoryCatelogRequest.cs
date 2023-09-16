namespace InventoryPOSApp.Core.Models.QueryModels
{
    public class InventoryCatelogRequest
    {
        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int StoreId { get; set; }

        public int NumItemsPerPage { get; set; }
    }
}