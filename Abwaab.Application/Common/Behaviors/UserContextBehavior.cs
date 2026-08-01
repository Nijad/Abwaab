using Abwaab.Application.Interfaces;
using MediatR;

namespace Abwaab.Application.Common.Behaviors
{
    public class UserContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUserRequest
{
    private readonly IUserContext _userContext;

    public UserContextBehavior(IUserContext userContext) => _userContext = userContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.UserId = _userContext.UserId; // Automatically sets it!
        return await next();
    }
}
}
