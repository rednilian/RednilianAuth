using RednilianAuth.Models;
using System;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;


namespace RednilianAuth
{
    public class AdAuthenticator
    {


        #region -----------------   FIELDS  -----------------------------

        #endregion



        #region -----------------   CONSTRUCTORS    -----------------------------

        #endregion



        #region -----------------   PROPERTIES  -----------------------------

        #endregion



        #region -----------------   METHODS -----------------------------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public  AdPerson AuthenticateAndAdPerson(string ldapUrl, string ldapDomain, string userName, string password)
        {
            AdPerson adPerson = new();
            var didAuthenticate = IsAuthenticated(ldapUrl, ldapDomain, userName, password);
            if (didAuthenticate is false)
            {
                return adPerson;
            }
            adPerson.IsAuthenticated = true;
            adPerson.AdLogin = userName;

            //  Authenticated against ActiveDirectory, so proceed to get name/email, etc.:
            DirectoryEntry dirEntry = new DirectoryEntry(ldapUrl, $"{ldapDomain}\\{userName}", password);
           
            try
            {
                DirectorySearcher dirSearcher = new DirectorySearcher(dirEntry)
                {
                    Filter = $"(SAMAccountName={userName})"
                };
                dirSearcher.PropertiesToLoad.Add("givenName");
                dirSearcher.PropertiesToLoad.Add("sn");
                dirSearcher.PropertiesToLoad.Add("DisplayName");
                dirSearcher.PropertiesToLoad.Add("mail");
                SearchResult result = dirSearcher.FindOne();
                if (result == null)
                {
                    return adPerson;
                }
                adPerson.FirstName = result.Properties["sn"][0] != null ? Convert.ToString(result.Properties["givenName"][0]) : "?";
                adPerson.LastName = result.Properties["sn"][0] != null ? Convert.ToString(result.Properties["sn"][0]) : "?";
                adPerson.DisplayName = result.Properties["sn"][0] != null ? Convert.ToString(result.Properties["DisplayName"][0]) : "?";
                adPerson.Email = result.Properties["sn"][0] != null ? Convert.ToString(result.Properties["mail"][0]) : "?";
            }
            catch (Exception e)
            {
                _ = e.ToString();
                adPerson.FirstName = string.Empty;
                adPerson.LastName = string.Empty;
                adPerson.DisplayName = string.Empty;
                adPerson.Email = string.Empty;
                return adPerson;
            }
            return adPerson;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static bool IsAuthenticated(string ldapUrl, string ldapDomain, string userName, string password)
        {
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, ldapDomain, userName, password))
                {
                    return context.ValidateCredentials(userName, password);
                }
            }
            catch (Exception e)
            {
                var error = e.ToString();
                return false;
            }
        }


        #endregion




    }
}
