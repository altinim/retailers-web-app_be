using Core.DTO;
using Core.Exceptions;
using Core.Interfaces;
using DB.Data;
using DB.Models;
using Expenses.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services {
    public class UserService : IUser {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher) {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> SignUp(SignUpDTO signUpDto) {
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(signUpDto.Email));

            if (checkUser != null) {
                throw new DuplicateEmailException("An account with this email address already exists");
            }

            User user = (User)signUpDto;

            if (!string.IsNullOrEmpty(user.Password)) {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            //Create User entity

            //Create Company for that User
            User newUser = await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == user.Id);

            return newUser;
        }
        public async Task<AuthenticatedEntity> SignIn(LoginDTO loginDTO) {

            var dbUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (dbUser == null || dbUser.Password == null || _passwordHasher.VerifyHashedPassword(dbUser, dbUser.Password, loginDTO.Password) == PasswordVerificationResult.Failed) {
                throw new InvalidUsernamePasswordException("Invalid email or password");
            }

            if (!dbUser.IsApproved) {
                throw new InvalidUsernamePasswordException("You have not been approved yet!");
            }
            return new AuthenticatedEntity()
            {
                Token = JwtGenerator.GenerateAuthToken(loginDTO.Email, dbUser.Role),

            };
        }

        public async Task<IEnumerable<User>> ReadUserAsync(bool? isApproved = null, int? pageNumber = null, int? pageSize = null) {
            try {
                var query = _context.Users.AsNoTracking();

                // Filter by approval status if the parameter is provided
                if (isApproved.HasValue) {
                    query = query.Where(user => user.IsApproved == isApproved.Value);
                }

                // Apply pagination
                if (pageNumber.HasValue && pageSize.HasValue) {
                    query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex) {
                throw new ApplicationException("Error occurred while retrieving users from the database.", ex);
            }
        }

        public async Task ApproveUser(Guid userId) {
            try {
                var userToUpdate = await _context.Users.FindAsync(userId);

                if (userToUpdate == null) {
                    throw new NotFoundException($"User with ID {userId} not found.");
                }

                userToUpdate.IsApproved = true;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw new ApplicationException("Error occurred while approving the user.", ex);
            }
        }

        public async Task<bool> DeleteUserAsync(string email) {
            try {
                var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (userToDelete == null) {
                    return false;
                }

                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) {
                throw new ApplicationException("Error deleting user", ex);
            }
        }
        public User GetUserById(Guid userId) {
            return _context.Users.Find(userId);
        }

        public void UpdateUserRole(Guid userId, string role) {
            var user = _context.Users.Find(userId);
            if (user != null) {
                user.Role = role;
                _context.SaveChanges();
            }
        }
    }
}
