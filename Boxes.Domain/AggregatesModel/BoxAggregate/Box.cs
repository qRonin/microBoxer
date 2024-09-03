using Boxes.Domain.Events.Box;
using Boxes.Domain.SeedWork;
using Boxes.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Boxes.Domain.AggregatesModel.BoxAggregate
{
    public partial class Box : Entity, IAggregateRoot
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
        public string? BoxName { get; set; }
        public List<BoxContent> BoxContents { get; set; }


        public Box(string boxName)
        {

            BoxName = !string.IsNullOrWhiteSpace(boxName)  ? boxName : throw new ArgumentNullException(nameof(boxName));
            BoxContents = new List<BoxContent>();
            AddBoxCreatedDomainEvent();
        }

        [JsonConstructor]
        public Box()
        {
            if (BoxContents != null)
            {
                foreach (var content in this.BoxContents)
                {
                    content.Box = null;
                }
            }

        }

        private void AddBoxCreatedDomainEvent()
        {
            var boxCreatedDomainEvent = new BoxCreatedDomainEvent(this);

            this.AddDomainEvent(boxCreatedDomainEvent);

        }
        public void AddBoxUpdatedDomainEvent()
        {
            var boxUpdatedDomainEvent = new BoxUpdatedDomainEvent(this.Id);

            this.AddDomainEvent(boxUpdatedDomainEvent);

        }
        public void AddBoxDeletedDomainEvent()
        {
            var boxDeletedDomainEvent = new BoxDeletedDomainEvent(this.Id);

            this.AddDomainEvent(boxDeletedDomainEvent);

        }


    }


}
