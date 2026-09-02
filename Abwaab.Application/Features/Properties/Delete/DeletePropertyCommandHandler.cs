using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;

namespace Abwaab.Application.Features.Properties.Delete;

public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, DeletePropertyResponse>
{
    private readonly IPropertyService _propertyService;
    private readonly IUserService _userService;
    private readonly IPropertyStatesService _propertyStatesService;
    private readonly ITransactionManager _transactionManager;
    private readonly IStorageService _storageService;
    private readonly IMediaService _mediaService;
    private readonly string errorTitle = ErrorTitle.DeleteProperty;
    public DeletePropertyCommandHandler(
        IPropertyService propertyService,
        IUserService userService,
        IPropertyStatesService propertyStatesService,
        ITransactionManager transactionManager,
        IStorageService storageService,
        IMediaService mediaService)
    {
        _propertyService = propertyService;
        _userService = userService;
        _propertyStatesService = propertyStatesService;
        _transactionManager = transactionManager;
        _storageService = storageService;
        _mediaService = mediaService;
    }
    public async Task<DeletePropertyResponse> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        string username = _userService.FindUserNameByContext(errorTitle);
        ApplicationUser? user = await _userService.FindUserByNameAsync(username);
        if (user == null)
            throw new UserNotFoundException(username, errorTitle);

        Property property = await _propertyService.FindPropertyByIdAsync(request.PropertyId, errorTitle);

        if (property.UserPlan.UserId != user.Id)
            throw new ObjectNotBelongToUserException("العقار", errorTitle);

        await _transactionManager.BeginTransactionAsync(cancellationToken);

        try
        {
            PropertyState deletedState = await _propertyStatesService.GetDeletedPropertyStateAsync(errorTitle);
            property.PropertyState = deletedState;
            await _propertyService.UpdatePropertyAsync(property);

            foreach (Media media in property.MediaList)
            {
                media.IsDeleted = true;
                await _mediaService.DeleteMediaAsync(media);
                await _storageService.DeleteMedia(media.FilePath);
            }

            await _transactionManager.CommitTransactionAsync(cancellationToken);
            return new DeletePropertyResponse
            {
                Success = true,
                Message = "تم حذف العقار بنجاح."
            };
        }
        catch
        {
            await _transactionManager.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}