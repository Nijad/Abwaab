using Abwaab.Application.Features.Properties.Common.DTOs;
using MediatR;

namespace Abwaab.Application.Features.Properties.Update
{
    public class UpdatePropertyCommand : PropertyDTO, IRequest<UpdatePropertyResponse>
    {
    }
}
