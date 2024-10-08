﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Domain.Events.BoxContent;

public record BoxContentDeletedDomainEvent(
    Guid id, Guid userId
    ) : INotification;

