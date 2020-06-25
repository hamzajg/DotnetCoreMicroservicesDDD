using Customer.App.Services;
using Domain.Common.Events;
using Moq;
using System;
using Xunit;

namespace Tests.Customer.App.Services
{
    public class PublisherServicesTests : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly Mock<PublisherServices> _mockPubServices;
        private readonly Mock<DomainEvent> _mockEvent;

        public PublisherServicesTests()
        {
            this._mockRepository = new MockRepository(MockBehavior.Loose);

            _mockPubServices = _mockRepository.Create<InMemoryPublisherServices>().As<PublisherServices>();
            _mockEvent = _mockRepository.Create<DomainEvent>();

            _mockPubServices.CallBase = true;

        }
        public void Dispose()
        {
            this._mockRepository.VerifyAll();
        }

        [Fact]
        public void ShouldBeAbleToPublishAnEvent()
        {
            //Given
            var pubServices = _mockPubServices.Object;
            //When
            pubServices.PublishAsync(_mockEvent.Object);
            //Then
            Assert.NotEmpty(((InMemoryPublisherServices) pubServices).Events);
        }
    }
}
