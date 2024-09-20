using HFPMapp.Models;
using HFPMapp.Services.EncryptionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HFPMapp.Services.HTTP
{
    public class BankSummaryApiClient
    {
        private readonly string _apiBaseUrl;

        public BankSummaryApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateBankSummaryAsync(BankSummary newBankSummary)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string bankSummaryJson = JsonSerializer.Serialize(newBankSummary);
                string encryptedData = EncryptionHelper.Encrypt(bankSummaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/BankSummary/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create bank summary: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bank summary creation failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<BankSummary>> GetBankSummariesAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/BankSummary/Get";

                // Realizar la solicitud POST a la API
                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta encriptada
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    var jsonString = Encoding.UTF8.GetString(responseBytes);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserializar la respuesta en un objeto EncryptedResponse
                    EncryptedResponse encryptedResponse = JsonSerializer.Deserialize<EncryptedResponse>(jsonString, options);

                    // Desencriptar la respuesta
                    string decryptedResponse = EncryptionHelper.Decrypt(encryptedResponse.EncryptedData);

                    // Deserializar la lista de resúmenes bancarios
                    List<BankSummary> bankSummaries = JsonSerializer.Deserialize<List<BankSummary>>(decryptedResponse);
                    return bankSummaries;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve bank summaries: " + response.ReasonPhrase);
                    return new List<BankSummary>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve bank summaries: " + ex.Message);
                return new List<BankSummary>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateBankSummaryAsync(BankSummary updatedBankSummary)
        {
            try
            {
                // Serializar el objeto de resumen bancario actualizado a JSON y encriptarlo
                string bankSummaryJson = JsonSerializer.Serialize(updatedBankSummary);
                string encryptedData = EncryptionHelper.Encrypt(bankSummaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/BankSummary/Update";

                // Serializar la solicitud encriptada a JSON y crear el contenido de la solicitud
                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");

                // Realizar la solicitud POST a la API
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to update bank summary: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update bank summary: " + ex.Message);
                return false;
            }
        }
    }
}
