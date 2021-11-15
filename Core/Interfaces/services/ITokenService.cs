using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interfaces.services
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}