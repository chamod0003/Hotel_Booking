using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Models.Entity;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Application_Layer
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

       

        public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users
               .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            // Verify password (stored in `Password` field as hashed value)
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }

            // Generate token
            var token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FullName = user.FullName
                }
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var existingUser = await _context.Users
                           .FirstOrDefaultAsync(u => u.Email == registerRequest.Email);

            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            // Create new user
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = registerRequest.Email,
                Password = passwordHash,
                FullName = registerRequest.FullName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate token
            var token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FullName = user.FullName
                }
            };
        }
    }
}
