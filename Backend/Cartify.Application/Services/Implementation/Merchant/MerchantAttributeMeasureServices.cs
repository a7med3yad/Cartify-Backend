using Cartify.Application.Services.Implementation.Helper;
using Cartify.Application.Services.Interfaces.Merchant;
using Cartify.Domain.Interfaces.Repositories;
using Cartify.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cartify.Application.Services.Implementation.Merchant
{
    public class MerchantAttributeMeasureServices : IMerchantAttributeMeasureServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GetUserServices _getUserServices;

        public MerchantAttributeMeasureServices(
            IUnitOfWork unitOfWork,
            GetUserServices getUserServices)
        {
            _unitOfWork = unitOfWork;
            _getUserServices = getUserServices;
        }

        // =========================================================
        // 🔹 ATTRIBUTE METHODS
        // =========================================================

        /// <summary>
        /// Gets all non-deleted attributes
        /// </summary>
        public async Task<IEnumerable<string>> GetAttributesAsync()
        {
            var attributes = await _unitOfWork.AttributeRepository.ReadAsync();
            return attributes
                .Where(a => !a.IsDeleted)
                .OrderBy(a => a.Name)
                .Select(a => a.Name)
                .ToList();
        }

        /// <summary>
        /// Checks if an attribute exists by name
        /// </summary>
        public async Task<bool> GetAttributeAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var attribute = await _unitOfWork.AttributeRepository
                .Search(a => a.Name.ToLower() == name.ToLower().Trim() && !a.IsDeleted);

            return attribute != null;
        }

        /// <summary>
        /// Adds a new attribute
        /// </summary>
        public async Task<bool> AddAttributeAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Check if attribute already exists
            var existingAttribute = await _unitOfWork.AttributeRepository
                .Search(a => a.Name.ToLower() == name.ToLower().Trim() && !a.IsDeleted);

            if (existingAttribute != null)
                return false;

            var merchantId = _getUserServices.GetMerchantIdFromToken();
            int.TryParse(merchantId, out int merchantIdInt);

            var attribute = new lkpAttribute
            {
                Name = name.Trim(),
                CreatedBy = merchantIdInt,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.AttributeRepository.CreateAsync(attribute);
            return await _unitOfWork.SaveChanges() > 0;
        }

        // =========================================================
        // 🔹 MEASURE METHODS
        // =========================================================

        /// <summary>
        /// Gets all non-deleted measures
        /// </summary>
        public async Task<IEnumerable<string>> GetMeasuresAsync()
        {
            var measures = await _unitOfWork.MeasureUnitRepository.ReadAsync();
            return measures
                .Where(m => !m.IsDeleted)
                .OrderBy(m => m.Name)
                .Select(m => m.Name)
                .ToList();
        }

        /// <summary>
        /// Checks if a measure exists by name
        /// </summary>
        public async Task<bool> GetMeasureAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var measure = await _unitOfWork.MeasureUnitRepository
                .Search(m => m.Name.ToLower() == name.ToLower().Trim() && !m.IsDeleted);

            return measure != null;
        }

        /// <summary>
        /// Adds a new measure
        /// </summary>
        public async Task<bool> AddMeasureAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Check if measure already exists
            var existingMeasure = await _unitOfWork.MeasureUnitRepository
                .Search(m => m.Name.ToLower() == name.ToLower().Trim() && !m.IsDeleted);

            if (existingMeasure != null)
                return false;

            var merchantId = _getUserServices.GetMerchantIdFromToken();
            int.TryParse(merchantId, out int merchantIdInt);

            var measure = new LkpMeasureUnite
            {
                Name = name.Trim(),
                CreatedBy = merchantIdInt,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            await _unitOfWork.MeasureUnitRepository.CreateAsync(measure);
            return await _unitOfWork.SaveChanges() > 0;
        }

        // =========================================================
        // 🔹 MEASURE BY ATTRIBUTE METHODS
        // =========================================================

        /// <summary>
        /// Gets all measures associated with a specific attribute by attribute ID
        /// </summary>
        /// <param name="attributeId">The ID of the attribute</param>
        /// <returns>
        /// List of measure names associated with the attribute.
        /// Returns null if attribute not found.
        /// Returns empty list if attribute exists but has no measures.
        /// </returns>
        public async Task<IEnumerable<string>> GetMeasuresByAttributeAsync(int attributeId)
        {
            // Validate input
            if (attributeId <= 0)
                return null;

            // Find the attribute by ID
            var attribute = await _unitOfWork.AttributeRepository.ReadByIdAsync(attributeId);

            // Check if attribute exists and is not deleted
            if (attribute == null || attribute.IsDeleted)
                return null;

            // Get all product detail attributes that use this attribute ID
            // Query directly using AttributeId instead of name matching for better performance
            var query = _unitOfWork.ProductDetailsRepository
                .GetAllIncluding2(
                    pd => pd.LkpProductDetailsAttributes,
                    pd => pd.LkpProductDetailsAttributes.Select(a => a.MeasureUnit),
                    pd => pd.LkpProductDetailsAttributes.Select(a => a.Attribute)
                )
                .Where(pd => !pd.IsDeleted)
                .SelectMany(pd => pd.LkpProductDetailsAttributes)
                .Where(pda => pda.AttributeId == attributeId 
                           && pda.MeasureUnit != null 
                           && !pda.MeasureUnit.IsDeleted)
                .Select(pda => pda.MeasureUnit.Name)
                .Distinct();

            var measures = await query.ToListAsync();
            
            // Return empty list if no measures found (attribute exists but has no measures)
            return measures ?? new List<string>();
        }

        /// <summary>
        /// Adds a measure to an attribute (creates association through product details)
        /// Note: This method ensures the measure exists and can be used with the attribute
        /// </summary>
        public async Task<bool> AddMeasureByAttributeAsync(string attributeName, string measureName)
        {
            if (string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(measureName))
                return false;

            // Find the attribute
            var attribute = await _unitOfWork.AttributeRepository
                .Search(a => a.Name.ToLower() == attributeName.ToLower().Trim() && !a.IsDeleted);

            if (attribute == null)
                return false;

            // Find or create the measure
            var measure = await _unitOfWork.MeasureUnitRepository
                .Search(m => m.Name.ToLower() == measureName.ToLower().Trim() && !m.IsDeleted);

            if (measure == null)
            {
                // Create the measure if it doesn't exist
                var merchantId = _getUserServices.GetMerchantIdFromToken();
                int.TryParse(merchantId, out int merchantIdInt);

                measure = new LkpMeasureUnite
                {
                    Name = measureName.Trim(),
                    CreatedBy = merchantIdInt,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _unitOfWork.MeasureUnitRepository.CreateAsync(measure);
                await _unitOfWork.SaveChanges();
            }

            // The association between attribute and measure is created when adding product details
            // This method ensures both exist and can be used together
            return true;
        }
    }
}

