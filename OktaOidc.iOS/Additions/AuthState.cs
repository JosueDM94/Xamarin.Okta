using UIKit;

namespace OktaOidc
{
    partial class AuthState
    {
        public static IAuthorizationFlowSession PresentAuthorizationRequest(OIDAuthorizationRequest authorizationRequest, UIViewController presentingViewController, OIDAuthStateAuthorizationCallback callback)
        {
            var coordinator = new AuthorizationUICoordinatorIOS(presentingViewController);
            return AuthState.PresentAuthorizationRequest(authorizationRequest, coordinator, callback);
        }
    }

    partial class AuthorizationService
    {
        public static IAuthorizationFlowSession PresentAuthorizationRequest(OIDAuthorizationRequest request, UIViewController presentingViewController, OIDAuthorizationCallback callback)
        {
            var coordinator = new AuthorizationUICoordinatorIOS(presentingViewController);
            return AuthorizationService.PresentAuthorizationRequest(request, coordinator, callback);
        }
    }
}
