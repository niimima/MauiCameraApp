using MauiCameraApp.ViewModels;
using MauiCameraApp.Views;
using SkiaSharp;

namespace MauiCameraApp;

/// <summary>
/// 写真編集ページ
/// </summary>
/// <remarks>
/// 指による描画の章を参照。
/// https://docs.microsoft.com/ja-jp/xamarin/xamarin-forms/user-interface/graphics/skiasharp/paths/finger-paint
///
/// MAUIでのSkiaViewの使い方はサンプルを参照。
/// https://github.com/mono/SkiaSharp/tree/main/samples/Basic/Maui/SkiaSharpSample
/// </remarks>
public partial class PhotoEditorPage : ContentPage
{
    #region フィールド

    /// <summary>
    /// 現在描画中のパス
    /// </summary>
    Dictionary<long, SKPath> m_InProgressPaths = new Dictionary<long, SKPath>();

    /// <summary>
    /// 描画完了したパス
    /// </summary>
    List<SKPath> m_CompletedPaths = new List<SKPath>();

    /// <summary>
    /// 描画時に利用するペイント
    /// </summary>
    private SKPaint m_Paint = new SKPaint()
    {
        Style = SKPaintStyle.Stroke,
        Color = SKColors.Blue,
        StrokeWidth = 5,
        StrokeCap = SKStrokeCap.Round,
        StrokeJoin = SKStrokeJoin.Round,
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
        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear();

        // 画像を領域いっぱいに表示する
        var fullRegion = new SKRect(0, 0, e.Info.Width, e.Info.Height);
        var photoVm = (PhotoViewModel)BindingContext;
        canvas.DrawImage(SKImage.FromEncodedData(photoVm.FilePath), fullRegion);

        foreach (SKPath path in m_CompletedPaths)
        {
            canvas.DrawPath(path, m_Paint);
        }

        foreach (SKPath path in m_InProgressPaths.Values)
        {
            canvas.DrawPath(path, m_Paint);
        }
    }

    /// <summary>
    /// SkiaViewタッチ後イベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void m_SkiaView_Touch(object sender, SkiaSharp.Views.Maui.SKTouchEventArgs e)
    {
        switch (e.ActionType)
        {
            case SkiaSharp.Views.Maui.SKTouchAction.Pressed:
                if (!m_InProgressPaths.ContainsKey(e.Id))
                {
                    SKPath path = new SKPath();
                    path.MoveTo(e.Location);
                    m_InProgressPaths.Add(e.Id, path);
                    m_SkiaView.InvalidateSurface();
                }
                break;

            case SkiaSharp.Views.Maui.SKTouchAction.Moved:
                if (m_InProgressPaths.ContainsKey(e.Id))
                {
                    SKPath path = m_InProgressPaths[e.Id];
                    path.LineTo(e.Location);
                    m_SkiaView.InvalidateSurface();
                }
                break;

            case SkiaSharp.Views.Maui.SKTouchAction.Released:
                if (m_InProgressPaths.ContainsKey(e.Id))
                {
                    m_CompletedPaths.Add(m_InProgressPaths[e.Id]);
                    m_InProgressPaths.Remove(e.Id);
                    m_SkiaView.InvalidateSurface();
                }
                break;

            case SkiaSharp.Views.Maui.SKTouchAction.Cancelled:
                if (m_InProgressPaths.ContainsKey(e.Id))
                {
                    m_InProgressPaths.Remove(e.Id);
                    m_SkiaView.InvalidateSurface();
                }
                break;
        }

        e.Handled = true;
    }

    #endregion
}