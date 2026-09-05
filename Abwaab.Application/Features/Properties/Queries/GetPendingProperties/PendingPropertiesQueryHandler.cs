using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Properties.Queries.GetPendingProperties;

public class PendingPropertiesQueryHandler : IRequestHandler<PendingPropertiesQuery, List<PendingPropertiesResponse>>
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly IPropertyService _propertyService;

    private readonly string errorTitle = ErrorTitle.PendingProperties;

    public PendingPropertiesQueryHandler(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        IPropertyStatesService propertyStatesService,
        IPropertyService propertyService)
    {
        _userService = userService;
        _userManager = userManager;
        _propertyStatesService = propertyStatesService;
        _propertyService = propertyService;
    }

    public async Task<List<PendingPropertiesResponse>> Handle(PendingPropertiesQuery request, CancellationToken cancellationToken)
    {
        //check if the user is an admin
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        if(!await _userManager.IsInRoleAsync(user, RoleConstants.ROLE_ADMIN))
            throw new NoPermissionException("ليس لديك صلاحية للقيام بهذه المهمة", errorTitle);

        //get pending status properties from the database
        PropertyState pendingProperties = await _propertyStatesService.GetPendingPropertyStateAsync(errorTitle);

        //get peinding properties from the database
        List<PendingPropertiesResponse> pendingPropertiesList = await _propertyService.GetPropertiesByStateAsync(pendingProperties);

        return pendingPropertiesList;
    }
}