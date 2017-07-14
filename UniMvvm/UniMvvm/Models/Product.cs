using System.Collections.Generic;

namespace UniMvvm.Models
{
    public class Product
    {
        public string Image { get; set; }
        public string ProfilePicture { get; set; }
        public string BusinessName { get; set; }
        public List<PurchasingItem> PurchasingItems { get; set; }

    }

    public class PurchasingItem
    {
        public string Image { get; set; }
        public string Url { get; set; }
    }
}
