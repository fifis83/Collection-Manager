using scalperApp.ViewModels;

namespace scalperApp.Views;

public partial class UserCollectionView : ContentPage
{
	public UserCollectionView(UserCollectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}