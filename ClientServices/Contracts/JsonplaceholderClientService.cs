using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Entities;

public class JsonplaceholderClientService: IJsonplaceholderClientService
{
    private readonly HttpClient _httpClient;

    public JsonplaceholderClientService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<JsonPlaceHolder[]> Get(int? Id,int? UserId)
    {
        try
        {
            string url = (Id, UserId) switch
            {
                (not null and not 0, not null and not 0) => $"posts/{Id}?userId={UserId}",
                (not null and not 0, _) => $"posts/{Id}",
                (_, not null and not 0) => $"posts?userId={UserId}",
                _ => "posts/"
            };

            if (Id is not null and not 0)
            {
                JsonPlaceHolder? request = await _httpClient.GetFromJsonAsync<JsonPlaceHolder>(url);

                if (request is not null)
                {
                    return new JsonPlaceHolder[] { request };
                }
                return [];
            }
            else
            {
                return await _httpClient.GetFromJsonAsync<JsonPlaceHolder[]>(url) ?? [];
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching data", ex);
        }
    }

}
