using MauiCameraApp.Services;

namespace MauiCameraApp;

public partial class App : Application
{
	public App(PhotoService photoService)
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage(photoService));
	}
}
