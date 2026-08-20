using System.Text.Json.Serialization;

namespace _0_Framework.Application.ZarinPal;

public class VerificationResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("ref_id")]
    public long RefID { get; set; }
}