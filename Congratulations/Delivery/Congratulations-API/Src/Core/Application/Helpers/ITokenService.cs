namespace Congrats.Application.Helpers
{
    public interface ITokenService
    {
        Task<CallOptions> AcquireToken();
        Task<CallOptions> GetAccessTokenAsync();



    }
}
