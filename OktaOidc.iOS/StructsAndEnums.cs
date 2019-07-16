using ObjCRuntime;

namespace Okta.Oidc
{
    [Native]
    public enum OIDErrorCode : long
    {
        InvalidDiscoveryDocument = -2,
        UserCanceledAuthorizationFlow = -3,
        ProgramCanceledAuthorizationFlow = -4,
        NetworkError = -5,
        ServerError = -6,
        JSONDeserializationError = -7,
        TokenResponseConstructionError = -8,
        SafariOpenError = -9,
        BrowserOpenError = -10,
        TokenRefreshError = -11,
        RegistrationResponseConstructionError = -12,
        JSONSerializationError = -13,
        IDTokenParsingError = -14,
        IDTokenFailedValidationError = -15
    }

    [Native]
    public enum OIDErrorCodeOAuth : long
    {
        InvalidRequest = -2,
        UnauthorizedClient = -3,
        AccessDenied = -4,
        UnsupportedResponseType = -5,
        InvalidScope = -6,
        ServerError = -7,
        TemporarilyUnavailable = -8,
        InvalidClient = -9,
        InvalidGrant = -10,
        UnsupportedGrantType = -11,
        InvalidRedirectURI = -12,
        InvalidClientMetadata = -13,
        ClientError = -61439,
        Other = -61440
    }

    [Native]
    public enum OIDOIDErrorCodeOAuthAuthorization : long
    {
        InvalidRequest = OIDErrorCodeOAuth.InvalidRequest,
        UnauthorizedClient = OIDErrorCodeOAuth.UnauthorizedClient,
        AccessDenied = OIDErrorCodeOAuth.AccessDenied,
        UnsupportedResponseType = OIDErrorCodeOAuth.UnsupportedResponseType,
        AuthorizationInvalidScope = OIDErrorCodeOAuth.InvalidScope,
        ServerError = OIDErrorCodeOAuth.ServerError,
        TemporarilyUnavailable = OIDErrorCodeOAuth.TemporarilyUnavailable,
        ClientError = OIDErrorCodeOAuth.ClientError,
        Other = OIDErrorCodeOAuth.Other
    }

    [Native]
    public enum OIDOIDErrorCodeOAuthToken : long
    {
        InvalidRequest = OIDErrorCodeOAuth.InvalidRequest,
        InvalidClient = OIDErrorCodeOAuth.InvalidClient,
        InvalidGrant = OIDErrorCodeOAuth.InvalidGrant,
        UnauthorizedClient = OIDErrorCodeOAuth.UnauthorizedClient,
        UnsupportedGrantType = OIDErrorCodeOAuth.UnsupportedGrantType,
        InvalidScope = OIDErrorCodeOAuth.InvalidScope,
        ClientError = OIDErrorCodeOAuth.ClientError,
        Other = OIDErrorCodeOAuth.Other
    }

    [Native]
    public enum OIDOIDErrorCodeOAuthRegistration : long
    {
        InvalidRequest = OIDErrorCodeOAuth.InvalidRequest,
        InvalidRedirectURI = OIDErrorCodeOAuth.InvalidRedirectURI,
        InvalidClientMetadata = OIDErrorCodeOAuth.InvalidClientMetadata,
        ClientError = OIDErrorCodeOAuth.ClientError,
        Other = OIDErrorCodeOAuth.Other
    }
}