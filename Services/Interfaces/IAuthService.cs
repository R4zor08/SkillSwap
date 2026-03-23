using SkillSwap.Models;

namespace SkillSwap.Services.Interfaces
{
    public interface IAuthService
    {
        Student? CurrentUser { get; }
        void Login(Student user);
        void Logout();
    }
}
