using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using scalperApp.Models;
using scalperApp.Views;
using System.Collections.ObjectModel;

namespace scalperApp.ViewModels
{
    public partial class UserCollectionViewModel : ObservableObject
    {

        UserCollectionModel model;
        public string Name { get { return model.Name; } set { model.Name = value; } }
        public string NewItemName { get; set; } = "";
        public ItemViewModel SelectedItem { get; set; }

        private bool itemWindowOpen = false;
        CollectionsListViewModel mainPageViewModel;

        public ObservableCollection<ItemViewModel> Items { get; set; }

        public UserCollectionViewModel(UserCollectionModel model, CollectionsListViewModel collectionsListViewModel)
        {
            mainPageViewModel = collectionsListViewModel;
            this.model = model;
            Items = new ObservableCollection<ItemViewModel>(model.Items.Select(m => new ItemViewModel(m)).OrderBy(vm => vm.Sold));
        }

        public void RefreshItems()
        {
            Items = new ObservableCollection<ItemViewModel>(model.Items.Select(m => new ItemViewModel(m)).OrderBy(vm => vm.Sold));
            OnPropertyChanged(nameof(Items));
            SelectedItem = null;
            mainPageViewModel.Save();
        }

        [RelayCommand]
        private void AddItem()
        {
            OpenItemWindow(false);
        }

        [RelayCommand]
        private void EditItem()
        {
            OpenItemWindow(true);
        }

        [RelayCommand]
        private void RemoveItem()
        {
            if (itemWindowOpen) return;
            if (SelectedItem == null)
            {
                Application.Current?.MainPage.DisplayAlertAsync("No item selected", "Please select an item to edit.", "OK");
                return;
            }
            model.Items.Remove(SelectedItem.Model);
            RefreshItems();
        }

        public void AddItemToCollection(ItemModel item)
        {
            model.Items.Add(item);
            RefreshItems();
        }

        private void OpenItemWindow(bool editing)
        {
            if (itemWindowOpen) return;
            itemWindowOpen = true;

            ItemInputView newPage;

            if (editing)
            {
                if (SelectedItem == null)
                {
                    itemWindowOpen = false;
                    Application.Current?.MainPage.DisplayAlertAsync("No item selected", "Please select an item to edit.", "OK");
                    return;
                }
                newPage = new Views.ItemInputView(new ItemInputViewModel(SelectedItem));
            }
            else newPage = new Views.ItemInputView(new ItemInputViewModel());

            var newWindow = new Window { Page = newPage };

            newWindow.Height = 700;
            newWindow.Width = 450;

            newWindow.Destroying += (s, e) =>
            {
                itemWindowOpen = false;
            };
            Application.Current?.OpenWindow(newWindow);

        }

        [RelayCommand]
        private async Task EditCollection()
        {
            string newName = await Application.Current?.MainPage.DisplayPromptAsync("Edit Collection", "Enter new name for the collection:", "OK", "Cancel", initialValue: Name);
            
            if (string.IsNullOrWhiteSpace(newName))
            {
                await Application.Current?.MainPage.DisplayAlertAsync("Invalid Name", "Collection name cannot be empty.", "OK");
                return;
            }
            
            Name = newName;
            mainPageViewModel.RefreshCollections();
        }

        [RelayCommand]
        private void DeleteCollection()
        {
            mainPageViewModel.RemoveCollection(model);
        }

        [RelayCommand]
        private void OpenSummary()
        {
            int soldItems = Items.Count(i => i.Sold);
            int wantToSellItems = Items.Count(i => i.WantToSell);
            var newPage = new Views.SummaryView(Name, Items.Count, soldItems, wantToSellItems);

            var newWindow = new Window { Page = newPage };

            newWindow.Height = 500;
            newWindow.Width = 400;

            Application.Current?.OpenWindow(newWindow);

        }
    }
}
