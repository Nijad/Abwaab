using Abwaab.Application.Common.Validations.Common;
using Abwaab.Domain.Enums;
using MediatR;
using System.Reflection;

namespace Abwaab.Application.Common.Behaviors
{
    public class DetectIdentifierBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Only process commands that have a PhoneNumber property
            PropertyInfo? identifierProp = typeof(TRequest).GetProperty("Identifier");
            PropertyInfo? identifierTypeProp = typeof(TRequest).GetProperty("IdentifierType");

            if (identifierProp != null && identifierTypeProp.CanWrite)
            {
                string? identifierValue = identifierProp?.GetValue(request) as string;
                identifierTypeProp.SetValue(request ,DetectIdentifierType(identifierValue));
            }

            // Continue to the next behavior or the handler
            return await next();
        }

        private IdentifierEnum DetectIdentifierType(string identifier)
        {
            if (CommonValidation.IsValidPhoneNumber(identifier))
                return IdentifierEnum.phone_number;
            else if (CommonValidation.IsValidEmail(identifier))
                return IdentifierEnum.email;
            else
                throw new ArgumentException("Invalid identifier format. Must be a valid phone number (+9639XXXXXXXX) or email address.");
        }
    }
}
