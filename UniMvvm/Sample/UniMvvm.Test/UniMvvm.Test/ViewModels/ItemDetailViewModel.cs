using System;

using UniMvvm.Test.Models;

namespace UniMvvm.Test.ViewModels
{
    public class ItemDetailViewModel : UniMvvm.ViewModels.Base.ViewModelBase
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
         
            Item = item;
        }
    }
}
