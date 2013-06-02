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
    
    public partial class Account
    {
        public Account()
        {
            this.Roles = new HashSet<Role>();
        }
    
        public System.Guid AccountId { get; set; }
        public string Username { get; set; }
        public System.Guid UserId { get; set; }
    
        public virtual ICollection<Role> Roles { get; set; }
        public virtual AccountPassword AccountPassword { get; set; }
        public virtual User User { get; set; }
        public virtual FailedLoginAttempt FailedLoginAttempt { get; set; }
    }
}
