using MauiCameraApp.ViewModels;
using MauiCameraApp.Views;
using SkiaSharp;

namespace MauiCameraApp;

/// <summary>
/// �ʐ^�ҏW�y�[�W
/// </summary>
public partial class PhotoEditorPage : ContentPage
{
    #region �t�B�[���h

    /// <summary>
    /// �^�b�`�����ʒu
    /// </summary>
    private SKPoint? touchLocation;

    /// <summary>
    /// ������������
    /// </summary>
    private bool m_Initialized = false;

    /// <summary>
    /// �`�掞�ɗ��p����y�C���g
    /// </summary>
    private SKPaint m_Paint = new SKPaint()
    {
        Color = SKColors.Black,
        IsAntialias = true,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 5,
    };

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
        // the the canvas and properties
        var canvas = e.Surface.Canvas;

        // adjust the location based on the pointer
        var coord = (touchLocation is SKPoint loc)
            ? new SKPoint(loc.X, loc.Y)
            : new SKPoint(e.Info.Width / 2, (e.Info.Height + m_Paint.TextSize) / 2);

        // ���߂ĕ\������ꍇ
        if (m_Initialized == false)
        {
            // �摜��̈悢���ς��ɕ\������
            var fullRegion = new SKRect(0, 0, e.Info.Width, e.Info.Height);
            var photoVm = (PhotoViewModel)BindingContext;
            canvas.DrawImage(SKImage.FromEncodedData(photoVm.FilePath), fullRegion);

            // �����\������
            m_Initialized = true;
        }
        else
        {
            // �_��`�悷��
            canvas.DrawPoint(coord, m_Paint);
        }
    }

    /// <summary>
    /// SkiaView�^�b�`��C�x���g�n���h��
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