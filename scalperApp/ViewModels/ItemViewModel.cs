using CommunityToolkit.Mvvm.ComponentModel;
using scalperApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace scalperApp.ViewModels
{
    public partial class ItemViewModel : ObservableObject
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        public ItemModel Model;
        public ItemViewModel(ItemModel model)
        {
            Model = model;
        }
    }
}
