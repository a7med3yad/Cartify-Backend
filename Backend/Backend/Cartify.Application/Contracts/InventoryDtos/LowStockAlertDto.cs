using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.InventoryDtos
{
    public class LowStockAlertDto
    {
        public int ProductDetailId { get; set; }
        public string ProductName { get; set; }
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
    }

}
