using SkillSwap.Models;

namespace SkillSwap.Services.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(Student student);
}
