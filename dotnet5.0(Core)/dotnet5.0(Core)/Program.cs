using System;
using System.Diagnostics;
using System.Collections.Generic;

using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

using Amazon.SSO;
using Amazon.SSO.Model;
using Amazon.SSOOIDC;
using Amazon.SSOOIDC.Model;

using (var ssoOidcClient = new AmazonSSOOIDCClient(new AnonymousAWSCredentials(), new AmazonSSOOIDCConfig { RegionEndpoint = RegionEndpoint.USWest2 })) //Change your AWS region
{
    var registerClientResponse = await ssoOidcClient.RegisterClientAsync(new RegisterClientRequest
    {
        ClientName = "athena_sso_windows",
        ClientType = "public"
    });

    var startDeviceAuthResponse = await ssoOidcClient.StartDeviceAuthorizationAsync(new StartDeviceAuthorizationRequest
    {
        ClientSecret = registerClientResponse.ClientSecret,
        ClientId = registerClientResponse.ClientId,
        StartUrl = "YourURL_Enter_Here!" //Change your URL
    });

    Process.Start(new ProcessStartInfo { FileName = startDeviceAuthResponse.VerificationUriComplete, UseShellExecute = true });

    DateTime start_timer = DateTime.Now;
    string accessToken;

    while (true)
    {
        try
        {
            var createTokenResponse = await ssoOidcClient.CreateTokenAsync(new CreateTokenRequest
            {
                ClientId = registerClientResponse.ClientId,
                ClientSecret = registerClientResponse.ClientSecret,
                GrantType = "urn:ietf:params:oauth:grant-type:device_code",
                DeviceCode = startDeviceAuthResponse.DeviceCode,
            });

            accessToken = createTokenResponse.AccessToken;
            break;
        }
        catch (AuthorizationPendingException ex)
        {
            if ((DateTime.Now - start_timer).TotalSeconds < startDeviceAuthResponse.ExpiresIn)
            {
                System.Threading.Thread.Sleep(5);
            }
            else
            {
                throw ex;
            }
        }
    }

    using (var ssoClient = new AmazonSSOClient(new AnonymousAWSCredentials(), new AmazonSSOConfig { RegionEndpoint = RegionEndpoint.USWest2 }))
    {
        List<string> accountID = new List<string> { "XXX", "YYY" }; //Change your accountID

        foreach (string ID in accountID)
        {
            var listAccountRolesResponse = await ssoClient.ListAccountRolesAsync(new ListAccountRolesRequest
            {
                AccountId = ID,
                AccessToken = accessToken
            });

            foreach (var item in listAccountRolesResponse.RoleList)
            {
                var getRoleCredentialsResponse = await ssoClient.GetRoleCredentialsAsync(new GetRoleCredentialsRequest
                {
                    AccessToken = accessToken,
                    AccountId = ID,
                    RoleName = item.RoleName
                });

                WriteCredentials(item.RoleName, getRoleCredentialsResponse.RoleCredentials.AccessKeyId, getRoleCredentialsResponse.RoleCredentials.SecretAccessKey, getRoleCredentialsResponse.RoleCredentials.SessionToken);
            }
        }

        void WriteCredentials(string profileName, string keyId, string secret, string sessionToken)
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = keyId,
                SecretKey = secret,
                Token = sessionToken
            };

            var profile = new CredentialProfile(profileName, options);
            SharedCredentialsFile sharedFile = new SharedCredentialsFile();
            sharedFile.RegisterProfile(profile);
        }
    }
}