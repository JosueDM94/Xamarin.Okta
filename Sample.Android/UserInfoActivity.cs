using System;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Support.V7.App;
using AndroidNet = Android.Net;
using Android.Support.Design.Widget;

using Org.Json;
using Okta.AppAuth;
using OpenId.AppAuth;
using Bumptech.Glide;
using Newtonsoft.Json.Linq;

namespace Sample.Android
{
    [Activity(Label = "@string/app_name_short", WindowSoftInputMode = SoftInput.StateHidden)]
    public class UserInfoActivity : AppCompatActivity
    {
        private static string Tag = "UserInfoActivity";
        private static string KeyUserInfo = "userInfo";
        private static string ExtraFailed = "failed";

        private static DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public OktaAppAuth mOktaAppAuth;
        public Dictionary<string,string> mUserInfoJson = new Dictionary<string, string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mOktaAppAuth = OktaAppAuth.GetInstance(this);
            SetContentView(Resource.Layout.activity_user_info);

            if (!mOktaAppAuth.IsUserLoggedIn)
            {
                Log.Debug(Tag, "No logged in user found. Finishing session");
                displayLoading("Finishing session");
                clearData();
                Finish();
            }

            displayLoading(GetString(Resource.String.loading_restoring));

            if (savedInstanceState != null)
            {
                try
                {
                    mUserInfoJson[KeyUserInfo]= savedInstanceState.GetString(KeyUserInfo);
                }
                catch (JSONException ex)
                {
                    Log.Error(Tag, "Failed to parse saved user info JSON, discarding", ex);
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (mOktaAppAuth.IsUserLoggedIn)
            {
                displayAuthorizationInfo();
            }
            else
            {
                Log.Info(Tag, "No authorization state retained - reauthorization required");
                StartActivity(new Intent(this, typeof(LoginActivity)));
                Finish();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (mUserInfoJson[KeyUserInfo] != null)
            {
                outState.PutString(KeyUserInfo, mUserInfoJson[KeyUserInfo]);
            }
        }

        public void refreshAccessToken()
        {
            displayLoading("Refreshing access token");
            mOktaAppAuth.RefreshAccessToken(new OktaAuthListener(this));
        }

        public void fetchUserInfo()
        {
            displayLoading(GetString(Resource.String.user_info_loading));
            mOktaAppAuth.GetUserInfo(new OktaAuthActionCallback(this));
        }

        public void signOut()
        {
            displayLoading("Ending current session");
            Intent completionIntent = new Intent(this, typeof(LoginActivity));
            Intent cancelIntent = new Intent(this, typeof(UserInfoActivity));
            cancelIntent.PutExtra(ExtraFailed, true);
            cancelIntent.SetFlags(ActivityFlags.ClearTop);
            completionIntent.SetFlags(ActivityFlags.ClearTask);

            mOktaAppAuth.SignOutFromOkta(this, PendingIntent.GetActivity(this, 0, completionIntent, 0), PendingIntent.GetActivity(this, 0, cancelIntent, 0));
        }

        public void clearData()
        {
            mOktaAppAuth.Revoke(new OktaRevokeListener(this));            
        }

        public void displayLoading(string message)
        {
            FindViewById(Resource.Id.loading_container).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.authorized).Visibility = ViewStates.Gone;
            FindViewById<TextView>(Resource.Id.loading_description).Text = message;
        }

        public void displayAuthorizationInfo()
        {
            FindViewById(Resource.Id.authorized).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.loading_container).Visibility = ViewStates.Gone;

            TextView refreshTokenInfoView = FindViewById<TextView>(Resource.Id.refresh_token_info);
            refreshTokenInfoView.SetText(!mOktaAppAuth.HasRefreshToken ? Resource.String.no_refresh_token_returned : Resource.String.refresh_token_returned);

            TextView idTokenInfoView = FindViewById<TextView>(Resource.Id.id_token_info);
            idTokenInfoView.SetText(!mOktaAppAuth.HasIdToken ? Resource.String.no_id_token_returned : Resource.String.id_token_returned);

            TextView accessTokenInfoView = FindViewById<TextView>(Resource.Id.access_token_info);
            if (!mOktaAppAuth.HasAccessToken)
            {
                accessTokenInfoView.SetText(Resource.String.no_access_token_returned);
            }
            else
            {
                long expiresAt = mOktaAppAuth.AccessTokenExpirationTime.LongValue();
                if (expiresAt == null)
                {
                    accessTokenInfoView.SetText(Resource.String.no_access_token_expiry);
                }
                else if (expiresAt < (DateTime.UtcNow-Jan1st1970).TotalMilliseconds)
                {
                    accessTokenInfoView.SetText(Resource.String.access_token_expired);
                }
                else
                {
                    string template = GetString(Resource.String.access_token_expires_at);
                    accessTokenInfoView.Text = string.Format(template, expiresAt.ToString("yyyy-MM-dd HH:mm:ss ZZ"));
                }
            }

            Button refreshTokenButton = FindViewById<Button>(Resource.Id.refresh_token);
            refreshTokenButton.Visibility = mOktaAppAuth.HasRefreshToken ? ViewStates.Visible : ViewStates.Gone;
            refreshTokenButton.Click += (sender, args) => { refreshAccessToken(); };

            Button viewProfileButton = FindViewById<Button>(Resource.Id.view_profile);

            viewProfileButton.Visibility = ViewStates.Visible;
            viewProfileButton.Click += (sender,args) => { fetchUserInfo(); };

            Button revokeTokenButton = FindViewById<Button>(Resource.Id.revoke_token);

            revokeTokenButton.Visibility = ViewStates.Visible;
            revokeTokenButton.Click += (sender,args)=> { clearData(); };

            FindViewById(Resource.Id.sign_out).Click += (sender,args) => { signOut(); };

            View userInfoCard = FindViewById(Resource.Id.userinfo_card);
            if (mUserInfoJson.Count <= 0)
                return;
            JObject userInfo = JObject.Parse(mUserInfoJson.GetValueOrDefault(KeyUserInfo));
            if (userInfo == null)
            {
                userInfoCard.Visibility = ViewStates.Invisible;
            }
            else
            {
                try
                {
                    string name = "???";
                    if (userInfo.Value<string>("name") != null)
                    {
                        name = userInfo.Value<string>("name");
                    }
                    ((TextView)FindViewById(Resource.Id.userinfo_name)).Text = name;

                    if (userInfo.Value<string>("picture") != null)
                    {
                        Glide.With(this)
                                .Load(AndroidNet.Uri.Parse(userInfo.Value<string>("picture")))
                                .FitCenter()
                                .Into((ImageView)FindViewById(Resource.Id.userinfo_profile));
                    }

                    ((TextView)FindViewById(Resource.Id.userinfo_json)).Text = mUserInfoJson[KeyUserInfo];
                    userInfoCard.Visibility = ViewStates.Visible;
                }
                catch (JSONException ex)
                {
                    Log.Error(Tag, "Failed to read userinfo JSON", ex);
                }
            }
        }

