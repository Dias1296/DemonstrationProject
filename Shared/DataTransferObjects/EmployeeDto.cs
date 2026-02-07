namespace Shared.DataTransferObjects
{
    //Record is used due to data immutability and value-based equality
    public record EmployeeDto(Guid Id, string Name, int Age, string Position);
}
