using MauiCameraApp.Services;

namespace MauiCameraApp;

/// <summary>
/// アプリケーションを表します。
/// </summary>
public partial class App : Application
{
	/// <summary>
	/// Appを生成します。
	/// </summary>
	/// <param name="mainPage">メインページのビュー。</param>
	public App(MainPage mainPage)
	{
		InitializeComponent();

		MainPage = new NavigationPage(mainPage);
	}
}
