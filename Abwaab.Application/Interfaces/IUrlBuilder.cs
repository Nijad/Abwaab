namespace Abwaab.Application.Interfaces
{
    public interface IUrlBuilder
    {
        string GetCancelEmailChangeUrl(string changingCode);
        string GetCancelPhoneChangeUrl(string changingCode);
        //todo: get confirm email url
        //todo: get confirm phone url

    }
}
