using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using labmaui.Models;

namespace labmaui.ViewModels
{
    public class GuitarViewModel : BaseViewModel
    {
        string _baseURL = "https://localhost:5001/api/Guitar";
        private ObservableCollection<GuitarAPI> _guitars;
        public ObservableCollection<GuitarAPI> Guitars
        {
            get => _guitars;
            set => SetProperty(ref _guitars, value);
        }
        private string _nameEntryText;
        public string NameEntryText
        {
            get => _nameEntryText;
            set => SetProperty(ref _nameEntryText, value);
        }
        private string _priceEntryText;
        public string PriceEntryText
        {
            get => _priceEntryText;
            set => SetProperty(ref _priceEntryText, value);
        }
        private string _imageUrlEntryText;
        public string ImageUrlEntryText
        {
            get => _imageUrlEntryText;
            set => SetProperty(ref _imageUrlEntryText, value);
        }

        private HttpClient _client;

        public GuitarViewModel()
        {
            _client = new HttpClient();
            Guitars = new ObservableCollection<GuitarAPI>();
        }

        public async Task LoadGuitarsAsync()
        {
            try
            {
                var response = await _client.GetAsync(_baseURL);
                Debug.WriteLine(_baseURL);
                if (response.IsSuccessStatusCode)
                {
                    var guitars = await response.Content.ReadAsAsync<List<GuitarAPI>>();
                    Guitars.Clear();
                    foreach (var guitar in guitars)
                    {
                        Guitars.Add(guitar);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error fetching guitars: " + ex.Message);
            }
        }

        public async Task AddGuitarAsync()
        {
            Console.WriteLine("In method AddGuitarAsync.");
            try
            {
                if (string.IsNullOrWhiteSpace(NameEntryText))
                {
                    Debug.WriteLine("Missing input. " + _baseURL);
                    return;
                }

                var newGuitar = new GuitarAPI { Name = NameEntryText };
                var response = await _client.PostAsJsonAsync(_baseURL, newGuitar);
                Debug.WriteLine(response);

                if (response.IsSuccessStatusCode)
                {
                    var createdGuitar = await response.Content.ReadAsAsync<GuitarAPI>();

                    // Add the created Phone to your ObservableCollection
                    Debug.WriteLine(createdGuitar.ToString);
                    Guitars.Add(createdGuitar);

                    // Clear the input field
                    NameEntryText = string.Empty;
                }
                else
                {
                    // Handle API error
                    Debug.WriteLine("Error with API.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log, or display an error message as needed
                Debug.WriteLine("Error!!!!! " + ex.Message);
            }
        }

        public async Task UpdateGuitarAsync(GuitarAPI guitar)
        {
            try
            {
                if (guitar == null)
                {
                    // Handle invalid input
                    return;
                }

                var response = await _client.PutAsJsonAsync(_baseURL + "/" + guitar.Id, guitar);
                var existingGuitar = Guitars.FirstOrDefault(p => p.Id == guitar.Id);
                if (existingGuitar != null)
                {
                    existingGuitar.Name = guitar.Name; // Update the property you want to change
                    existingGuitar.Price = guitar.Price;
                    existingGuitar.ImageUrl = guitar.ImageUrl;
                }

                if (!response.IsSuccessStatusCode)
                {
                    // Handle API error
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task DeleteGuitarAsync(GuitarAPI guitar)
        {
            try
            {
                if (guitar == null)
                {
                    // Handle invalid input
                    return;
                }

                var response = await _client.DeleteAsync(_baseURL + "/" + guitar.Id);

                if (response.IsSuccessStatusCode)
                {
                    Guitars.Remove(guitar);
                }
                else
                {
                    // Handle API error
                    Debug.WriteLine("Error with API");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
    }
}

