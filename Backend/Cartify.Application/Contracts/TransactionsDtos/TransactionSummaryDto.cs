using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartify.Application.Contracts.TransactionsDtos
{
    public class TransactionSummaryDto
    {
        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int RefundsCount { get; set; }
        public string Period { get; set; } // daily, weekly, monthly
    }
}
