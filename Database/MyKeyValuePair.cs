using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace CryptoAppXamarinAndroid.Database
{
    public class MyKeyValuePair
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}