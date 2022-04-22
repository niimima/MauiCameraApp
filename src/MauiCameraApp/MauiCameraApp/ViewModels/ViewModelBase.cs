using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiCameraApp.ViewModels
{
    /// <summary>
    /// VMベース
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region イベント

        /// <summary>
        /// プロパティ変更後イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region 内部処理

        /// <summary>
        /// プロパティ変更を通知する
        /// </summary>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
