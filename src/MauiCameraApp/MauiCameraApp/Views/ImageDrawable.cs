using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics.Platform;

namespace MauiCameraApp.Views
{
    /// <summary>
    /// 画像を描画くするクラス
    /// </summary>
    public class ImageDrawable : IDrawable
    {
        /// <summary>
        /// 画像パス
        /// </summary>
        public string ImagePath { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">画像パス</param>
        public ImageDrawable(string path)
        {
            ImagePath = path;
        }

        /// <summary>
        /// キャンバスに描画する
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // 画像読み込み
            // https://docs.microsoft.com/ja-jp/dotnet/maui/user-interface/graphics/draw#draw-an-image
            // https://docs.microsoft.com/en-us/dotnet/maui/user-interface/graphics/images
            Microsoft.Maui.Graphics.IImage image;
            using (FileStream fs = File.OpenRead(ImagePath))
            {
                image = PlatformImage.FromStream(fs);
            }
            // デバイスサイズの取得
            // https://stackoverflow.com/questions/70712367/net-maui-get-screen-y-and-x
            // var width = (float)DeviceDisplay.MainDisplayInfo.Width;
            // var height = (float)DeviceDisplay.MainDisplayInfo.Height;

            // イメージサイズの変更
            // https://docs.microsoft.com/ja-jp/dotnet/maui/user-interface/graphics/images
            // Microsoft.Maui.Graphics.IImage newImage = image.Resize(width, height);

            canvas.DrawImage(image, 0, 0, dirtyRect.Width, dirtyRect.Height);
        }
    }
}
