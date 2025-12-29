using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    //Record is used due to data immutability and value-based equality
    public record CompanyDto
    {
        //Init-only properties protect the state of the object from mutation (serialization during request) once initialization is finished.
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    }
}
