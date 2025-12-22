using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid UserId);

        Task<User> GetUserByEmailAsync(string email);

        Task<User> CreateUserAsync(User user);

        Task<User> UpdateUserAsync(User user);

        Task<bool> UpdateUserContactInfoAsync(Guid userId, string phoneNumber, string address, string city, string country);

    }
}
