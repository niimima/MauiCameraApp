﻿using MauiCameraApp.Models;
using MauiCameraApp.Services;
using MauiCameraApp.ViewModels;
using System.Collections.ObjectModel;

namespace MauiCameraApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	/// <summary>
	/// フォトサービス
	/// </summary>
	PhotoService m_PhotoService;

    private PhotoViewModel m_SelectedPhoto;

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
    /// コンストラクタ
    /// </summary>
    public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
		m_PhotoService = new PhotoService();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		CounterLabel.Text = $"Current count: {count}";

		SemanticScreenReader.Announce(CounterLabel.Text);

        var photo = await m_PhotoService.TakePhotoAsync();
        if (photo != null)
        {
			Photos.Add(new PhotoViewModel(photo));
        }
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new PhotoEditorPage()
        {
            BindingContext = SelectedPhoto,
        });
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

