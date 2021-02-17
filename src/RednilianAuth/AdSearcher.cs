using RednilianAuth.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace RednilianAuth
{
    public class AdSearcher
    {

        /// <summary>
        /// <para>Get List of RednilianAuth.AdPerson from Active Directory matching the provided firstName (givenName) argument.</para>
        /// <para>Workstation Must be on the domain, otherwise an empty list is returned. </para>
        /// </summary>
        /// <param name="firstName">First Name ('givenName') of the user.</param>
        /// <param name="domainName">Domain Name</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static List<AdPerson> GetAdUsersbyFirstName(string firstName, string domainName)
        {
            List<DirectoryEntry> dirEntries = new List<DirectoryEntry>();

            List<AdPerson> adPersons = new();
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, domainName))
                {

                    UserPrincipal userPrincipal = new UserPrincipal(context);

                    userPrincipal.Surname = firstName;

                    PrincipalSearcher principalSearcher = new PrincipalSearcher(userPrincipal);

                    foreach (var principalSearchResult in principalSearcher.FindAll())
                    {
                        DirectoryEntry directoryEntry = principalSearchResult.GetUnderlyingObject() as DirectoryEntry;
                        dirEntries.Add(directoryEntry);
                    }


                    foreach (var d in dirEntries.Where(x => x.Properties["givenName"].Value != null))
                    {
                        adPersons.Add(new AdPerson
                        {
                            AdLogin = d.Properties["samAccountName"].Value.ToString(),
                            FirstName = d.Properties["givenName"].Value.ToString(),
                            LastName = d.Properties["sn"].Value.ToString(),
                            Email = d.Properties["mail"].Value.ToString()
                        });

                    }

                }
            }
            catch (Exception e)
            {
                _ = e.ToString();
            }
            return adPersons.OrderBy(f => f.FirstName).OrderBy(l => l.LastName).ToList();

        }


        /// <summary>
        /// <para>Get List of RednilianAuth.AdPerson from Active Directory matching the provided LastName (surname) argument.</para>
        /// <para>Workstation Must be on the domain, otherwise an empty list is returned. </para>
        /// </summary>
        /// <param name="lastName">Last Name ('sn' Surname) of the user.</param>
        /// <param name="domainName">Domain Name</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static List<AdPerson> GetAdUsersbyLastName(string lastName, string domainName)
        {            
            List<DirectoryEntry> dirEntries = new List<DirectoryEntry>();

            List<AdPerson> adPersons = new();
            try {
                using (var context = new PrincipalContext(ContextType.Domain, domainName)) {

                    UserPrincipal userPrincipal = new UserPrincipal(context);

                    userPrincipal.Surname = lastName;

                    PrincipalSearcher principalSearcher = new PrincipalSearcher(userPrincipal);

                    foreach (var principalSearchResult in principalSearcher.FindAll()) {
                        DirectoryEntry directoryEntry = principalSearchResult.GetUnderlyingObject() as DirectoryEntry;
                        dirEntries.Add(directoryEntry);
                    }


                    foreach (var d in dirEntries.Where(x => x.Properties["sn"].Value != null)) {
                        adPersons.Add(new AdPerson
                        {
                            AdLogin = d.Properties["samAccountName"].Value.ToString(),
                            FirstName = d.Properties["givenName"].Value.ToString(),
                            LastName = d.Properties["sn"].Value.ToString(),
                            Email = d.Properties["mail"].Value.ToString()
                        });

                    }
                    
                }
            }
            catch (Exception e) {
                _ = e.ToString();
            }
            return adPersons.OrderBy(l=>l.LastName).OrderBy(f=>f.FirstName).ToList();

        }

    }
}
