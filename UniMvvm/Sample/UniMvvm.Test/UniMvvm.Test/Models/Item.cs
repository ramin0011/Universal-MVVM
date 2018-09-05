using System;

namespace UniMvvm.Test.Models
{
    public class Item
    {
        public Item()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}