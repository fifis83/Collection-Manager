using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using scalperApp.Models;
using scalperApp.Services;
using scalperApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace scalperApp.ViewModels
{
    public partial class CollectionsListViewModel : ObservableObject
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "Collections.txt");

        CollectionsListModel model;

        public ObservableCollection<UserCollectionViewModel> Collections { get; set; }
        public string NewCollectionName { get; set; } = "";

        public UserCollectionViewModel SelectedCollection { get; set; }

        public CollectionsListViewModel()
        {
            model = new CollectionsListModel();
            Load();
            RefreshCollections();
        }

        public void RefreshCollections()
        {
            Collections = new ObservableCollection<UserCollectionViewModel>(model.AllCollections.Select(m => new UserCollectionViewModel(m, this)));
            OnPropertyChanged(nameof(Collections));
            Save();
        }

        [RelayCommand]
        private void AddCollection()
        {
            if (!ValidateCollectionName(NewCollectionName)) return;
            model.AllCollections.Add(new UserCollectionModel(NewCollectionName));
            NewCollectionName = "";
            OnPropertyChanged(nameof(NewCollectionName));
            RefreshCollections();
        }

        [RelayCommand]
        private async Task OpenCollectionPageAsync()
        {
            await Shell.Current.Navigation.PushAsync(new UserCollectionView(SelectedCollection));
            SelectedCollection = null;
            OnPropertyChanged(nameof(SelectedCollection));
        }

        public void RemoveCollection(UserCollectionModel collection)
        {
            model.AllCollections.Remove(collection);
            RefreshCollections();
        }

        public void Save()
        {
            File.WriteAllText(filePath, FileService.SerializeCollectionList(model));
            Debug.WriteLine($"Saved to {filePath}");
        }

        public void Load()
        {
            if (!File.Exists(filePath))
            {
                Debug.WriteLine($"No file found at {filePath}");
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            model = FileService.DeserializeCollectionList(lines);
            RefreshCollections();
            Debug.WriteLine($"Loaded from {filePath}");
        }

        private bool ValidateCollectionName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Application.Current?.MainPage.DisplayAlertAsync("Invalid name", "Collection name cannot be empty.", "OK");
                return false;
            }
            return true;
        }
    }
}
