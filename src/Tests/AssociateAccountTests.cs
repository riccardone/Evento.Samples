using System;
using System.Linq;
using AppServices;
using Domain.Aggregates;
using Domain.Commands;
using Domain.Events;
using Evento;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests
{
    [TestClass]
    public class AssociateAccountTests
    {
        [TestMethod]
        public void WhenCreateAssociateAccount_ThenIExpectNoErrors()
        {
            // Assign
            var correlationId = Guid.NewGuid();
            var expectedAssociateId = Guid.NewGuid();
            var inputCommand = new CreateAssociateAccount(correlationId, expectedAssociateId);
            var repo = new Mock<IDomainRepository>();
            repo.Setup(a => a.GetById<AssociateAccount>(correlationId.ToString())).Throws(new AggregateNotFoundException("test"));

            // Act
            var result = new AssociateAccountHandler(repo.Object).Handle(inputCommand);

            // Assert
            repo.Verify();
            Assert.IsTrue(((AssociateAccountCreated)result.UncommitedEvents().Single()).AssociateId.Equals(expectedAssociateId));
        }
    }
}
