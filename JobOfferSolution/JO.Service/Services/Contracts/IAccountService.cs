namespace JO.Service.Services.Contracts
{
    public interface IAccountService
    {
        Task CreateRole(string roleName);
        Task<int> GetJobOfferUserId();
        Task<bool> LocalLogIn(string email);
        Task LocalLogOut();
    }
}