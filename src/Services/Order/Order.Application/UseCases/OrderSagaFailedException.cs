using System;
using Saga.Common;

namespace Order.Application.UseCases
{
    /// <summary>
    /// Thrown when the order saga could not complete. Carries the
    /// <see cref="SagaResult"/> so a caller can see which step failed, what was
    /// completed and what was compensated.
    /// </summary>
    public class OrderSagaFailedException : Exception
    {
        public OrderSagaFailedException(SagaResult result)
            : base((result == null ? "The order saga failed" : result.Error))
        {
            Result = result;
        }

        public SagaResult Result { get; }
    }
}
