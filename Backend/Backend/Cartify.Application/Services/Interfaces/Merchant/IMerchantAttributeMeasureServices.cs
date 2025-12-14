using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Interfaces.Merchant
{
    public interface IMerchantAttributeMeasureServices
    {
        // Attribute Methods
        Task<IEnumerable<string>> GetAttributesAsync();
        Task<bool> GetAttributeAsync(string name);
        Task<bool> AddAttributeAsync(string name);

        // Measure Methods
        Task<IEnumerable<string>> GetMeasuresAsync();
        Task<bool> GetMeasureAsync(string name);
        Task<bool> AddMeasureAsync(string name);


    }
}

