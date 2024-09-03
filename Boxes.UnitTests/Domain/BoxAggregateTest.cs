﻿using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.Events.BoxContent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.UnitTests.Domain;

[TestClass]
public class BoxAggregateTest
{
    public BoxAggregateTest()
    { }

    [TestMethod]
    public void Create_box_success()
    {
        //Arrange    
        var name = "fake Box";
        //Act 
        var fakeBox = new Box(name);
        //Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(fakeBox);       
    }

    [TestMethod]
    public void Update_box_eventpublished_success()
    {
        //Arrange    
        var name = "fake Box";
        var expectedEventsCount = 2;
        //Act 
        var fakeBox = new Box(name);
        fakeBox.BoxName = "new name";
        fakeBox.AddBoxUpdatedDomainEvent();
        //Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(fakeBox.DomainEvents.Count, expectedEventsCount);
    }
    [TestMethod]
    public void Delete_box_eventpublished_success()
    {
        //Arrange    
        var name = "fake Box";
        var expectedEventsCount = 2;
        //Act 
        var fakeBox = new Box(name);
        fakeBox.AddBoxDeletedDomainEvent();
        //Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(fakeBox.DomainEvents.Count, expectedEventsCount);
    }


}
