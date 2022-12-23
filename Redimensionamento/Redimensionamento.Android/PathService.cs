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
    }
}