namespace JO.Service.Services.Contracts
{
    public interface IReturnJobOfferService
    {
        Task<int> ReSubmitReturnedJO(int id, int positionId, int departmentId, decimal basicSalary, decimal allowance, decimal signingBonus, DateTime startDate, int modifiedBy);
        Task<int> ReturnToTA(int id, int reasonId, int activityId, int userId, string remarks);
    }
}