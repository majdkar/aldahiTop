using FirstCall.Shared.Settings;
using System.Threading.Tasks;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}