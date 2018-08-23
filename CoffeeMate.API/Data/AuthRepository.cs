using System;
using System.Threading.Tasks;
using CoffeeMate.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMate.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string pwd)
        {
            // returns a user that matches or returns null.
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == username);
            if(user == null) return null;
            if(!VerfiedPasswordHash(pwd, user.PasswordHash, user.PasswordSalt)) return null;

            return user;
        }

        private bool VerfiedPasswordHash(string pwd, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                passwordSalt = hmac.Key;
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd));
                // loop through hash array and check if it's not equal return false
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string pwd)
        {
            // passes data back as ref and updates the values.
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(pwd, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordHash = passwordSalt;
            
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string pwd, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd));
            }
            
        }

        public async Task<bool> UserExists(string username)
        {   // check to compare the username against any username in the db
            if(await _context.User.AnyAsync(u => u.UserName == username))
            {
                return true;
            }
            return false;
        }
    }
}