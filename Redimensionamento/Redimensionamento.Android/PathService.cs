using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redimensionamento.Interface;
using System.Runtime.CompilerServices;
using System.IO;
using Android.Graphics;
[assembly: Xamarin.Forms.Dependency(typeof(Redimensionamento.Droid.PathService))]
namespace Redimensionamento.Droid
{
    public class PathService : IPathService
    {        
        public string Pictures
        {
            get
            {
                return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            }
        }

        public byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}