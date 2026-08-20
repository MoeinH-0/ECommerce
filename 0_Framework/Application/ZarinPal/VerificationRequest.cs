using System.Text.Json.Serialization;

namespace _0_Framework.Application.ZarinPal;

public class VerificationRequest
{
    [JsonPropertyName("amount")] public int Amount { get; set; }
    [JsonPropertyName("merchant_id")] public string MerchantID { get; set; }
    [JsonPropertyName("authority")] public string Authority { get; set; }
}