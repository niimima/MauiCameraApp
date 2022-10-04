using MauiCameraApp.Services;
using MauiCameraApp.ViewModels;
using MauiCameraApp.Views;

namespace MauiCameraApp
{
    /// <summary>
    /// ブートストラップです。
    /// MAUIアプリ起動時処理を実装します。
    /// </summary>
    /// <remarks>
    /// 以下のサイトを参考に実装。
    /// https://learn.microsoft.com/en-us/dotnet/architecture/maui/dependency-injection
    /// </remarks>
    public static class Bootstrap
    {
        /// <summary>
        /// アプリケーションサービスを登録します。
        /// </summary>
        /// <param name="mauiAppBuilder">MAUIアプリビルダー。</param>
        /// <returns>MAUIアプリビルダー。</returns>
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<PhotoService>();

            return mauiAppBuilder;
        }

        /// <summary>
        /// ビューモデルを登録します。
        /// </summary>
        /// <param name="mauiAppBuilder">MAUIアプリビルダー。</param>
        /// <returns>MAUIアプリビルダー。</returns>
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            return mauiAppBuilder;
        }

        /// <summary>
        /// ビューを登録します。
        /// </summary>
        /// <param name="mauiAppBuilder">MAUIアプリビルダー。</param>
        /// <returns>MAUIアプリビルダー。</returns>
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<MainPage>();
            mauiAppBuilder.Services.AddSingleton<PhotoEditorPage>();

            return mauiAppBuilder;
        }
    }
}
