﻿using RednilianAuth.Models;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;


namespace RednilianAuth
{
    public static class AdAuthenticator
    {


        #region -----------------   FIELDS  -----------------------------

        #endregion



        #region -----------------   CONSTRUCTORS    -----------------------------

        #endregion



        #region -----------------   PROPERTIES  -----------------------------

        #endregion



        #region -----------------   METHODS -----------------------------
        /// <summary>
        /// Checks if provided credentials authenticate with LDAP, and returns an 'AdPerson' class object.
        /// </summary>
        /// <param name="ldapUrl">Url for LDAP</param>
        /// <param name="ldapDomain">Domain for LDAP</param>
        /// <param name="userName">samAccountName</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public  static AdPerson AuthenticateAndGetAdPerson(string ldapUrl, string ldapDomain, string userName, string password)
        {
            AdPerson adPerson = new();
            var didAuthenticate = IsAuthenticated(ldapDomain, userName, password);
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


        /// <summary>
        /// Returns true if provided username/password are PrincipalContext.ValidCredentials
        /// </summary>
        /// <param name="ldapUrl">Url for LDAP</param>
        /// <param name="ldapDomain">Domain for LDAP</param>
        /// <param name="userName">samAccountName</param>
        /// <param name="password"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static bool IsAuthenticated(string ldapDomain, string userName, string password)
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
                _ = e.ToString();
                return false;
            }
        }


        #endregion




    }
}
