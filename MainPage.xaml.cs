using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public List<Song> songsEntries;
        public FileResult file;
        public List<int> ids = new List<int>();

        public MainPage()
        {

            InitializeComponent();
            Label.Text = "Текст";
            ChangeMenuItem.IsEnabled = false;
            SaveMenuItem.IsEnabled = false;
            SearchMenuItem.IsEnabled = false;
            DeleteMenuItem.IsEnabled = false;
            MessagingCenter.Subscribe<ChangeDataPage, List<Song>>(this, "UpdateSongs", (sender, updatedSongs) =>
            {
                Show(updatedSongs);
            });
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            bool searchByTitle = TitleCheckBox.IsChecked;
            bool searchByArtist = ArtistCheckBox.IsChecked;
            bool searchByGenre = GenreCheckBox.IsChecked;

            string selectedTitle = TitleEntry.Text;
            string selectedArtist = ArtistEntry.Text;
            string selectedGenre = GenreEntry.Text;

            if (!searchByTitle && !searchByArtist && !searchByGenre)
            {
                Show(songsEntries);
                return;
            }

            var filteredEntries = songsEntries;

            if (searchByTitle && !string.IsNullOrEmpty(selectedTitle))
            {
                filteredEntries = filteredEntries.Where(entry => entry.Title == selectedTitle).ToList();
            }

            if (searchByArtist && !string.IsNullOrEmpty(selectedArtist))
            {
                filteredEntries = filteredEntries.Where(entry => entry.Artist == selectedArtist).ToList();
            }

            if (searchByGenre && !string.IsNullOrEmpty(selectedGenre))
            {
                filteredEntries = filteredEntries.Where(entry => entry.Genre == selectedGenre).ToList();
            }

            Show(filteredEntries);
        }

        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            TitleCheckBox.IsChecked = false;
            ArtistCheckBox.IsChecked = false;
            GenreCheckBox.IsChecked = false;
            DeleteEntry.Text = "";
            TitleEntry.Text = "";
            ArtistEntry.Text = "";
            GenreEntry.Text = "";
            songsGrid.Children.Clear();
            ChangeMenuItem.IsEnabled = false;
            SaveMenuItem.IsEnabled = false;
            SearchMenuItem.IsEnabled = false;
            DeleteMenuItem.IsEnabled = false;
            file = null;
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (int.TryParse(DeleteEntry.Text, out int selectedId))
            {
                Song entryToRemove = songsEntries.FirstOrDefault(entry => entry.Id == selectedId);

                if (entryToRemove != null)
                {
                    songsEntries.Remove(entryToRemove);
                    ids.Remove(selectedId);
                    Show(songsEntries);
                    DeleteEntry.Text = "";
                }
                else if (!songsEntries.Any(entry => entry.Id == selectedId))
                {
                    await DisplayAlert("Notification", "Song with this ID does not exist", "Ок");
                }
                else
                {
                    await DisplayAlert("Notification", "Error deleting song", "Ок");
                }
            }
            else
            {
                await DisplayAlert("Notification", "Please enter valid ID", "Ок");
            }
        }

        private async void OnOpenFileButtonClicked(object sender, EventArgs e)
        {
            file = await FilePicker.PickAsync();

            if (file != null)
            {
                if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    await DisplayAlert("Notification", "Selected file is not JSON file", "Ок");
                    return;
                }

                using Stream stream = await file.OpenReadAsync();

                if (stream.Length == 0)
                {
                    await DisplayAlert("Notification", "File is empty", "Ок");
                    return;
                }

                ChangeMenuItem.IsEnabled = true;
                SaveMenuItem.IsEnabled = true;
                SearchMenuItem.IsEnabled = true;
                DeleteMenuItem.IsEnabled = true;

                using StreamReader reader = new(stream);
                string json = await reader.ReadToEndAsync();

                JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                };

                songsEntries = JsonSerializer.Deserialize<List<Song>>(json, jsonOptions);
                ids = songsEntries.Select(song => song.Id).ToList();
            }
            else
            {
                await DisplayAlert("Notification", "Failed to open file", "Ок");
            }

            Show(songsEntries);
        }

        private async void OnSaveFileButtonClicked(object sender, EventArgs e)
        {
            if (songsEntries != null && songsEntries.Any())
            {
                if (file != null)
                {
                    songsEntries = songsEntries.OrderBy(song => song.Id).ToList();

                    string filePath = file.FullPath;
                    using StreamWriter writer = new StreamWriter(filePath);

                    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                        AllowTrailingCommas = true,
                        WriteIndented = true,
                    };

                    string json = JsonSerializer.Serialize(songsEntries, jsonOptions);
                    await writer.WriteAsync(json);
                }
            }
            else
            {
                await DisplayAlert("Notification", "Table is empty", "Ок");
            }
        }

        private async void OnAboutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutProgramPage());
        }

        private async void OnChangeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangeDataPage(songsEntries, ids));
        }

        private void OnExitButtonClicked(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Show(List<Song> songs)
        {
            songsGrid.Children.Clear();
            int row = 0;
            if (file != null && songs != null && songs.Any())
            {
                List<Song> sortedSongs = songs.OrderBy(song => song.Id).ToList();
                foreach (var songEntry in sortedSongs)
                {
                    songsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    
                    Label idLabel = new Label { Text = songEntry.Id.ToString() };
                    Label titleLabel = new Label { Text = songEntry.Title };
                    Label artistLabel = new Label { Text = songEntry.Artist };
                    Label albumLabel = new Label { Text = songEntry.Album };
                    Label releaseYearLabel = new Label { Text = songEntry.ReleaseYear };
                    Label genreLabel = new Label { Text = songEntry.Genre };

                    Grid.SetRow(idLabel, row);
                    Grid.SetColumn(idLabel, 0);

                    Grid.SetRow(titleLabel, row);
                    Grid.SetColumn(titleLabel, 1);

                    Grid.SetRow(artistLabel, row);
                    Grid.SetColumn(artistLabel, 2);
                    
                    Grid.SetRow(albumLabel, row);
                    Grid.SetColumn(albumLabel, 3);
                    
                    Grid.SetRow(releaseYearLabel, row);
                    Grid.SetColumn(releaseYearLabel, 4);
                    
                    Grid.SetRow(genreLabel, row);
                    Grid.SetColumn(genreLabel, 5);
                    
                    songsGrid.Children.Add(idLabel);
                    songsGrid.Children.Add(titleLabel);
                    songsGrid.Children.Add(artistLabel);
                    songsGrid.Children.Add(albumLabel);
                    songsGrid.Children.Add(releaseYearLabel);
                    songsGrid.Children.Add(genreLabel);

                    row++;
                }
            }
        }
    }
}