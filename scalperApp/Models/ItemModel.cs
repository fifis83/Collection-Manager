using System;
using System.Collections.Generic;
using System.Text;

namespace scalperApp.Models
{
    public class ItemModel
    {
        public string Name;
        public float Price;
        public string Condition;
        public bool WantToSell;
        public bool Sold;
        public string ImgBlob;


        public ItemModel(string name = "item", float price = 0.0f, string condition = "New", bool wantToSell = false, bool sold = false, string imgBlob = "")
        {
            Name = name;
            Price = price;
            Condition = condition;
            WantToSell = wantToSell;
            ImgBlob = imgBlob;
            Sold = sold;
        }
    }
}
