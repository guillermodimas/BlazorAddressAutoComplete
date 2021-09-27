using BlazorAddressAutoComplete.Models;
using System.Threading.Tasks;

namespace BlazorAddressAutoComplete.Services
{
    public interface IAzureMapsService
    {
        Task<ResultsAutoComplete> AutocompleteAddress(string Address);
     
    }
}