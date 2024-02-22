using Core.DTO;
using DB.Models;

namespace Core.Interfaces {
    public interface IUser {
        Task<User> SignUp(SignUpDTO user);
        Task<AuthenticatedEntity> SignIn(LoginDTO user);
        Task<IEnumerable<User>> ReadUserAsync(bool? isApproved, int? pageNumber = null, int? pageSize = null);
        Task ApproveUser(Guid userId);
        Task<bool> DeleteUserAsync(string email);
        User GetUserById(Guid userId);
        void UpdateUserRole(Guid userId, string role);
    }
}
