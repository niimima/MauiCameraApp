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

        await TakePhotoAsync();
	}

    /// <summary>
    /// 写真を撮影する
    /// </summary>
    /// <remarks>
    /// 参考記事：
    /// https://docs.microsoft.com/ja-jp/xamarin/essentials/media-picker?tabs=android
    /// </remarks>
    async Task TakePhotoAsync()
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            await LoadPhotoAsync(photo);
            Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Feature is not supported on the device
        }
        catch (PermissionException pEx)
        {
            // Permissions not granted
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
        }
    }

    async Task LoadPhotoAsync(FileResult photo)
    {
        // canceled
        if (photo == null)
        {
            PhotoPath = null;
            return;
        }
        // save the file into local storage
        var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
        using (var stream = await photo.OpenReadAsync())
        using (var newStream = File.OpenWrite(newFile))
            await stream.CopyToAsync(newStream);

        PhotoPath = newFile;
    }
}

