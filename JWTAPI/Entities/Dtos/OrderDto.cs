namespace JWTAPI.Entities.Dtos;

public record OrderDto
{
    public int Id { get; init; }
    public DateOnly DateTime {get; init;}
    public int Status {get; init;}
    public int UserId {get; init;}
}