using System.Threading.Tasks;

namespace FirstCall.Application.Interfaces.Services
{
    public interface IDatabaseSeeder
    {
        Task Initialize();

    }
}