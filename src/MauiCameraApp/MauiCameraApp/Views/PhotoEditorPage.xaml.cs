using MauiCameraApp.ViewModels;
using MauiCameraApp.Views;
using SkiaSharp;

namespace MauiCameraApp;

/// <summary>
/// �ʐ^�ҏW�y�[�W
/// </summary>
/// <remarks>
/// �w�ɂ��`��̏͂��Q�ƁB
/// https://docs.microsoft.com/ja-jp/xamarin/xamarin-forms/user-interface/graphics/skiasharp/paths/finger-paint
///
/// MAUI�ł�SkiaView�̎g�����̓T���v�����Q�ƁB
/// https://github.com/mono/SkiaSharp/tree/main/samples/Basic/Maui/SkiaSharpSample
/// </remarks>
public partial class PhotoEditorPage : ContentPage
{
    #region �t�B�[���h

    /// <summary>
    /// ���ݕ`�撆�̃p�X
    /// </summary>
    Dictionary<long, SKPath> m_InProgressPaths = new Dictionary<long, SKPath>();

    /// <summary>
    /// �`�抮�������p�X
    /// </summary>
    List<SKPath> m_CompletedPaths = new List<SKPath>();

    /// <summary>
    /// �`�掞�ɗ��p����y�C���g
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
    /// �ۑ����ɗ��p����摜
    /// </summary>
    private SKBitmap m_SaveBitmap;

    #endregion

    #region �\�z

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public PhotoEditorPage()
	{
		InitializeComponent();
	}

    #endregion

    #region �C�x���g�n���h��

    /// <summary>
    /// SkiaView�y�C���g�C�x���g�n���h��
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
            // ���������ɉ摜��ǂݍ���
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
    /// SkiaView�^�b�`��C�x���g�n���h��
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
    /// �N���A�{�^���������̃C�x���g�n���h��
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
    /// �ۑ��{�^���������̃C�x���g�n���h��
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

    #region ��������
    
    /// <summary>
    /// �摜���X�V����
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