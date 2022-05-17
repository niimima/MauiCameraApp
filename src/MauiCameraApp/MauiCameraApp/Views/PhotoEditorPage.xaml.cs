using MauiCameraApp.Views;

namespace MauiCameraApp;

public partial class PhotoEditorPage : ContentPage
{
    #region 構築

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PhotoEditorPage()
	{
		InitializeComponent();

        // タップジェスチャにイベントハンドラを登録する
        // https://stackoverflow.com/questions/71911160/how-to-capture-global-touch-events-in-maui-app
		var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
        m_GraphicsView.GestureRecognizers.Add(tapGestureRecognizer);
	}

    #endregion

    #region イベントハンドラ

    /// <summary>
    /// タップ後イベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        // 赤い線を引くのみ
        m_GraphicsView.Drawable = new GraphicsDrawable();
    }

    #endregion
}