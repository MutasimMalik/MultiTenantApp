using Core.Contracts;
using System;

namespace Core.Entities
{
    public class Registration : BaseEntity, ITenant
    {
        public Registration(string name, string email, DateTime dateOfBirth)
        {
            Name = name;
            Email = email;  
            DateOfBirth = dateOfBirth;
        }

        protected Registration()
        {

        }

        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TenantId { get; set; }

    }
}
