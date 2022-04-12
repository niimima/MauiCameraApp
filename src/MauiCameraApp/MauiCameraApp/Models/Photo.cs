using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCameraApp.Models
{
    /// <summary>
    /// 写真
    /// </summary>
    public class Photo : ModelBase
    {
        #region フィールド

        /// <summary>
        /// タイトル
        /// </summary>
        private string m_Title;

        /// <summary>
        /// パス
        /// </summary>
        private string m_Path;

        #endregion

        #region 構築

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">ファイルパス</param>
        internal Photo(string path)
        {
            FilePath = path;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title 
        {
            get => m_Title;
            set
            {
                m_Title = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// パス
        /// </summary>
        public string FilePath
        { 
            get => m_Path;
            set
            {
                m_Path = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
