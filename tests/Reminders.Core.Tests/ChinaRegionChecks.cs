using Reminders.Core;
using Reminders.Windows.Services;

internal static class ChinaRegionChecks
{
    public static void Run(Action<bool, string> check)
    {
        check(new Uri(AppleSignInPolicy.StartUrl).Host == "www.icloud.com.cn", "China build starts on the China website");
        foreach (var host in new[] { "icloud.com.cn", "setup.icloud.com.cn", "p01-ckdatabasews.icloud.com.cn" })
        {
            check(AppleAuth.AppleDomain(host), "China session cookies are retained: " + host);
            check(AppleSignInPolicy.IsAppleUri(new Uri("https://" + host)), "China navigation accepted: " + host);
        }
        foreach (var host in new[] { "icloud.com.cn.evil.example", "evilicloud.com.cn", "icloud.com", "setup.icloud.com" })
        {
            check(!AppleAuth.AppleDomain(host), "unrelated cookies rejected: " + host);
            check(!AppleSignInPolicy.IsAppleUri(new Uri("https://" + host)), "unrelated navigation rejected: " + host);
        }
        using var auth = new AppleAuth();
        using var request = auth.Request(HttpMethod.Post, "https://setup.icloud.com.cn/setup/ws/1/validate", null, false);
        check(request.Headers.GetValues("Origin").Single() == "https://www.icloud.com.cn", "China service requests carry the China origin");
        check(request.Headers.Referrer?.Host == "www.icloud.com.cn", "China service requests carry the China referrer");
        using var signIn = auth.Request(HttpMethod.Get, AppleAuth.AuthEndpoint, null, true);
        check(signIn.Headers.GetValues("x-apple-oauth-redirect-uri").Single() == "https://www.icloud.com.cn", "OAuth redirects back to China");
        check(AppleSignInPolicy.Stage(new Uri("https://setup.icloud.com.cn/setup/ws/1/validate")) == "account", "China validation is recognized for browser handoff");
    }
}
