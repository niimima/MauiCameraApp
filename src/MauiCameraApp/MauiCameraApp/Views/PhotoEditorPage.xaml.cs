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

        // make sure the canvas is blank
        canvas.Clear(SKColors.White);

        // decide what the text looks like
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Center,
            TextSize = 24
        };

        // adjust the location based on the pointer
        var coord = (touchLocation is SKPoint loc)
            ? new SKPoint(loc.X, loc.Y)
            : new SKPoint(e.Info.Width / 2, (e.Info.Height + paint.TextSize) / 2);

        // draw some text
        canvas.DrawText("SkiaSharp", coord, paint);

        var photoVm = (PhotoViewModel)BindingContext;
        canvas.DrawImage(SKImage.FromEncodedData(photoVm.FilePath), coord);
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