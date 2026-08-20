using System.Text.Json.Serialization;

namespace _0_Framework.Application.ZarinPal;

public class PaymentResponse
{
    [JsonPropertyName("code")]
    public int Code{ get; set; }
    [JsonPropertyName("authority")] public string Authority { get; set; }
}