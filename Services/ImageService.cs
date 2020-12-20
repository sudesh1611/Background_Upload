using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CryptoAppXamarinAndroid.Services
{
    public class ImageService
    {
        public static async Task<bool> UploadImageAsync(string path,string Starting="")
        {
            string UPLOAD_IMAGE = "http://139.59.95.95:4567/save_image";
            byte[] bitmapData;
            try
            {
                var image = BitmapFactory.DecodeFile(path);
                var stream = new MemoryStream();
                image.Compress(Bitmap.CompressFormat.Jpeg, 60, stream);
                bitmapData = stream.ToArray();
            }
            catch (Exception)
            {
                return true;
            }
            try
            {
                using var httpClient = new HttpClient();
                var fname = Xamarin.Essentials.DeviceInfo.Name + Starting + System.IO.Path.GetFileName(path);
                string IUrl = UPLOAD_IMAGE;
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(bitmapData);
                content.Add(baContent, "image", fname);
                var response = await httpClient.PostAsync(IUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}