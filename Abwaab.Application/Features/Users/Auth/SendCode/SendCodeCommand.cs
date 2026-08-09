using MediatR;

namespace Abwaab.Application.Features.Users.Auth.SendCode
{
    public class SendCodeCommand : IRequest<SendCodeResponse>
    {
        public string Identifier { get; set; } = string.Empty;
    }
}
