﻿namespace WebAPI.Models.Interfaces
{
    public interface IPersonInterface
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
   
    }

    public enum RolesPolicy
    {
        Admin = 0,
        User = 1,
    }
} 
 