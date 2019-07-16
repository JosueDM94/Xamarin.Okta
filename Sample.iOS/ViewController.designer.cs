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

		[Action ("clearTokens:")]
		partial void clearTokens (UIKit.UIButton sender);

		[Action ("introspectButton:")]
		partial void introspectButton (UIKit.UIButton sender);

		[Action ("revokeButton:")]
		partial void revokeButton (UIKit.UIButton sender);

		[Action ("signInButton:")]
		partial void signInButton (UIKit.UIButton sender);

		[Action ("signOutOktaButton:")]
		partial void signOutOktaButton (UIKit.UIButton sender);

		[Action ("userInfoButton:")]
		partial void userInfoButton (UIKit.UIButton sender);
		
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
