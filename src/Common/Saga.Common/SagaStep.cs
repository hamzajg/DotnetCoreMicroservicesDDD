using System;
using System.Threading.Tasks;

namespace Saga.Common
{
    /// <summary>
    /// One step of a saga: an action plus the compensation that undoes it.
    /// Both are transport-agnostic delegates — HTTP calls, in-memory writes,
    /// or domain commands all fit the same shape.
    /// </summary>
    public class SagaStep
    {
        public SagaStep(string name, Func<Task> action, Func<Task> compensation = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Compensation = compensation;
        }

        public string Name { get; }
        public Func<Task> Action { get; }
        public Func<Task> Compensation { get; }
    }
}
