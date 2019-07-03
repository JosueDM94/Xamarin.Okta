using Android.OS;
using Android.App;
using Android.Text;
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
    public class SessionAuthorizeActivity : AppCompatActivity
    {
        private static string Tag = "SessionAuthActivity";
        public OktaAppAuth mOktaAppAuth;

        LinearLayout mContainer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mOktaAppAuth = OktaAppAuth.GetInstance(this);
            SetContentView(Resource.Layout.activity_session_login);
            FindViewById(Resource.Id.start_auth).Click += (sender, args) => { startAuth(); };
            mContainer = FindViewById<LinearLayout>(Resource.Id.auth_container);
        }

        protected override void OnStart()
        {
            base.OnStart();
            initializeOktaAuth();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void initializeOktaAuth()
        {
            Log.Info(Tag, "Initializing OktaAppAuth");
            displayLoading(GetString(Resource.String.loading_initializing));

            mOktaAppAuth.Init(this, new OktaAuthListener(this));
        }

        private void startAuth()
        {
            displayLoading(GetString(Resource.String.loading_authorizing));

            string sessionToken = ((EditText)FindViewById(Resource.Id.session_token_value)).Text;

            if (TextUtils.IsEmpty(sessionToken))
            {
                showMessage(GetString(Resource.String.empty_login_or_password));
                return;
            }

            mOktaAppAuth.Authenticate(sessionToken, new OktaNativeAuthListener(this));
        }

        public void displayLoading(string loadingMessage)
        {
            FindViewById(Resource.Id.loading_container).Visibility = ViewStates.Visible;
            mContainer.Visibility = ViewStates.Gone;

            ((TextView)FindViewById(Resource.Id.loading_description)).Text = loadingMessage;
        }

        public void displayAuthOptions()
        {
            mContainer.Visibility = ViewStates.Visible;

            FindViewById(Resource.Id.loading_container).Visibility = ViewStates.Gone;
        }

        public void showMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        public class OktaNativeAuthListener : Java.Lang.Object, OktaAppAuth.IOktaNativeAuthListener
        {
            private SessionAuthorizeActivity Activity;

            public OktaNativeAuthListener(SessionAuthorizeActivity activity)
            {
                Activity = activity;
            }

            public void OnSuccess()
            {
                Activity.RunOnUiThread(()=> {
                    Activity.StartActivity(new Intent(Activity, typeof(UserInfoActivity)));
                    Activity.Finish();
                });
            }

            public void OnTokenFailure(AuthenticationError ex)
            {
                Activity.RunOnUiThread(()=>
                {
                    Activity.showMessage(ex.Message);
                    Activity.displayAuthOptions();
                });
            }
        }

        public class OktaAuthListener : Java.Lang.Object, OktaAppAuth.IOktaAuthListener
        {
            private SessionAuthorizeActivity Activity;

            public OktaAuthListener(SessionAuthorizeActivity activity)
            {
                Activity = activity;
            }

            public void OnSuccess()
            {
                Activity.RunOnUiThread(()=>
                {
                    if (Activity.mOktaAppAuth.IsUserLoggedIn)
                    {
                        Log.Info(Tag, "User is already authenticated, proceeding to token activity");
                        Activity.StartActivity(new Intent(Activity, typeof(UserInfoActivity)));
                        Activity.Finish();
                    }
                    else
                    {
                        Log.Info(SessionAuthorizeActivity.Tag,"Login activity setup finished");
                        Activity.displayAuthOptions();
                    }
                });
            }

            public void OnTokenFailure(AuthorizationException ex)
            {
                Activity.RunOnUiThread(() =>
                {
                    Activity.showMessage(Activity.GetString(Resource.String.init_failure)+ ":"+ ex.ErrorDescription);
                });
            }
        }
    }
}
