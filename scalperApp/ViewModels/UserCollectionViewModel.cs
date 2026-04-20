using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using scalperApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace scalperApp.ViewModels
{
    public partial class UserCollectionViewModel : ObservableObject
    {
        UserCollectionModel model;
        public string Name { get; set; }
        public string NewItemName { get; set; } = "";
        public ItemViewModel SelectedItem { get; set; }


        public ObservableCollection<ItemViewModel> Items { get; set; }

        public UserCollectionViewModel(UserCollectionModel model)
        {
            this.model = model;
            Name = model.Name;
            RefreshItems();
        }

        private void RefreshItems()
        {
            Items = new ObservableCollection<ItemViewModel>(model.Items.Select(m=>new ItemViewModel(m)));
            OnPropertyChanged(nameof(Items));
        }
        
        [RelayCommand]
        private void AddItem()
        {
            model.Items.Add(new ItemModel(NewItemName));
            NewItemName = "";
            OnPropertyChanged(nameof(NewItemName));
            RefreshItems();
        }
        
        [RelayCommand]
        private void RemoveItem()
        {
            model.Items.Remove(SelectedItem.Model);
            RefreshItems();
        }
        
        [RelayCommand]
        private void EditItem()
        {
            //TODO: Make more yk yk
            SelectedItem.Name = NewItemName;
            RefreshItems();
        }
        
    }
}
