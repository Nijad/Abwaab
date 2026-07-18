using System.Text;
using System.Text.Json;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abwaab.Infrastructure.Services
{
    public class TextBeeSmsSender : ISmsSender
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TextBeeSmsSender> _logger;
        private readonly TextBeeSettings _settings;

        public TextBeeSmsSender(
            HttpClient httpClient,
            IOptions<TextBeeSettings> settings,
            ILogger<TextBeeSmsSender> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _settings = settings.Value;

            // Set the base URL for TextBee API
            _httpClient.BaseAddress = new Uri("https://api.textbee.dev/");
            // Add API key to default headers
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _settings.ApiKey);
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                // Build the request payload
                var payload = new
                {
                    recipients = new[] { phoneNumber },
                    message,
                    from = _settings.SenderPhoneNumber
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the SMS via TextBee API
                var endpoint = $"api/v1/gateway/devices/{_settings.DeviceId}/send-sms";
                var response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("SMS sent successfully to {PhoneNumber}", phoneNumber);
                    return true;
                }

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("TextBee API error: {StatusCode} - {Error}", response.StatusCode, error);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending SMS to {PhoneNumber}", phoneNumber);
                return false;
            }
        }
    }
}