using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniMvvm.Models;

namespace UniMvvm.Helpers
{
    public class SampleData
    {
        public static Product GetProductDetailViewModel()
        {
            return new Product()
            {
                ProfilePicture = "sample.png",
                BusinessName = "aleyn_molina",
                Image = "sample.png",
                PurchasingItems = new List<PurchasingItem>()
                {
                    new PurchasingItem()
                    {
                        Image = "sample.png",
                        Url = "http://google.com"
                    }
                }
            };
        }
        public static ObservableCollection<Product> GetProducts()
        {
            var products= new ObservableCollection<Product>
            {              
            };
            for (int i = 0; i < 10; i++)
            {
                products.Add(new Product()
                {
                    ProfilePicture = "sample.png",
                    BusinessName = "aleyn_molina",
                    Image = "sample.png",
                    PurchasingItems = new List<PurchasingItem>()
                    {
                        new PurchasingItem()
                        {
                            Image = "sample.png",
                            Url = "http://google.com"
                        }
                    }
                });
            }
            return products;
        }
    }
}
