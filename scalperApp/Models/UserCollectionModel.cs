using System;
using System.Collections.Generic;
using System.Text;

namespace scalperApp.Models
{
    public class UserCollectionModel
    {
        public List<ItemModel> Items = new List<ItemModel>();
        public string Name;

        public UserCollectionModel(string name)
        {
            Name = name;
        }
    }
}
