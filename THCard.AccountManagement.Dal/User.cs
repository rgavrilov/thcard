//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace THCard.AccountManagement.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Accounts = new HashSet<Account>();
        }
    
        public System.Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
