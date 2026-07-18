namespace Abwaab.Application.Common.Contracts
{
    public interface IAppDbContextInitializer
    {
        Task InitializeAsync();
        Task SeedAsync();
    }
}
