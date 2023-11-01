using labmaui.Models;
using labmaui.ViewModels;

namespace labmaui;

public partial class MainPage : ContentPage
{
    private GuitarViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = new GuitarViewModel();
        BindingContext = _viewModel;
        GuitarsListView.ItemTapped += GuitarsListView_ItemTapped;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadGuitarsAsync();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await _viewModel.AddGuitarAsync();
        _viewModel.NameEntryText = string.Empty;
        _viewModel.PriceEntryText = null;
        _viewModel.ImageUrlEntryText = string.Empty;
    }
    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        if (GuitarsListView.SelectedItem is GuitarAPI selectedGuitar && !string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            selectedGuitar.Name = NameEntry.Text;
            selectedGuitar.Price = Convert.ToDouble(PriceEntry);
            selectedGuitar.ImageUrl = ImageUrlEntry.Text;
            await _viewModel.UpdateGuitarAsync(selectedGuitar);

            GuitarsListView.SelectedItem = null; // Deselect the item after update
            _viewModel.NameEntryText = string.Empty;
            _viewModel.PriceEntryText = null;
            _viewModel.ImageUrlEntryText = string.Empty; // Clear the input field
        }
        else
        {
            // Handle invalid input or no selected item
            await DisplayAlert("Error", "Invalid input or no item selected.", "OK");
        }
    }


    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var guitar = button.CommandParameter as GuitarAPI;
            if (guitar != null)
            {
                await _viewModel.DeleteGuitarAsync(guitar);
                GuitarsListView.SelectedItem = null;
            }
        }
    }
    private void GuitarsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is GuitarAPI selectedGuitar)
        {
            _viewModel.NameEntryText = selectedGuitar.Name;
            _viewModel.PriceEntryText = selectedGuitar.Price.ToString();
            _viewModel.ImageUrlEntryText = selectedGuitar.ImageUrl;
        }
    }
}


