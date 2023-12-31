﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Models.Entities
{
    [Index(nameof(Email), IsUnique = true)]

    public class AdminEntity : IPersonInterface
    {
        public AdminEntity(
            string firstName, 
            string lastName, 
            string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Required, Column(TypeName = "nvarchar(50)", Order = 1)]
        public string FirstName { get; set; } = null!;

        [Required, Column(TypeName = "nvarchar(50)", Order = 2)]
        public string LastName { get; set; } = null!;

        [Required, Column(TypeName = "nvarchar(100)", Order = 3)]
        public string Email { get; set; } = null!;


        [Required, Column(Order = 4)]
        public RolesPolicy RolesPolicy { get; set; } = 0!;


        [Required, Column(Order = 5)]
        public byte[] PasswordHash { get; private set; } = null!;

        [Required, Column(Order = 6)]
        public byte[] Security { get; private set; } = null!;


        public void CreatePassword(string password)
        {
            using var hmac = new HMACSHA512();
            Security = hmac.Key;
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool MatchPassword(string password)
        {
            using (var hmac = new HMACSHA512(Security))
            {
                var _hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password.Trim()));

                for (int i = 0; i < _hash.Length; i++)
                {
                    if (_hash[i] != PasswordHash[i])
                        return false;
                }

                return true;
            }
        }
    }
}
