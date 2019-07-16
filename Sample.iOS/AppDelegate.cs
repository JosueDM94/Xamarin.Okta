using Foundation;
using Okta.Oidc;
using UIKit;

namespace Sample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {        

        public OktaOidc oktaOidc;

        public override UIWindow Window { get; set; }
        public static AppDelegate Shared { get => UIApplication.SharedApplication.Delegate as AppDelegate; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                return oktaOidc?.Resume(url, (NSDictionary<NSString, NSObject>)options) ?? false;
            return false;
        }
    }
}

