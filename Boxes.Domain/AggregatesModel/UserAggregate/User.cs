using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Domain.SeedWork;
using Boxes.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Domain.AggregatesModel.UserAggregate
{
    public partial class User : Entity, IAggregateRoot
    {
        public string UserName { get; set; }
        [Required]
        public Guid Id { get; set; }
        public IEnumerable<Box>? UserBoxes { get; set; }
        public IEnumerable<BoxContent>? UserBoxContents { get; set; }

        public User(Guid id)
        {
                Id = id;
        }

    }
}
