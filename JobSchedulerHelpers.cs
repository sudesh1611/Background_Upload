using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace CryptoAppXamarinAndroid
{
    public static class JobSchedulerHelpers
    {
        public static JobInfo.Builder CreateJobInfoBuilderForSync(this Context context, int value)
        {
            
            var component = context.GetComponentNameForJob<SyncService>();
            JobInfo.Builder builder = new JobInfo.Builder(value, component);
            return builder;
        }

        public static ComponentName GetComponentNameForJob<T>(this Context context) where T : JobService
        {
            Type t = typeof(T);
            Class javaClass = Class.FromType(t);
            return new ComponentName(context, javaClass);
        }
    }
}