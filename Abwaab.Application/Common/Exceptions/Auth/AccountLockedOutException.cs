using Abwaab.Application.Common.Constants;

namespace Abwaab.Application.Common.Exceptions.Auth
{
    public class AccountLockedOutException : Exception
    {
        public bool ReturnToUser { get; } = true;
        public string Title { get; }

        public AccountLockedOutException(string message, string title) : base(message)
        {
            Title = title;
        }
    }
}
