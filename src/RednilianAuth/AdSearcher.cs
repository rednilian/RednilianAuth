using RednilianAuth.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RednilianAuth
{
    public class AdSearcher
    {

        /// <summary>
        /// Get List RednilianAuth.AdPerson from Active Directory matching the provided LastName (surname) argument.
        /// </summary>
        /// <param name="lastName">Last Name (Surname) of the user.</param>
        /// <param name="domainName"></param>
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
                            LastName = d.Properties["givenName"].Value.ToString(),
                            Email = d.Properties["mail"].Value.ToString()
                        });

                    }
                    
                }
            }
            catch (Exception e) {
                _ = e.ToString();
                throw;
            }
            return adPersons.OrderBy(l=>l.LastName).OrderBy(f=>f.FirstName).ToList();

        }

    }
}
