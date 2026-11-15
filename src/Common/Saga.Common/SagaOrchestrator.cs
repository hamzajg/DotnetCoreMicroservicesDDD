using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saga.Common
{
    /// <summary>
    /// Minimal choreography-style saga orchestrator.
    ///
    /// Runs steps in order; on the first failure, compensates the steps that
    /// already completed in reverse order. A failing compensation is recorded
    /// in <see cref="SagaResult.CompensationErrors"/> but never masks the
    /// original error — the saga's job is to leave the system as close to
    /// consistent as possible, and to say honestly when it couldn't.
    /// </summary>
    public class SagaOrchestrator
    {
        public async Task<SagaResult> RunAsync(IEnumerable<SagaStep> steps)
        {
            var result = new SagaResult { Succeeded = true };
            var completed = new List<SagaStep>();

            foreach (var step in steps ?? Enumerable.Empty<SagaStep>())
            {
                try
                {
                    await step.Action();
                    completed.Add(step);
                    result.CompletedSteps.Add(step.Name);
                }
                catch (Exception ex)
                {
                    result.Succeeded = false;
                    result.Error = $"Step '{step.Name}' failed: {ex.Message}";
                    await CompensateAsync(completed, result);
                    return result;
                }
            }

            return result;
        }

        private static async Task CompensateAsync(List<SagaStep> completed, SagaResult result)
        {
            for (var i = completed.Count - 1; i >= 0; i--)
            {
                var step = completed[i];
                if (step.Compensation == null)
                    continue;
                try
                {
                    await step.Compensation();
                    result.CompensatedSteps.Add(step.Name);
                }
                catch (Exception ex)
                {
                    result.CompensationErrors.Add(
                        $"Compensation for '{step.Name}' failed: {ex.Message}");
                }
            }
        }
    }
}
