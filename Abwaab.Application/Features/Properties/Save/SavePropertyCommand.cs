using Abwaab.Application.Features.Properties.Common.DTOs;
using MediatR;

namespace Abwaab.Application.Features.Properties.Save
{
    public class SavePropertyCommand : PropertyDTO, IRequest<SavePropertyResponse>
    {
    }
}