        public void showSnackbar(string message)
        {
            Window.DecorView.Post(()=>
            {
                Snackbar.Make(FindViewById(Resource.Id.coordinator),message,Snackbar.LengthShort).Show();
            });
        }

        public class OktaAuthListener : Java.Lang.Object, OktaAppAuth.IOktaAuthListener
        {
            private UserInfoActivity Activity;

            public OktaAuthListener(UserInfoActivity activity)
            {
                Activity = activity;
            }

            public void OnSuccess()
            {
                Activity.RunOnUiThread(() => Activity.displayAuthorizationInfo());
            }

            public void OnTokenFailure(AuthorizationException ex)
            {
                Activity.RunOnUiThread(()=> Activity.showSnackbar(Activity.GetString(Resource.String.token_failure_message)));
            }        
        }

        public class OktaAuthActionCallback : Java.Lang.Object, OktaAppAuth.IOktaAuthActionCallback
        {
            private UserInfoActivity Activity;

            public OktaAuthActionCallback(UserInfoActivity activity)
            {
                Activity = activity;
            }

            public void OnFailure(int p0, Java.Lang.Exception p1)
            {
                // Do whatever you need to do with the user info data
                Activity.mUserInfoJson[KeyUserInfo] = null;
                Activity.RunOnUiThread(() => {
                    Activity.displayAuthorizationInfo();
                    Activity.showSnackbar(Activity.GetString(Resource.String.token_failure_message));
                });
            }

            public void OnSuccess(Java.Lang.Object p0)
            {
                Activity.mUserInfoJson[KeyUserInfo] = (p0 as JSONObject).ToString();
                Activity.RunOnUiThread(() => Activity.displayAuthorizationInfo());                
            }

            public void OnTokenFailure(AuthorizationException p0)
            {
                // Handle a network error when fetching the user info data
                Activity.mUserInfoJson[KeyUserInfo] = null;
                Activity.RunOnUiThread(()=> {
                    Activity.displayAuthorizationInfo();
                    Activity.showSnackbar(Activity.GetString(Resource.String.network_failure_message));
                });
            }
        }

        public class OktaRevokeListener : Java.Lang.Object, OktaAppAuth.IOktaRevokeListener
        {
            private UserInfoActivity Activity;

            public OktaRevokeListener(UserInfoActivity activity)
            {
                Activity = activity;
            }

            public void OnError(AuthorizationException p0)
            {
                Activity.showSnackbar("Unable to clean data");
            }

            public void OnSuccess()
            {
                Activity.mOktaAppAuth.ClearSession();
                Activity.StartActivity(new Intent(Activity, typeof(StartActivity)));
                Activity.Finish();
            }
        }
    }
}