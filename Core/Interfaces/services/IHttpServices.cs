using System.Threading.Tasks;

namespace Core.Interfaces.services
{
    public interface IHttpServices
    {
        Task<string> LocalityName(decimal latitude, decimal longitude);
    }
}