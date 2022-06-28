using MauiCameraApp.ViewModels;
using MauiCameraApp.Views;
using SkiaSharp;

namespace MauiCameraApp;

/// <summary>
/// 写真編集ページ
/// </summary>
public partial class PhotoEditorPage : ContentPage
{
    #region フィールド

    /// <summary>
    /// タッチした位置
    /// </summary>
    private SKPoint? touchLocation;

    /// <summary>
    /// 初期化したか
    /// </summary>
    private bool m_Initialized = false;

    /// <summary>
    /// 描画時に利用するペイント
    /// </summary>
    private SKPaint m_Paint = new SKPaint()
    {
        Color = SKColors.Black,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 5,
    };

    #endregion

    #region 構築

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PhotoEditorPage()
	{
		InitializeComponent();
	}

    #endregion

    #region イベントハンドラ

    /// <summary>
    /// SkiaViewペイントイベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void m_SkiaView_PaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        // the the canvas and properties
        var canvas = e.Surface.Canvas;

        // adjust the location based on the pointer
        var coord = (touchLocation is SKPoint loc)
            ? new SKPoint(loc.X, loc.Y)
            : new SKPoint(e.Info.Width / 2, (e.Info.Height + m_Paint.TextSize) / 2);

        // 初めて表示する場合
        if (m_Initialized == false)
        {
            // 画像を領域いっぱいに表示する
            var fullRegion = new SKRect(0, 0, e.Info.Width, e.Info.Height);
            var photoVm = (PhotoViewModel)BindingContext;
            canvas.DrawImage(SKImage.FromEncodedData(photoVm.FilePath), fullRegion);

            // 初期表示完了
            m_Initialized = true;
        }
        else
        {
            // 点を描画する
            canvas.DrawPoint(coord, m_Paint);
        }
    }

    /// <summary>
    /// SkiaViewタッチ後イベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void m_SkiaView_Touch(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        if (e.InContact)
            touchLocation = e.Location;
        else
            touchLocation = null;

        m_SkiaView.InvalidateSurface();

        e.Handled = true;
    }

    #endregion
}