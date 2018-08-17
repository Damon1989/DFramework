using System;
using System.Security.Cryptography.X509Certificates;

namespace Dframework.SingleSignOn.IdentityProvider
{
    public class CertificateUtil
    {
        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, string subjectName)
        {
            var store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                X509Certificate2 result = null;
                certificates = store.Certificates;

                foreach (var cert in certificates)
                {
                    if (cert.SubjectName.Name == null || !string.Equals(cert.SubjectName.Name, subjectName,
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    if (result != null)
                    {
                        throw new ApplicationException($"subject Name {subjectName} 存在多个证书");
                    }

                    result = new X509Certificate2(cert);
                }

                if (result == null)
                {
                    throw new ApplicationException($"没有找到用于 subject Name {subjectName} 的证书");
                }

                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    foreach (var item in certificates)
                    {
                        item.Reset();
                    }
                }

                store.Close();
            }
        }
    }
}