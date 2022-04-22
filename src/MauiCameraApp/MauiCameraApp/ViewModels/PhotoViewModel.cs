using MauiCameraApp.Models;

namespace MauiCameraApp.ViewModels
{
    /// <summary>
    /// 写真のVM
    /// </summary>
    public class PhotoViewModel : ViewModelBase
    {
        #region フィールド

        /// <summary>
        /// 写真モデル
        /// </summary>
        private Photo m_Photo;

        /// <summary>
        /// 選択されているか
        /// </summary>
        private bool m_IsSelected;

        #endregion

        #region 構築

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="photo">写真</param>
        internal PhotoViewModel(Photo photo)
        {
            m_Photo = photo;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title 
        {
            get => m_Photo.Title;
            set
            {
                m_Photo.Title = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// パス
        /// </summary>
        public string FilePath
        { 
            get => m_Photo.FilePath;
            set
            {
                m_Photo.FilePath = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsSelected
        {
            get => m_IsSelected;
            set
            {
                m_IsSelected = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
