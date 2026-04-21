using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public float Price
        {
            get { return Model.Price; }
            set { Model.Price = value; }
        }
        public string Condition
        {
            get { return Model.Condition; }
            set { Model.Condition = value; }
        }
        public bool WantToSell
        {
            get { return Model.WantToSell; }
            set { Model.WantToSell = value; }
        }
        public bool Sold
        {
            get { return Model.Sold; }
            set { Model.Sold = value; }
        }

        public string ImgBlob
        {
            get { return Model.ImgBlob; }
            set { Model.ImgBlob = value; }
        }

        public string BgColor
        {
            get
            {
                if (Sold) return "#555555";
                if (WantToSell) return "#1B5E20";
                return "#1F1F1F";
            }
        }

        public string SellBtnText
        {
            get
            {
                return Sold ? "Cancel Sell" : "$ Sell";
            }
        }
        public ItemModel Model;

        public ItemViewModel(ItemModel model)
        {
            Model = model;
        }

        public void RefreshItem()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(Condition));
            OnPropertyChanged(nameof(WantToSell));
            OnPropertyChanged(nameof(Sold));
            OnPropertyChanged(nameof(BgColor));
        }

        [RelayCommand]
        private void Sell()
        {
            Sold = !Sold;
            WantToSell = false;
            RefreshItem();
            ((UserCollectionViewModel)Shell.Current.CurrentPage.BindingContext).RefreshItems();
        }
    }
}
