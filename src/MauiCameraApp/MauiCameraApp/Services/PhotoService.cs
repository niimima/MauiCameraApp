using MauiCameraApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class PhotoService
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
            // 撮影キャンセルされた場合はnullを返す
            if (photo == null)
            {
                return null;
            }

            // ローカルストレージに保存する
            var title = DateTime.Now.ToString("yyyyMMdd-HH:mm:ss") + Path.GetExtension(photo.FileName);
            var newFileName = Path.Combine(FileSystem.AppDataDirectory, "MauiCameraApp", title);
            await SaveFileAsync(await photo.OpenReadAsync(), newFileName);

            // 保存した写真をモデルにして返す
            var newPhoto = new Photo(newFileName)
            {
                Title = title,
            };
            return newPhoto;
        }

        /// <summary>
        /// ストリームデータをファイルに保存する
        /// </summary>
        internal async Task SaveFileAsync(Stream stream, string fileName)
        {
            using (var newStream = File.OpenWrite(fileName))
                await stream.CopyToAsync(newStream);
        }

        /// <summary>
        /// ファイル保存されている画像一覧を取得する
        /// </summary>
        /// <returns>ファイル保存されている画像一覧</returns>
        internal IEnumerable<Photo> GetSavingPhotos()
        {
            // 保存先のフォルダがない場合は生成する
            var savePath = Path.Combine(FileSystem.AppDataDirectory, "MauiCameraApp");
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }

            // ファイル一覧をモデルに変換する
            var files = Directory.GetFiles(savePath);
            var photos = new List<Photo>();
            foreach (var file in files)
            {
                var photo = new Photo(file);
                photo.Title = Path.GetFileName(file);
                photos.Add(photo);
            }
            return photos;
        }
    }
}
