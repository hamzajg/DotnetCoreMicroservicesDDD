using System.Linq;
using System;
using Moq;
using Xunit;
using Customer.App.UseCases;
using Customer.App.Services;
using Customer.App.Commands;
using Customer.Domain.Events;
using Domain.Common.Events;

namespace Tests.Customer.App.UseCases
{
    public class RegisterCustomerUseCaseTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<RegisterCustomer> _mockRCCM;
        private readonly Mock<PublisherServices> _mockPubServices;

        private RegisterCustomerUseCase _sutRCCM;

        public RegisterCustomerUseCaseTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockRCCM = _mockRepository.Create<RegisterCustomer>("Test", "Test", "Test", "Test", "Test");
            _mockPubServices = _mockRepository.Create<InMemoryPublisherServices>(MockBehavior.Loose).As<PublisherServices>();
            _mockPubServices.CallBase = true;
        }

        public void Dispose() {
            this._mockRepository.VerifyAll();
        }

        [Fact]
        public void HandleCommandTest()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCaseImpl(_mockPubServices.Object);
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
            var pubServices = _mockPubServices.Object;
            _sutRCCM = new RegisterCustomerUseCaseImpl(pubServices);
            //When
            var result = _sutRCCM.HandleAsync(_mockRCCM.Object);
            //Then
            _mockPubServices.Verify(x => x.PublishAsync(It.IsAny<DomainEvent>()), Times.Once);
            Assert.NotEmpty(((InMemoryPublisherServices) pubServices).Events);
            Assert.NotNull(((CustomerCreated) ((InMemoryPublisherServices) pubServices).Events.ElementAtOrDefault(0)).Customer);
            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void HandlingNullCommandShouldThrowsException()
        {
            //Given
            _sutRCCM = new RegisterCustomerUseCaseImpl(_mockPubServices.Object);
            //Then
            Assert.ThrowsAsync<Exception>(() => _sutRCCM.HandleAsync(null));
        }

    }
}