namespace Abwaab.Application.Features.Users.Role.GetUserRoles
{
    public class GetUserRolesResponse
    {
        public bool Success { get; set; }
        public List<string> Roles { get; set; } = new();
        public string Message { get; set; }
    }
}
