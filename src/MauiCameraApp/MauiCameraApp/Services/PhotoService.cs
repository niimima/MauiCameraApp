using MauiCameraApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCameraApp.Services
{
    /// <summary>
    /// 写真操作に関するサービス
    /// </summary>
    /// <remarks>
    /// 参考記事：
    /// https://docs.microsoft.com/ja-jp/xamarin/essentials/media-picker?tabs=android
    /// </remarks>
    internal class PhotoService
    {
        /// <summary>
        /// 写真を撮影する
        /// </summary>
        internal async Task<Photo> TakePhotoAsync()
        {
            try
            {
                var photoResult = await MediaPicker.CapturePhotoAsync();
                var photo = await LoadPhotoAsync(photoResult);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {photo.FilePath}");
                return photo;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }

            // 写真撮影に失敗した場合は何も返さない
            return null;
        }

        /// <summary>
        /// 写真を読み込む
        /// </summary>
        /// <param name="photo">撮影結果</param>
        /// <returns>撮影に成功した場合はPhotoクラスを、撮影に失敗した場合はnullを返す</returns>
        async Task<Photo> LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                // 撮影キャンセルされた場合はnullを返す
                return null;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            return new Photo(newFile);
        }
    }
}
