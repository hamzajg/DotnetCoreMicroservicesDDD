using System.Collections.Generic;

namespace Saga.Common
{
    /// <summary>Outcome of a saga run.</summary>
    public class SagaResult
    {
        public bool Succeeded { get; set; }
        public List<string> CompletedSteps { get; } = new List<string>();
        public List<string> CompensatedSteps { get; } = new List<string>();
        public List<string> CompensationErrors { get; } = new List<string>();
        public string Error { get; set; }
    }
}
