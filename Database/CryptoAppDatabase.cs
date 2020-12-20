using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace CryptoAppXamarinAndroid.Database
{
    public class CryptoAppDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public CryptoAppDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(MyKeyValuePair).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(MyKeyValuePair)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<MyKeyValuePair> GetKeyValuePairAsync(string key)
        {
            return Database.Table<MyKeyValuePair>().Where(i => i.Key == key).FirstOrDefaultAsync();
        }

        public Task<int> SaveKeyValuePairAsync(MyKeyValuePair keyValuePair)
        {
            if(keyValuePair.ID!=0)
            {
                return Database.UpdateAsync(keyValuePair);
            }
            else
            {
                return Database.InsertAsync(keyValuePair);
            }
        }

        public Task<int> DeleteKeyValuePair(MyKeyValuePair keyValuePair)
        {
            return Database.DeleteAsync(keyValuePair);
        }

    }
}