using MauiCameraApp.ViewModels;
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

    #region �I�[�o�[���C�h

    /// <summary>
    /// �o�C���f�B���O�R���e�L�X�g�ύX�㏈��
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        // �O���t�B�b�N�r���[�ɉ摜��`��
        var vm = BindingContext as PhotoViewModel;
        if (vm != null && string.IsNullOrEmpty(vm.FilePath) == false)
        {
            m_GraphicsView.Drawable = new ImageDrawable(vm.FilePath);
        }
    }

    #endregion
}