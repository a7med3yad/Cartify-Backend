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

        // Measure by Attribute Methods
        /// <summary>
        /// Gets all measures associated with a specific attribute by attribute ID
        /// </summary>
        /// <param name="attributeId">The ID of the attribute</param>
        /// <returns>List of measure names, or null if attribute not found</returns>
        Task<IEnumerable<string>> GetMeasuresByAttributeAsync(int attributeId);
        Task<bool> AddMeasureByAttributeAsync(string attributeName, string measureName);
    }
}

