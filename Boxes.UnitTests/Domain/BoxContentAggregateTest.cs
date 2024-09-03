using Boxes.Domain.AggregatesModel.BoxAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.UnitTests.Domain
{
    [TestClass]
    public class BoxContentAggregateTest
    {
        [TestMethod]
        public void Create_box_success()
        {
            //Arrange    
            var name = "fake Box";
            var description = "desc";
            //Act 
            var fakeBoxContent = new BoxContent(name, description);
            //Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(fakeBoxContent);
        }

        [TestMethod]
        public void Update_box_eventpublished_success()
        {
            //Arrange    
            var name = "fake Box";
            var expectedEventsCount = 2;
            var description = "desc";
            //Act 
            var fakeBoxContent = new BoxContent(name,description);
            fakeBoxContent.Name = "new name";
            fakeBoxContent.AddBoxContentUpdatedDomainEvent();
            //Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(fakeBoxContent.DomainEvents.Count, expectedEventsCount);
        }
        [TestMethod]
        public void Delete_box_eventpublished_success()
        {
            //Arrange    
            var name = "fake Box";
            var expectedEventsCount = 2;
            var description = "desc";
            //Act 
            var fakeBoxContent = new BoxContent(name,description);
            fakeBoxContent.AddBoxContentDeletedDomainEvent();
            //Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(fakeBoxContent.DomainEvents.Count, expectedEventsCount);
        }
    }
}
