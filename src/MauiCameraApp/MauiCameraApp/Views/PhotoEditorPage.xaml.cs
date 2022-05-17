using MauiCameraApp.Views;

namespace MauiCameraApp;

public partial class PhotoEditorPage : ContentPage
{
    #region �\�z

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public PhotoEditorPage()
	{
		InitializeComponent();

        // �^�b�v�W�F�X�`���ɃC�x���g�n���h����o�^����
        // https://stackoverflow.com/questions/71911160/how-to-capture-global-touch-events-in-maui-app
		var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
        m_GraphicsView.GestureRecognizers.Add(tapGestureRecognizer);
	}

    #endregion

    #region �C�x���g�n���h��

    /// <summary>
    /// �^�b�v��C�x���g�n���h��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        // �Ԃ����������̂�
        m_GraphicsView.Drawable = new GraphicsDrawable();
    }

    #endregion
}