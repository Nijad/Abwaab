using MediatR;

namespace Abwaab.Application.Features.Properties.Delete;

public class DeletePropertyCommand: IRequest<DeletePropertyResponse>
{
    public Guid PropertyId { get; set; }
    public string Comment { get; set; } = string.Empty;
}
