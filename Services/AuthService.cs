using SkillSwap.Models;
using SkillSwap.Services.Interfaces;

namespace SkillSwap.Services
{
    public class AuthService : IAuthService
    {
        private Student? _currentUser;
        public Student? CurrentUser => _currentUser;

        public void Login(Student user)
        {
            _currentUser = user;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}
