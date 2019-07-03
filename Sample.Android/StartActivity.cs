using Android.OS;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Content.PM;
using Android.Support.V7.App;

namespace Sample.Android
{
    [Activity(Label = "@string/app_name_short", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance, WindowSoftInputMode = SoftInput.StateHidden)]
    public class StartActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_start);

            FindViewById(Resource.Id.auth_browser_flow).Click += (sender, args) =>  {
                StartActivity(new Intent(this, typeof(LoginActivity)));
            };

            FindViewById(Resource.Id.auth_session_token_flow).Click += (sender, args) => {
                StartActivity(new Intent(this, typeof(SessionAuthorizeActivity)));
            };
        }
    }
}