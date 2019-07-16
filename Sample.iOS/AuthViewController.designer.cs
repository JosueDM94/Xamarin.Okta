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
	[Register ("AuthViewController")]
	partial class AuthViewController
	{
		[Outlet]
		UIKit.UIButton authenticateButton { get; set; }

		[Outlet]
		UIKit.UITextView messageView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView progressIndicator { get; set; }

		[Outlet]
		UIKit.UIView progressOverlay { get; set; }

		[Outlet]
		UIKit.UITextView tokenTextView { get; set; }

		[Action ("authenticate:")]
		partial void authenticate (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (progressOverlay != null) {
				progressOverlay.Dispose ();
				progressOverlay = null;
			}

			if (progressIndicator != null) {
				progressIndicator.Dispose ();
				progressIndicator = null;
			}

			if (authenticateButton != null) {
				authenticateButton.Dispose ();
				authenticateButton = null;
			}

			if (tokenTextView != null) {
				tokenTextView.Dispose ();
				tokenTextView = null;
			}

			if (messageView != null) {
				messageView.Dispose ();
				messageView = null;
			}
		}
	}
}
