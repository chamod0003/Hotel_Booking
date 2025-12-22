using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var result = await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var result = await appDbContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return result;
        }

        public async Task<User> GetUserByIdAsync(Guid UserId)
        {
            var result = await appDbContext.Users
                .Include(u=>u.Bookings)
                .FirstOrDefaultAsync(u => u.UserId == UserId);
            return result;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var result = await appDbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);
            return result;
        }

        public async Task<bool> UpdateUserContactInfoAsync(Guid userId, string phoneNumber, string address, string city, string country)
        {
            var user = await appDbContext.Users.FindAsync(userId);
            if (user == null) return false;

            user.PhoneNumber = phoneNumber;
            user.Address = address;
            user.City = city;
            user.Country = country;

            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
