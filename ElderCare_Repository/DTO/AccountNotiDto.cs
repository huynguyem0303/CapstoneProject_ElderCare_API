using Newtonsoft.Json;

namespace ElderCare_Repository.DTO
{
    public class AccountNotiDto
    {
        public class NotificationInfo
        {
            [JsonProperty("isAndroiodDevice")]
            public bool IsAndroidDevice { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
        }

        [JsonProperty("accountId")]
        public int AccountId { get; set; }

        [JsonProperty("data")]
        public NotificationInfo Data { get; set; }
    }
}
