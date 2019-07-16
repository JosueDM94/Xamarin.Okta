using System;

using Android.OS;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Support.V7.App;

using Okta.AppAuth;
using OpenId.AppAuth;

namespace Sample.Android
{
    [Activity(Label = "@string/app_name_short", WindowSoftInputMode = SoftInput.StateHidden)]
    public class LoginActivity : AppCompatActivity, OktaAppAuth.IOktaAuthListener
    { 
        private static string Tag = "LoginActivity";
        private static string ExtraFailed = "failed";

        private OktaAppAuth mOktaAppAuth;

        public LinearLayout mContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mOktaAppAuth = OktaAppAuth.GetInstance(this);

            SetContentView(Resource.Layout.activity_login);

            FindViewById(Resource.Id.start_auth).Click += LoginActivity_Click;

            ((EditText)FindViewById(Resource.Id.login_hint_value)).AddTextChangedListener(new OktaAppAuth.LoginHintChangeHandler(mOktaAppAuth));

            mContainer = FindViewById<LinearLayout>(Resource.Id.auth_container);
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (Intent.GetBooleanExtra(ExtraFailed, false))
            {
                showMessage(GetString(Resource.String.auth_canceled));
            }

            initializeOktaAuth();
        }

        protected override void OnDestroy()
        {
            if (mOktaAppAuth != null)
            {
                mOktaAppAuth.Dispose();
                mOktaAppAuth = null;
            }
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
        }

        private void LoginActivity_Click(object sender, EventArgs e)
        {
            startAuth();
        }

        private void initializeOktaAuth()
        {
            Log.Info(Tag, "Initializing OktaAppAuth");
            displayLoading(GetString(Resource.String.loading_initializing));

            mOktaAppAuth.Init(this,this,getColorCompat(Resource.Color.colorPrimary));
    }        

        private void startAuth()
        {
            displayLoading(GetString(Resource.String.loading_authorizing));

            Intent completionIntent = new Intent(this, typeof(UserInfoActivity));
            Intent cancelIntent = new Intent(this, typeof(LoginActivity));
            cancelIntent.PutExtra(ExtraFailed, true);
            cancelIntent.SetFlags(ActivityFlags.ClearTop);

            mOktaAppAuth.Login(
                    this,
                    PendingIntent.GetActivity(this, 0, completionIntent, 0),
                    PendingIntent.GetActivity(this, 0, cancelIntent, 0)
            );
        }

        private void displayLoading(string loadingMessage)
        {
            FindViewById(Resource.Id.loading_container).Visibility = ViewStates.Visible;
            mContainer.Visibility = ViewStates.Gone;

            ((TextView)FindViewById(Resource.Id.loading_description)).Text = loadingMessage;
        }

        private void displayAuthOptions()
        {
            mContainer.Visibility = ViewStates.Visible;

            FindViewById(Resource.Id.loading_container).Visibility= ViewStates.Gone;
        }

        private void showMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private int getColorCompat(int color)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                return GetColor(color);
            }
            else
            {
                return Resources.GetColor(color);
            }
        }

        public void OnSuccess()
        {
            RunOnUiThread(()=>
            {
                if(mOktaAppAuth.IsUserLoggedIn)
                {
                    Log.Info(Tag, "User is already authenticated, proceeding to token activity");
                    StartActivity(new Intent(this, typeof(UserInfoActivity)));
                    Finish();
                }
                else
                {
                    Log.Info(Tag, "Login activity setup finished");
                    displayAuthOptions();
                }
            });
        }

        public void OnTokenFailure(AuthorizationException ex)
        {
            RunOnUiThread(()=> {
                showMessage(GetString(Resource.String.init_failure)+":"+ ex.ErrorDescription);
            });
        }
    }
}