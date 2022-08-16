using System.Text.Json.Serialization;

namespace Entities.Responses
{
    public class AccountByUserResponse
    {
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public decimal CurrentBalance { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountStatus AccountStatus { get; set; }
        public string UserImageUrl { get; set; }
    }
}
