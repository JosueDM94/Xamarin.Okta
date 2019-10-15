// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Sample.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton signInButton { get; set; }

		[Outlet]
		UIKit.UITextView tokenView { get; set; }

		[Action ("ClearButton:")]
		partial void ClearButton (Foundation.NSObject sender);

		[Action ("IntrospectButton:")]
		partial void IntrospectButton (UIKit.UIButton sender);

		[Action ("RevokeButton:")]
		partial void RevokeButton (UIKit.UIButton sender);

		[Action ("SignInButton:")]
		partial void SignInButton (UIKit.UIButton sender);

		[Action ("SignOutButton:")]
		partial void SignOutButton (UIKit.UIButton sender);

		[Action ("UserInfoButton:")]
		partial void UserInfoButton (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (signInButton != null) {
				signInButton.Dispose ();
				signInButton = null;
			}

			if (tokenView != null) {
				tokenView.Dispose ();
				tokenView = null;
			}
		}
	}
}
