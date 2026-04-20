using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using scalperApp.Models;
using scalperApp.Views;

namespace scalperApp.ViewModels
{
    public partial class CollectionsListViewModel : ObservableObject
    {
        CollectionsListModel model;
        
        public ObservableCollection<UserCollectionViewModel> Collections { get; set; }
        public string NewCollectionName { get; set; } = "";
        
        public UserCollectionViewModel SelectedCollection { get; set; }

        public CollectionsListViewModel()
        {
            model = new CollectionsListModel();
            RefreshCollections();
        }

        private void RefreshCollections()
        {
            Collections = new ObservableCollection<UserCollectionViewModel>(model.AllCollections.Select(m=>new UserCollectionViewModel(m)));
            OnPropertyChanged(nameof(Collections));
        }

        [RelayCommand]
        private void AddCollection()
        {
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
    }
}
