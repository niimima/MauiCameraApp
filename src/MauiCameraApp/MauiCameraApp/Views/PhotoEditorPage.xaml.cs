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

    /// <summary>
    /// 保存時に利用する画像
    /// </summary>
    private SKBitmap m_SaveBitmap;

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
        SKImageInfo info = e.Info;
        SKSurface surface = e.Surface;
        SKCanvas canvas = surface.Canvas;
        var fullRegion = new SKRect(0, 0, e.Info.Width, e.Info.Height);

        // Create bitmap the size of the display surface
        if (m_SaveBitmap == null)
        {
            // 初期化時に画像を読み込む
            var photoVm = (PhotoViewModel)BindingContext;
            m_SaveBitmap = SKBitmap.FromImage(SKImage.FromEncodedData(photoVm.FilePath));
        }
        // Or create new bitmap for a new size of display surface
        else if (m_SaveBitmap.Width < info.Width || m_SaveBitmap.Height < info.Height)
        {
            SKBitmap newBitmap = new SKBitmap(Math.Max(m_SaveBitmap.Width, info.Width),
                                              Math.Max(m_SaveBitmap.Height, info.Height));

            using (SKCanvas newCanvas = new SKCanvas(newBitmap))
            {
                newCanvas.Clear();
                newCanvas.DrawBitmap(m_SaveBitmap, fullRegion);
            }

            m_SaveBitmap = newBitmap;
        }

        // Render the bitmap
        canvas.Clear();
        canvas.DrawBitmap(m_SaveBitmap, fullRegion);
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
                    UpdateBitmap();
                }
                break;

            case SkiaSharp.Views.Maui.SKTouchAction.Moved:
                if (m_InProgressPaths.ContainsKey(e.Id))
                {
                    SKPath path = m_InProgressPaths[e.Id];
                    path.LineTo(e.Location);
                    UpdateBitmap();
                }
                break;

            case SkiaSharp.Views.Maui.SKTouchAction.Released:
                if (m_InProgressPaths.ContainsKey(e.Id))
                {
                    m_CompletedPaths.Add(m_InProgressPaths[e.Id]);
                    m_InProgressPaths.Remove(e.Id);
                    UpdateBitmap();
                }
                break;

            case SkiaSharp.Views.Maui.SKTouchAction.Cancelled:
                if (m_InProgressPaths.ContainsKey(e.Id))
                {
                    m_InProgressPaths.Remove(e.Id);
                    UpdateBitmap();
                }
                break;
        }

        e.Handled = true;
    }

    /// <summary>
    /// クリアボタン押下時のイベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void OnClearButtonClicked(object sender, EventArgs args)
    {
        m_CompletedPaths.Clear();
        m_InProgressPaths.Clear();
        UpdateBitmap();
    }

    /// <summary>
    /// 保存ボタン押下時のイベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    async void OnSaveButtonClicked(object sender, EventArgs args)
    {
        using (SKImage image = SKImage.FromBitmap(m_SaveBitmap))
        {
            SKData data = image.Encode();
            var photoVm = (PhotoViewModel)BindingContext;
            string filename = photoVm.FilePath;
            using (var stream = File.Open(photoVm.FilePath, FileMode.Create))
                await data.AsStream().CopyToAsync(stream);
        }
    }

    #endregion

    #region 内部処理
    
    /// <summary>
    /// 画像を更新する
    /// </summary>
    private void UpdateBitmap()
    {
        using (SKCanvas saveBitmapCanvas = new SKCanvas(m_SaveBitmap))
        {
            foreach (SKPath path in m_CompletedPaths)
            {
                saveBitmapCanvas.DrawPath(path, m_Paint);
            }

            foreach (SKPath path in m_InProgressPaths.Values)
            {
                saveBitmapCanvas.DrawPath(path, m_Paint);
            }
        }

        m_SkiaView.InvalidateSurface();
    }

    #endregion
}