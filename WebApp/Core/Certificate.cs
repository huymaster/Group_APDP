using System.Security.Cryptography.X509Certificates;

namespace WebApp.Core;

public static class Certificate
{
    public static X509Certificate2? GetCertificate(string? thumbprint)
    {
        thumbprint ??= string.Empty;
        using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        store.Open(OpenFlags.ReadOnly);
        var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
        return certificates.FirstOrDefault();
    }
}