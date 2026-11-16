using System;
using System.Threading.Tasks;

namespace Order.Application.Ports
{
    /// <summary>
    /// Read side of the Customer service the order saga depends on. A port, so
    /// the saga stays transport-agnostic: today the adapter is in-memory, a
    /// later HTTP adapter against Customer.WebApi implements the same contract.
    /// </summary>
    public interface CustomerDirectory
    {
        Task<bool> ExistsAsync(Guid customerId);
    }
}
