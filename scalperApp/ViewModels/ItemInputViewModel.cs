using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using scalperApp.Models;
using System.Windows.Input;

namespace scalperApp.ViewModels
{
    public partial class ItemInputViewModel : ObservableObject
    {
        public string TitleText { get; set; }
        UserCollectionViewModel userCollection;
        public Page EditingPage;

        public ICommand ConfirmCommand { get; }

        ItemViewModel editingViewModel;

        ItemModel itemModel;

        public string ItemName
        {
            get
            {
                return itemModel.Name;
            }
            set { 
                itemModel.Name = value;
            }
        }
        public float ItemPrice
        {
            get
            {
                return itemModel.Price;
            }
            set
            {
                itemModel.Price = value;
            }
        }
        public string ItemCondition
        {
            get
            {
                return itemModel.Condition;
            }
            set
            {
                itemModel.Condition = value;
            }
        }
        public bool WantToSell
        {
            get
            {
                return itemModel.WantToSell;
            }
            set
            {
                itemModel.WantToSell = value;
            }
        }
        public string ImgBlob
        {
            get
            {
                return itemModel.ImgBlob;
            }
            set
            {
                itemModel.ImgBlob = value;
            }
        }



        public ItemInputViewModel()
        {
            TitleText = "New Item";
            userCollection = Shell.Current.CurrentPage.BindingContext as UserCollectionViewModel;
            itemModel = new ItemModel();
            ConfirmCommand = new AsyncRelayCommand(AddItem);
        }

        public ItemInputViewModel(ItemViewModel itemViewModel)
        {
            TitleText = "Edit an item";
            userCollection = Shell.Current.CurrentPage.BindingContext as UserCollectionViewModel;
            itemModel = new ItemModel();

            ItemName = itemViewModel.Name;
            ItemPrice = itemViewModel.Price;
            ItemCondition = itemViewModel.Condition;
            WantToSell = itemViewModel.WantToSell;
            ImgBlob = itemViewModel.ImgBlob;

            editingViewModel = itemViewModel;

            ConfirmCommand = new AsyncRelayCommand(EditItem);
        }

        private async Task AddItem()
        {
            if (!await ValidateInput()) return;
            if (CheckIfItemAlreadyExists())
            {
                bool result = await EditingPage.DisplayAlertAsync("Warning", "An item with this name already exists.\nDo you wish to proceed?", "Proceed", "Cancel");
                if (!result) return;
            }

            userCollection.AddItemToCollection(itemModel);
            userCollection.RefreshItems();

            CloseWindow();
        }

        private async Task EditItem()
        {
            if (!await ValidateInput()) return;

            if (ItemName != editingViewModel.Name && CheckIfItemAlreadyExists())
            {
                bool result = await EditingPage.DisplayAlertAsync("Warning", "An item with this name already exists.\nDo you wish to proceed?", "Proceed", "Cancel");
                if (!result) return;
            }
            editingViewModel.Name = ItemName;
            editingViewModel.Price = ItemPrice;
            editingViewModel.Condition = ItemCondition;
            editingViewModel.WantToSell = WantToSell;
            editingViewModel.ImgBlob = ImgBlob;
            editingViewModel.RefreshItem();

            userCollection.RefreshItems();
            CloseWindow();
        }

        private bool CheckIfItemAlreadyExists()
        {
            return userCollection.Items.Any(item=>item.Name == ItemName
                    // can add more paramaters if needed
                    //&& item.Price == price
                    );
        }

        private async Task<bool> ValidateInput()
        {
            if(string.IsNullOrWhiteSpace(ItemName))
            {
                await EditingPage.DisplayAlertAsync("Invalid input", "Item name cannot be empty.", "OK");
                return false;
            }
            if(ItemName.Contains('[') || ItemName.Contains(']') || ItemName.Contains('\n') )
            {
                await EditingPage.DisplayAlertAsync("Invalid input", "Item name cannot contain [ or ] characters.", "OK");
                return false;
            }
            if (ItemPrice < 0)
            {
                await EditingPage.DisplayAlertAsync("Invalid input", "Item price cannot be negative.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ItemCondition))
            {
                await EditingPage.DisplayAlertAsync("Invalid input", "Item condition cannot be empty.", "OK");
                return false;
            }
            return true;

        }

        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.CloseWindow(EditingPage.Window);
        }

        [RelayCommand]
        private async Task SelectImage()
        {
            try
            {
                var result = (await MediaPicker.PickPhotosAsync())[0];
                byte[] fileBytes = File.ReadAllBytes(result.FullPath);
                ImgBlob = Convert.ToBase64String(fileBytes);
                OnPropertyChanged(nameof(ImgBlob));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading picture: {ex.Message}");
            }

        }
    }
}
