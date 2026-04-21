namespace scalperApp.Views;

public partial class SummaryView : ContentPage
{
	public SummaryView(string name,int itemCount, int soldCount, int wantToSellCount)
	{
		InitializeComponent();
		CollectionName.Text = name;
		ItemCount.Text = $"Total Items: {itemCount}";
		SoldCount.Text = $"Sold Items: {soldCount}";
		WantToSellCount.Text = $"Items Want to Sell: {wantToSellCount}";
	}
}