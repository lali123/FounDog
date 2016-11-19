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

namespace FindogMobile.Services
{
    [BroadcastReceiver]
    class DogNotificationReciever : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var nMgr = (NotificationManager)context.GetSystemService(Context.NotificationService);
            var notification = new Notification(Resource.Drawable.Dog, "New dog data is available");
            var pendingIntent = PendingIntent.GetActivity(context, 0, new Intent(context, typeof(SplashScreen)), 0);
            notification.SetLatestEventInfo(context, "Dogs Updated", "New dogs is available", pendingIntent);
            
            nMgr.Notify(0, notification);
        }
    }
}