using MauiCameraApp.Services;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MauiCameraApp;

/// <summary>
/// MAUIプログラム。
/// </summary>
public static class MauiProgram
{
	/// <summary>
	/// MAUIアプリを生成します。
	/// </summary>
	/// <returns>MAUIアプリ。</returns>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseSkiaSharp(true)
			.UseMauiApp<App>()
			.RegisterAppServices()
			.RegisterViewModels()
			.RegisterViews()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		return builder.Build();
	}
}
