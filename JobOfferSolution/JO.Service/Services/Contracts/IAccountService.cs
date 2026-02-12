namespace JO.Service.Services.Contracts
{
    public interface IAccountService
    {
        Task<bool> LocalLogIn(string email);
        Task LocalLogOut();
    }
}