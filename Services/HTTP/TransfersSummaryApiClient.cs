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
    public class TransfersSummaryApiClient
    {
        private readonly string _apiBaseUrl;

        public TransfersSummaryApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateTransferSummaryAsync(TransferSummary newTransferSummary)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string transferSummaryJson = JsonSerializer.Serialize(newTransferSummary);
                string encryptedData = EncryptionHelper.Encrypt(transferSummaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersSummary/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create transfer summary: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer summary creation failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<TransferSummary>> GetTransferSummariesAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersSummary/Get";

                // Realizar la solicitud GET a la API
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

                    // Deserializar la lista de resúmenes de transferencias
                    List<TransferSummary> transferSummaries = JsonSerializer.Deserialize<List<TransferSummary>>(decryptedResponse);
                    return transferSummaries;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve transfer summaries: " + response.ReasonPhrase);
                    return new List<TransferSummary>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve transfer summaries: " + ex.Message);
                return new List<TransferSummary>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateTransferSummaryAsync(TransferSummary updatedTransferSummary)
        {
            try
            {
                // Serializar el objeto de resumen de transferencia actualizado a JSON y encriptarlo
                string transferSummaryJson = JsonSerializer.Serialize(updatedTransferSummary);
                string encryptedData = EncryptionHelper.Encrypt(transferSummaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersSummary/Update";

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
                    Console.WriteLine("Failed to update transfer summary: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update transfer summary: " + ex.Message);
                return false;
            }
        }
    }
}
