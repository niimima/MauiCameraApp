using MauiCameraApp.Models;
using MauiCameraApp.Services;
using System.Collections.ObjectModel;

namespace MauiCameraApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	/// <summary>
	/// フォトサービス
	/// </summary>
	PhotoService m_PhotoService;

    /// <summary>
    /// 写真一覧
    /// </summary>
    public ObservableCollection<Photo> Photos { get; } = new ObservableCollection<Photo>();

    public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
		m_PhotoService = new PhotoService();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);

        var photo = await m_PhotoService.TakePhotoAsync();
        if (photo != null)
        {
			Photos.Add(photo);
        }
	}
}

