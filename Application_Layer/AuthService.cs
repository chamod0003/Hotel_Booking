using Application_Layer.DTO;
using Application_Layer.Interface;
using Azure.Core;
using Domain_Layer.Models.Entity;
using Google.Apis.Auth;
using Infrastructure_Layer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Application_Layer
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, ITokenService tokenService,IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<AuthResponse> GoogleLoginAsync(string idToken)
        {
            try
            {
                // Verify Google token
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    idToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                    });

                // Check if user exists
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == payload.Email);

                if (user == null)
                {
                    // Create new user from Google data
                    user = new User
                    {
                        UserId = Guid.NewGuid(),
                        Email = payload.Email,
                        FullName = payload.Name,
                        ProfilePicture = payload.Picture,
                        OAuthProvider = "Google",
                        OAuthId = payload.Subject,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        Password = null  
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Update existing user
                    user.ProfilePicture = payload.Picture;

                    // If user registered with email/password before, link Google account
                    if (string.IsNullOrEmpty(user.OAuthProvider))
                    {
                        user.OAuthProvider = "Google";
                        user.OAuthId = payload.Subject;
                    }

                    await _context.SaveChangesAsync();
                }

                // Generate JWT token
                var token = _tokenService.GenerateToken(user);

                return new AuthResponse
                {
                    Token = token,
                    User = new UserDto
                    {
                        UserId = user.UserId,
                        Email = user.Email,
                        FullName = user.FullName,
                        ProfilePicture = user.ProfilePicture,
                        OAuthProvider = user.OAuthProvider
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Google authentication failed: {ex.Message}");
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create new user
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                Password = passwordHash,
                FullName = request.FullName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OAuthProvider = null,  // Regular email/password user
                OAuthId = null
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
                    FullName = user.FullName,
                    ProfilePicture = user.ProfilePicture,
                    OAuthProvider = user.OAuthProvider
                }
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            // Check if user registered with OAuth only
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new Exception("This account uses Google login. Please sign in with Google.");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }

            // Update last login
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
                    FullName = user.FullName,
                    ProfilePicture = user.ProfilePicture,
                    OAuthProvider = user.OAuthProvider
                }
            };
        }


    }
}
