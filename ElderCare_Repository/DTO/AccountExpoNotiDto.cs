using ElderCare_Domain.Validation;

namespace ElderCare_Repository.DTO
{
    public class AccountExpoNotiDto
    {
        public int AccountId { get; set; }

        public class ExpoNotiDto
        {
            public string Title { get; set; }

            public string body { get; set; }

            [StringRange(AllowableValues = new[] { "default", "normal", "high" }, AllowNull = true)]
            public string? Priority { get; set; } //'default' | 'normal' | 'high'

            public string SubTitle { get; set; }

            [StringRange(AllowableValues = new[] { "default" }, AllowNull = true)]
            public string? Sound { get; set; } = "default";//'default' | null

            public int? Badge { get; set; }

            public string ChannelId { get; set; } = "test";

            public bool MutableContent { get; set; } = true;
        }

        public ExpoNotiDto Data { get; set; }
    }
}
