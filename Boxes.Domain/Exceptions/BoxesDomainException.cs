using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Domain.Exceptions;

public class BoxesDomainException : Exception
{
    public BoxesDomainException()
    { }

    public BoxesDomainException(string message)
        : base(message)
    { }

    public BoxesDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
