using MediatR;

namespace Abwaab.Application.Features.Properties.Save
{
    public class SavePropertyCommandHandler : IRequestHandler<SavePropertyCommand, SavePropertyResponse>
    {
        public Task<SavePropertyResponse> Handle(SavePropertyCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
