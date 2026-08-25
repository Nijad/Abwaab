using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.MediaEntities;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Medias.DeleteMedia
{
    public class DeleteMediaCommandHandler : IRequestHandler<DeleteMediaCommand, DeleteMediaResponse>
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<DeleteMediaCommandHandler> _logger;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IStorageService _storageService;
        private readonly ITransactionManager _transactionManager;

        private readonly string errorTitle = ErrorTitle.DeleteMedia;

        public DeleteMediaCommandHandler(
            IMediaService mediaStorageService,
            ILogger<DeleteMediaCommandHandler> logger,
            IUserService userService,
            IPropertyService propertyService,
            IStorageService storageService,
            ITransactionManager transactionManager)
        {
            _mediaService = mediaStorageService;
            _logger = logger;
            _userService = userService;
            _propertyService = propertyService;
            _storageService = storageService;
            _transactionManager = transactionManager;
        }

        public async Task<DeleteMediaResponse> Handle(DeleteMediaCommand command, CancellationToken cancellationToken)
        {
            string username = _userService.FindUserNameByContext(errorTitle);
            ApplicationUser? user = await _userService.FindUserByNameAsync(username);
            if (user == null)
                throw new UserNotFoundException(username, errorTitle);

            Media media = await _mediaService.FindMediaByIdAsync(command.MediaId, errorTitle, cancellationToken);

            bool belong = await _propertyService.PropertyBelongToUser(user.Id, media.Property!.Id);

            if (!belong)
                throw new ObjectNotBelongToUserException("ملف الوسائط", errorTitle);

            await _transactionManager.BeginTransactionAsync(cancellationToken);
            try
            {
                //delete physical file
                await _storageService.DeleteMedia(media.FilePath);

                //delete from database
                await _mediaService.DeleteMediaAsync(media, cancellationToken);

                await _transactionManager.CommitTransactionAsync(cancellationToken);
                
                return new() { Success = true, Message = "تم حذف ملف الوسائط بنجاح." };
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
