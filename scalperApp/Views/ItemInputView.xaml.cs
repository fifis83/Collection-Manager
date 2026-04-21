using scalperApp.ViewModels;

namespace scalperApp.Views;

public partial class ItemInputView : ContentPage
{
	public ItemInputView(ItemInputViewModel viewModel)
	{
		InitializeComponent();
		viewModel.EditingPage = this;
		BindingContext = viewModel;
	}
}