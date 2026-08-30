using Abwaab.Application.Features.Properties.Common.DTOs;
using MediatR;

namespace Abwaab.Application.Features.Properties.Submit
{
    public class SubmitPropertyCommand : PropertyDTO, IRequest<SubmitPropertyResponse>
    {
    }
}
