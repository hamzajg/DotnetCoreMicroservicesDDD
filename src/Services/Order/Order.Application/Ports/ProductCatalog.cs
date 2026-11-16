using System;
using System.Threading.Tasks;

namespace Order.Application.Ports
{
    /// <summary>
    /// Read side of the Product service the order saga depends on. A port, so
    /// the saga stays transport-agnostic — mirrors <see cref="CustomerDirectory"/>.
    /// </summary>
    public interface ProductCatalog
    {
        Task<bool> ExistsAsync(Guid productId);
    }
}
