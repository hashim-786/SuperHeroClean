using SuperHeroAPI.Core.Entities;

namespace SuperHeroAPI.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
