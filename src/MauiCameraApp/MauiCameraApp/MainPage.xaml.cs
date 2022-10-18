using MauiCameraApp.Models;
using MauiCameraApp.Services;
using MauiCameraApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiCameraApp;

public partial class MainPage : ContentPage
{
    #region フィールド

	/// <summary>
	/// フォトサービス
	/// </summary>
	PhotoService m_PhotoService;

    /// <summary>
    /// 写真編集ページ
    /// </summary>
    private PhotoEditorPage m_PhotoEditorPage;

    /// <summary>
    /// 選択されている写真
    /// </summary>
    private PhotoViewModel m_SelectedPhoto;

    #endregion

    #region プロパティ

    /// <summary>
    /// 選択されている写真
    /// </summary>
    public PhotoViewModel SelectedPhoto
    {
        get => m_SelectedPhoto;
        set
        {
            m_SelectedPhoto = value;
            ToggleIsSelected();
        }
    }

    /// <summary>
    /// 写真一覧
    /// </summary>
    public ObservableCollection<PhotoViewModel> Photos { get; } = new ObservableCollection<PhotoViewModel>();

    /// <summary>
    /// 編集コマンド
    /// </summary>
    public ICommand EditCommand { get; set; }

    /// <summary>
    /// 削除コマンド
    /// </summary>
    public ICommand DeleteCommand { get; set; }

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainPage(PhotoService photoService, PhotoEditorPage photoEditorPage)
	{
		InitializeComponent();
		BindingContext = this;

        // DI注入されたインスタンスをフィールドに保持
        m_PhotoService = photoService;
        m_PhotoEditorPage = photoEditorPage;

        // コマンド定義
        EditCommand = new Command(Edit, CanEdit);
        DeleteCommand = new Command(Delete, CanDelete);

        // VMの生成
        var photos = m_PhotoService.GetSavingPhotos();
        foreach(var photo in photos)
        {
            Photos.Add(new PhotoViewModel(photo));
        }
	}

    /// <summary>
    /// 写真を編集できるか
    /// </summary>
    /// <param name="arg"></param>
    /// <returns>写真を編集できるか</returns>
    private bool CanEdit(object arg)
    {
        return arg != null;
    }

    /// <summary>
    /// 写真を編集する
    /// </summary>
    /// <param name="obj"></param>
    private async void Edit(object obj)
    {
        if (obj is PhotoViewModel vm)
        {
            m_PhotoEditorPage.BindingContext = vm;
            await Navigation.PushAsync(m_PhotoEditorPage);
        }
    }

    /// <summary>
    /// 写真を削除できるか
    /// </summary>
    /// <param name="arg"></param>
    /// <returns>写真を削除できるか</returns>
    private bool CanDelete(object arg)
    {
        return arg != null;
    }

    /// <summary>
    /// 写真を削除する
    /// </summary>
    /// <param name="obj"></param>
    private void Delete(object obj)
    {
        if(obj is PhotoViewModel vm)
        {
            Photos.Remove(vm);
            File.Delete(vm.FilePath);
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{
        var photo = await m_PhotoService.TakePhotoAsync();
        if (photo != null)
        {
			Photos.Add(new PhotoViewModel(photo));
        }
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        m_PhotoEditorPage.BindingContext = SelectedPhoto;
        await Navigation.PushAsync(m_PhotoEditorPage);
    }

    /// <summary>
    /// 選択状態をトグルする
    /// </summary>
    private void ToggleIsSelected()
    {
        foreach (var item in Photos)
        {
            item.IsSelected = item == SelectedPhoto;
        }
    }
}

