using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CryptoAppXamarinAndroid.Database
{
    public class Constants
    {
        public const string DatabaseFilename = "CryptoAppDatabase.db3";
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;
        public static string DatabasePath
        {
            get
            {
                var basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                return System.IO.Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}