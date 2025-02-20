using CodeChallenging.Entities;

namespace CodeChallenging.ClientServices.Interfaces
{
    public interface IJsonplaceholderClientService
    {
        Task<JsonPlaceHolder[]> Get(int? Id,int? UserId);
    }
}
