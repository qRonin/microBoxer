using Boxes.Domain.Events.Box;
using Boxes.Domain.Events.BoxContent;
using Boxes.Domain.SeedWork;
using Boxes.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Boxes.Domain.AggregatesModel.BoxAggregate
{
    public partial class BoxContent : Entity, IAggregateRoot
    {

        Guid _Id;
        override public Guid Id
        {
            get
            {
                return _Id;
            }
            protected set
            {
                _Id = value;
            }
        }
        [Required]
        public string Name { get; set; } = null!;
        public Guid? BoxId { get; set; }
        public string? Description { get; set; }
        public Guid? LastKnownBoxId { get; set; }
        public int? OrderNumber { get; set; }
        public Box? Box { get; set; }

        public BoxContent(string name, string description)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Description = description;
            Box = null;
            AddBoxContentCreatedDomainEvent();
        }

        public BoxContent(Guid? boxId, string name, string description)
        {
            BoxId = boxId;
            LastKnownBoxId = boxId;
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Description = description;
            Box = null;
            AddBoxContentCreatedDomainEvent();
        }
        
        [JsonConstructor]
        public BoxContent()
        {
            this.Box = null;
        }



        private void AddBoxContentCreatedDomainEvent()
        {
            var boxContentCreatedDomainEvent = new BoxContentCreatedDomainEvent(this);

            this.AddDomainEvent(boxContentCreatedDomainEvent);

        }
        public void AddBoxContentUpdatedDomainEvent()
        {
            var boxContentUpdatedDomainEvent = new BoxContentUpdatedDomainEvent(this.Id);

            this.AddDomainEvent(boxContentUpdatedDomainEvent);

        }
        public void AddBoxContentDeletedDomainEvent()
        {
            var boxContentDeletedDomainEvent = new BoxContentDeletedDomainEvent(this.Id);

            this.AddDomainEvent(boxContentDeletedDomainEvent);

        }

    }

}
