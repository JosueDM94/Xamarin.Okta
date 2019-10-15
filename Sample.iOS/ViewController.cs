using CoreFoundation;
using Foundation;
using Okta.Oidc;
using System;
using UIKit;

namespace Sample.iOS
{
    public partial class ViewController : UIViewController
    {
        private OktaOidc oktaAppAuth;

        private OktaOidcStateManager _authStateManager;
        public OktaOidcStateManager authStateManager
        {
            get => _authStateManager;
            set
            {
                _authStateManager?.Clear();
                _authStateManager = value;
                authStateManager?.WriteToSecureStorage();
            }
        }

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            OktaOidcConfig oktaOidc = new OktaOidcConfig("Okta", out NSError error0);
            oktaAppAuth = new OktaOidc(oktaOidc, out NSError error);
            AppDelegate.Shared.oktaOidc = oktaAppAuth;
            if(oktaAppAuth?.Configuration != null)
            {
                authStateManager = authStateManager.ReadFromSecureStorageFor(oktaAppAuth?.Configuration);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if(oktaAppAuth == null)
            {
                UpdateUI("SDK is not configured!");
            }
            BuildTokenTextView();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            var authViewController = segue.DestinationViewController as AuthViewController;
            if (authViewController != null)
            {
                authViewController.oktaAppAuth = oktaAppAuth;
                authViewController.onAuthenticated = (stateManager)=>
                {
                    authStateManager = stateManager;
                };
            }
        }

        partial void SignInButton(UIButton sender)
        {
            SignInWithBrowser();
        }

        partial void SignOutButton(UIButton sender)
        {
            SignOut();
        }

        partial void ClearButton(NSObject sender)
        {
            authStateManager?.Clear();
            BuildTokenTextView();
        }

        partial void UserInfoButton(UIButton sender)
        {
            authStateManager?.GetUser((response, error) =>
            {
                if(error != null)
                {
                    UpdateUI("Error: " + error);
                    return;
                }

                if(response != null)
                {
                    var userInfoText = "";
                    foreach(var row in response)
                    {
                        userInfoText += $"{row.Key}: {row.Value}\n";
                    }
                    UpdateUI(userInfoText);
                }
            });
        }

        partial void IntrospectButton(UIButton sender)
        {
            if (string.IsNullOrWhiteSpace(authStateManager?.AccessToken))
                return;

            authStateManager?.IntrospectWithToken(authStateManager?.AccessToken, (payload, error) =>
            {
                NSNumber isValid = (NSNumber)payload.ValueForKey(new NSString("active"));
                if(!isValid.BoolValue)
                {
                    UpdateUI("Error: " + error.LocalizedDescription ?? "Unknown");
                    return;
                }
                UpdateUI("Is the AccessToken valid? - " + isValid.BoolValue);
            });
        }

        partial void RevokeButton(UIButton sender)
        {
            if (string.IsNullOrWhiteSpace(authStateManager?.AccessToken))
                return;

            authStateManager?.Revoke(authStateManager?.AccessToken, (response, error) =>
            {
                if(error != null)
                {
                    UpdateUI("Error: " + error);
                }
                else
                {
                    UpdateUI("AccessToken was revoked");
                }
            });
        }

        private void SignInWithBrowser()
        {
            oktaAppAuth?.SignInWithBrowserFrom(this, (stateManager, error) =>
            {
                if(error != null)
                {
                    authStateManager = null;
                    UpdateUI("Error: " + error.LocalizedDescription);
                    return;
                }

                authStateManager = stateManager;
                BuildTokenTextView();
            });
        }

        private void SignOut()
        {
            if (authStateManager == null)
                return;

            oktaAppAuth?.SignOutOfOkta(authStateManager, this, (error) =>
            {
                if(error != null)
                {
                    UpdateUI("Error: " + error);
                    return;
                }

                authStateManager = null;
                BuildTokenTextView();
            });
        }

        private void UpdateUI(string updateText)
        {
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                tokenView.Text = updateText;
            });
        }

        private void BuildTokenTextView()
        {
            var currentManager = authStateManager;
            if (currentManager == null)
            {
                tokenView.Text = "";
                return;
            }

            var tokenString = "";
            if(!string.IsNullOrWhiteSpace(currentManager?.AccessToken))
            {
                tokenString += $"\nAccess Token: {currentManager?.AccessToken}\n";
            }

            if (!string.IsNullOrWhiteSpace(currentManager?.IdToken))
            {
                tokenString += $"\nID Token: {currentManager?.IdToken}\n";
            }

            if (!string.IsNullOrWhiteSpace(currentManager?.RefreshToken))
            {
                tokenString += $"\nRefresh Token: {currentManager?.RefreshToken}\n";
            }

            UpdateUI(tokenString);
        }
    }
}