using System;
using System.Collections.Generic;
using System.Text;

namespace RednilianAuth.Models
{
    /// <summary>
    /// <para>Class representing a 'Person' with info obtained from Active Directory.</para> 
    /// <para>Contains basic info Properties FirstName, LastName, Email, DisplayName, samAccountName.</para>
    /// </summary>
    public class AdPerson
    {
        public AdPerson()
        {
            AdLogin = string.Empty;
            Email = string.Empty;
            FirstName = string.Empty;
            DisplayName = string.Empty;
            LastName = string.Empty;
            IsAuthenticated = false;
        }


        /// <summary>
        /// SAMAccountName
        /// </summary>
        public string AdLogin { get; set; }
        public string DisplayName { get; set; }

        /// <summary>
        /// mail
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// givenName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// If Authentication was attempted, returns the results True of False
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// sn  (surname)
        /// </summary>
        public string LastName { get; set; }

    }
}
