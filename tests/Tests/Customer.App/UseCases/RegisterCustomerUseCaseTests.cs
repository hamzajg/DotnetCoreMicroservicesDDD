using System.Linq;
using System;
using Moq;
using Xunit;
using Customer.App.UseCases;
using Customer.App.Services;
using Services.Common.Domain.Events;
using Customer.App.Events;

namespace Tests.Customer.App.UseCases
{
    public class RegisterCustomerUseCaseTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<RegisterCustomerCommandMessage> _mockRCCM;
        private readonly Mock<IPublisherServices> _mockPubServices;

        private RegisterCustomerUseCase _sutRCCM;

        public RegisterCustomerUseCaseTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockRCCM = _mockRepository.Create<RegisterCustomerCommandMessage>("Test", "Test", "Test", "Test", "Test");
            _mockPubServices = _mockRepository.Create<InMemoryPublisherServices>().As<IPublisherServices>();
        }

        public void Dispose() {
            this._mockRepository.VerifyAll();
        }

        [Fact]
        public void HandleCommandTest()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCase(_mockPubServices.Object);
            //When
            var result = _sutRCCM.HandleAsync(_mockRCCM.Object);
            //Then
            _mockPubServices.Verify(x => x.PublishAsync(It.IsAny<DomainEvent>()), Times.Once);
            Assert.True(result.IsCompleted);
        }
        
        [Fact]
        public void HandleCommandShouldReturnValidCustomerFromRegistredEventTest()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCase(_mockPubServices.Object);
            //When
            var result = _sutRCCM.HandleAsync(_mockRCCM.Object);
            //Then
            _mockPubServices.Verify(x => x.PublishAsync(It.IsAny<DomainEvent>()), Times.Once);
            Assert.NotEqual(0, ((InMemoryPublisherServices) _mockPubServices.Object).Events.Count);
            Assert.NotNull(((CustomerRegistered) ((InMemoryPublisherServices) _mockPubServices.Object).Events.ElementAtOrDefault(0)).Customer);
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void HandlingNullCommandShouldThrowsException()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCase(_mockPubServices.Object);
            //Then
            Assert.ThrowsAsync<Exception>(() => _sutRCCM.HandleAsync(null));
        }

    }
}