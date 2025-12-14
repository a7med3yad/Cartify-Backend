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


    }
}

