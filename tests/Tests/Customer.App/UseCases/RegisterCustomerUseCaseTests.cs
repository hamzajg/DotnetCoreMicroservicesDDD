using System;
using Moq;
using Xunit;
using Customer.App.UseCases;

namespace Tests.Customer.App.UseCases
{
    public class RegisterCustomerUseCaseTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<RegisterCustomerCommandMessage> _mockRCCM;

        private RegisterCustomerUseCase _sutRCCM;

        public RegisterCustomerUseCaseTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockRCCM = _mockRepository.Create<RegisterCustomerCommandMessage>("Test", "Test", "Test", "Test", "Test");
        }

        public void Dispose() {
            this._mockRepository.VerifyAll();
        }

        [Fact]
        public void HandleCommandTest()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCase();
            //When
            var result = _sutRCCM.HandleAsync(_mockRCCM.Object);
            //Then
            Assert.True(result.IsCompleted);
        }
        
        [Fact]
        public void HandlingNullCommandShouldThrowsException()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCase();
            //Then
            Assert.ThrowsAsync<Exception>(() => _sutRCCM.HandleAsync(null));
        }

    }
}