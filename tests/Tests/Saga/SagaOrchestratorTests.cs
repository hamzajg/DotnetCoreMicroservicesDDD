using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Saga.Common;
using Xunit;

namespace Tests.Saga
{
    public class SagaOrchestratorTests
    {
        private readonly SagaOrchestrator _sut = new SagaOrchestrator();

        private static SagaStep Step(string name, List<string> log, bool fail = false,
            bool failCompensation = false)
        {
            return new SagaStep(
                name,
                () =>
                {
                    log.Add($"action:{name}");
                    return fail ? Task.FromException(new InvalidOperationException($"boom-{name}"))
                                : Task.CompletedTask;
                },
                () =>
                {
                    log.Add($"compensate:{name}");
                    return failCompensation
                        ? Task.FromException(new InvalidOperationException($"comp-{name}"))
                        : Task.CompletedTask;
                });
        }

        [Fact]
        public async Task HappyPathRunsAllStepsInOrder()
        {
            var log = new List<string>();
            var result = await _sut.RunAsync(new[]
            {
                Step("a", log), Step("b", log), Step("c", log),
            });

            Assert.True(result.Succeeded);
            Assert.Equal(new[] { "action:a", "action:b", "action:c" }, log);
            Assert.Empty(result.CompensatedSteps);
        }

        [Fact]
        public async Task FailureCompensatesCompletedStepsInReverse()
        {
            var log = new List<string>();
            var result = await _sut.RunAsync(new[]
            {
                Step("a", log), Step("b", log), Step("c", log, fail: true),
            });

            Assert.False(result.Succeeded);
            Assert.Contains("boom-c", result.Error);
            Assert.Equal(
                new[] { "action:a", "action:b", "action:c",
                        "compensate:b", "compensate:a" },
                log);
            Assert.Equal(new[] { "b", "a" }, result.CompensatedSteps);
            Assert.Equal(new[] { "a", "b" }, result.CompletedSteps);
        }

        [Fact]
        public async Task FailedStepIsNotCompensated()
        {
            var log = new List<string>();
            var result = await _sut.RunAsync(new[]
            {
                Step("a", log, fail: true),
            });

            Assert.False(result.Succeeded);
            Assert.Equal(new[] { "action:a" }, log);
            Assert.Empty(result.CompensatedSteps);
        }

        [Fact]
        public async Task CompensationFailureDoesNotMaskOriginalError()
        {
            var log = new List<string>();
            var result = await _sut.RunAsync(new[]
            {
                Step("a", log, failCompensation: true),
                Step("b", log, fail: true),
            });

            Assert.False(result.Succeeded);
            Assert.Contains("boom-b", result.Error);
            Assert.Single(result.CompensationErrors);
            Assert.Contains("comp-a", result.CompensationErrors[0]);
        }

        [Fact]
        public async Task StepWithoutCompensationIsSkippedOnRollback()
        {
            var log = new List<string>();
            var noComp = new SagaStep("a", () => { log.Add("action:a"); return Task.CompletedTask; });
            var result = await _sut.RunAsync(new[] { noComp, Step("b", log, fail: true) });

            Assert.False(result.Succeeded);
            Assert.Equal(new[] { "action:a", "action:b" }, log);
            Assert.Empty(result.CompensatedSteps);
        }

        [Fact]
        public async Task EmptySagaSucceeds()
        {
            var result = await _sut.RunAsync(new SagaStep[0]);
            Assert.True(result.Succeeded);
            Assert.Empty(result.CompletedSteps);
        }
    }
}
