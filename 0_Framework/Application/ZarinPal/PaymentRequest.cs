using System.Text.Json.Serialization;

namespace _0_Framework.Application.ZarinPal;

public class PaymentRequest
{
    [JsonPropertyName("mobile")] public string Mobile { get; set; } // mobile -> zarinpal dose not understand it !!!
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("callback_url")] public string CallbackURL { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("amount")] public int Amount { get; set; }
    [JsonPropertyName("merchant_id")] public string MerchantID { get; set; }
}