using MauiCameraApp.Services;

namespace MauiCameraApp;

public partial class MainPage : ContentPage
{
	int count = 0;

    string PhotoPath { get; set; }

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);

		var service = new PhotoService();
        var photo = await service.TakePhotoAsync();
		m_Photo.Source = photo.Path;
	}
}

