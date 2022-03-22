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
    internal class Photo : ModelBase
    {
        #region フィールド

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
            Path = path;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// パス
        /// </summary>
        public string Path
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
