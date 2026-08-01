namespace Abwaab.Application.Interfaces
{
    public interface IAppDbContextInitializer
    {
        Task InitializeAsync();
        Task SeedAsync();
    }
}
