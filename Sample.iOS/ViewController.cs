using Foundation;
using Okta.Oidc;
using System;
using UIKit;

namespace Sample.iOS
{
    public partial class ViewController : UIViewController
    {
        private OktaOidc oktaAppAuth;
        private OktaOidcStateManager authStateManager;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            oktaAppAuth = new OktaOidc(null, out NSError error);
            AppDelegate.Shared.oktaOidc = oktaAppAuth;
            if(oktaAppAuth?.Configuration != null )
            {
                authStateManager = authStateManager.ReadFromSecureStorageFor(oktaAppAuth?.Configuration);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        partial void signOutOktaButton(UIButton sender)
        {
            Console.WriteLine("signOut");
        }
    }
}