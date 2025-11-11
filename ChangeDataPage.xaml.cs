namespace MauiApp1;

public partial class ChangeDataPage : ContentPage
{
    private List<Song> songsEntries;
    private List<int> ids;
    public ChangeDataPage(List<Song> songsEntries, List<int> ids)
	{
		InitializeComponent();
        applyButton.IsEnabled = false;
        this.songsEntries = songsEntries;
        this.ids = ids;
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(IdEntry.Text, out int id) || id < 0)
        {
            await DisplayAlert("Notification", "Enter valid id", "Îê");
            return;
        }
        if (ids.Contains(id))
        {
            await DisplayAlert("Notification", "Song with this ID already exists", "Îê");
            return;
        }
        string title = TitleEntry.Text;
        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Notification", "Enter title of song", "Îê");
            return;
        }

        string artist = ArtistEntry.Text;
        if (string.IsNullOrWhiteSpace(artist))
        {
            await DisplayAlert("Notification", "Enter author of song", "Îê");
            return;
        }
        string album = AlbumEntry.Text;
        string releaseYear = ReleaseYearEntry.Text;
        string genre = GenreEntry.Text;

        Song newSong = new Song
        {
            Id = id,
            Title = title,
            Artist = artist,
            Album = album,
            ReleaseYear = releaseYear,
            Genre = genre
        };

        songsEntries.Add(newSong);

        IdEntry.Text = "";
        TitleEntry.Text = "";
        ArtistEntry.Text = "";
        AlbumEntry.Text = "";
        ReleaseYearEntry.Text = "";
        GenreEntry.Text = "";
    }

    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
        if (int.TryParse(IdEntry.Text, out int enteredId))
        {
            if (!ids.Contains(enteredId))
            {
                await DisplayAlert("Notification", "Song with this ID does not exist", "Îê");
            }
            else
            {
                int idToEdit = int.Parse(IdEntry.Text);

                Song songToEdit = songsEntries.FirstOrDefault(song => song.Id == idToEdit);

                if (songToEdit != null)
                {
                    TitleEntry.Text = songToEdit.Title;
                    ArtistEntry.Text = songToEdit.Artist;
                    AlbumEntry.Text = songToEdit.Album;
                    ReleaseYearEntry.Text = songToEdit.ReleaseYear;
                    GenreEntry.Text = songToEdit.Genre;
                }
                applyButton.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Notification", "Enter valid id", "Îê");
        }
    }

    private void OnApplyButtonClicked(object sender, EventArgs e)
    {
        int idToEdit = int.Parse(IdEntry.Text);

        Song songToEdit = songsEntries.FirstOrDefault(song => song.Id == idToEdit);

        if (songToEdit != null)
        {
            songToEdit.Title = TitleEntry.Text;
            songToEdit.Artist = ArtistEntry.Text;
            songToEdit.Album = AlbumEntry.Text;
            songToEdit.ReleaseYear = ReleaseYearEntry.Text;
            songToEdit.Genre = GenreEntry.Text;
        }

        IdEntry.Text = "";
        TitleEntry.Text = "";
        ArtistEntry.Text = "";
        AlbumEntry.Text = "";
        ReleaseYearEntry.Text = "";
        GenreEntry.Text = "";
        applyButton.IsEnabled = false;
    }

    private async void OnMainPageButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        MessagingCenter.Send(this, "UpdateSongs", songsEntries);
    }
}