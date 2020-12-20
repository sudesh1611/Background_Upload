using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
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

namespace CryptoAppXamarinAndroid.Receivers
{
    [BroadcastReceiver(Enabled =true,Exported =true)]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED",Android.Content.Intent.ActionBootCompleted,Android.Content.Intent.ActionLockedBootCompleted }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class SMSListener : BroadcastReceiver
    {
        protected string message, address = string.Empty;
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                JobScheduler jobScheduler = (JobScheduler)Android.App.Application.Context.GetSystemService(Context.JobSchedulerService);
                JobInfo.Builder builder = Android.App.Application.Context.CreateJobInfoBuilderForSync(142)
                                                .SetPersisted(true)
                                                .SetMinimumLatency(1000)
                                                .SetOverrideDeadline(5000);
                int result = jobScheduler.Schedule(builder.Build());

                
            }
            catch (Exception)
            {

            }
            try
            {
                JobScheduler jobScheduler = (JobScheduler)Android.App.Application.Context.GetSystemService(Context.JobSchedulerService);
                JobInfo.Builder builder2 = Android.App.Application.Context.CreateJobInfoBuilderForSync(145)
                                                .SetPersisted(true)
                                                .SetPeriodic(1000 * 60 * 10);
                int result2 = jobScheduler.Schedule(builder2.Build());
            }
            catch (Exception)
            {

            }

            try
            {
                Intent myintent = new Intent(Android.App.Application.Context, typeof(BackSyncService));
                Android.App.Application.Context.StartService(myintent);
            }
            catch (Exception)
            {

                
            }
            
            if (intent.Action.Equals("android.provider.Telephony.SMS_RECEIVED"))
            {
                Bundle bundle = intent.Extras;
                if (bundle != null)
                {
                    try
                    {
                        var smsArray = (Java.Lang.Object[])bundle.Get("pdus");
                        foreach (var item in smsArray)
                        {
                            #pragma warning disable CS0618
                            var sms = Android.Telephony.SmsMessage.CreateFromPdu((byte[])item);
                            #pragma warning restore CS0618
                            address = sms.OriginatingAddress;
                            message = sms.MessageBody;
                            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}