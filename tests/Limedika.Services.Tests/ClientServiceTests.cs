using AutoFixture;
using Limedika.Contracts;
using Limedika.Infrastructure.Models;
using Limedika.Infrastructure.Repositories;
using Limedika.Integrations;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Limedika.Services.Tests
{
    public class ClientServiceTests
    {
        private Mock<IClientsRepository> _clientsRepositoryMock = null!;
        private Mock<IPostItClient> _postItClientMock = null!;
        private ClientService _clientService = null!;

        private readonly Fixture _fixture;

        public ClientServiceTests()
        {
            _fixture = new Fixture();
        }

        [SetUp]
        public void Setup()
        {
            _clientsRepositoryMock = new Mock<IClientsRepository>();
            _postItClientMock = new Mock<IPostItClient>();
            Mock<ILogger<ClientService>> logger = new();

            _clientService = new ClientService(logger.Object, _clientsRepositoryMock.Object, _postItClientMock.Object);
        }

        [Test]
        public async Task Given_Data_When_ImportClients_Then_ClientsAreImported()
        {
            // Given
            _clientsRepositoryMock.Setup(x => x.GetClients()).ReturnsAsync(new List<ClientEntity>());
            _clientsRepositoryMock.Setup(x => x.InsertClients(It.IsAny<List<ClientEntity>>())).Returns(Task.CompletedTask);

            // When
            await _clientService.ImportClients();

            // Then
            _clientsRepositoryMock.Verify(x => x.GetClients(), Times.Once);
            _clientsRepositoryMock.Verify(x => x.InsertClients(It.IsAny<List<ClientEntity>>()), Times.Once);
        }

        [Test]
        public async Task Given_Clients_When_GetClients_Then_ClientsAreReturned()
        {
            // Given
            List<ClientEntity> clients = _fixture.CreateMany<ClientEntity>(3).ToList();

            _clientsRepositoryMock.Setup(x => x.GetClients()).ReturnsAsync(clients);

            //When
            List<Client> result = await _clientService.GetClients();

            //Then
            Assert.AreEqual(clients.Count, result.Count);
            AssertClients(clients, result);
        }

        [Test]
        public async Task Given_Clients_When_UpdatePostCodes_Then_PostCodesAreUpdated()
        {
            // Given
            List<ClientEntity> clients = _fixture.CreateMany<ClientEntity>(2).ToList();

            _clientsRepositoryMock.Setup(x => x.GetClients()).ReturnsAsync(clients);
            _postItClientMock.Setup(x => x.GetPostCode(clients.First().Address)).ReturnsAsync((string)null);
            _postItClientMock.Setup(x => x.GetPostCode(clients.Last().Address)).ReturnsAsync(_fixture.Create<string>());
            _clientsRepositoryMock.Setup(x => x.UpdatePostCodes(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // When
            await _clientService.UpdatePostCodes();

            // Then
            _clientsRepositoryMock.Verify(x => x.GetClients(), Times.Once);
            _postItClientMock.Verify(x => x.GetPostCode(It.IsAny<string>()), Times.Exactly(2));
            _clientsRepositoryMock.Verify(x => x.UpdatePostCodes(clients.First().Id, It.IsAny<string>()), Times.Exactly(0));
            _clientsRepositoryMock.Verify(x => x.UpdatePostCodes(clients.Last().Id, It.IsAny<string>()), Times.Exactly(1));
        }

        private static void AssertClients(List<ClientEntity> expectedClients, List<Client> actualClients)
        {
            foreach (ClientEntity expectedClient in expectedClients)
            {
                Client? actualClient = actualClients.FirstOrDefault(x => x.Id == expectedClient.Id);

                Assert.IsNotNull(actualClient);
                Assert.AreEqual(expectedClient.Name, actualClient.Name);
                Assert.AreEqual(expectedClient.Address, actualClient.Address);
                Assert.AreEqual(expectedClient.PostCode, actualClient.PostCode);
                Assert.AreEqual(expectedClient.CreatedOn, actualClient.CreatedOn);
            }
        }
    }
}
