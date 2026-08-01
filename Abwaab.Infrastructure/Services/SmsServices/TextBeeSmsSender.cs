using Abwaab.Application.Common.Exceptions.SMS;
using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Abwaab.Infrastructure.Services.SmsServices
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

        public async Task SendSmsAsync(string phoneNumber, string message)
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

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"TextBee API error: {response.StatusCode} - {error}", response.StatusCode, error);

                throw new FailedSendignSMSException();
            }
        }
    }
}