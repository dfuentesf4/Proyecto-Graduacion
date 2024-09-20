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
    public class PettyCashSummaryApiClient
    {
        private readonly string _apiBaseUrl;

        public PettyCashSummaryApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreatePettyCashSummaryAsync(PettyCashSummary newPettyCashSummary)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string pettyCashSummaryJson = JsonSerializer.Serialize(newPettyCashSummary);
                string encryptedData = EncryptionHelper.Encrypt(pettyCashSummaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/PettyCashSummary/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create petty cash summary: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Petty cash summary creation failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<PettyCashSummary>> GetPettyCashSummariesAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/PettyCashSummary/Get";

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

                    // Deserializar la lista de resúmenes de caja chica
                    List<PettyCashSummary> pettyCashSummaries = JsonSerializer.Deserialize<List<PettyCashSummary>>(decryptedResponse);
                    return pettyCashSummaries;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve petty cash summaries: " + response.ReasonPhrase);
                    return new List<PettyCashSummary>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve petty cash summaries: " + ex.Message);
                return new List<PettyCashSummary>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdatePettyCashSummaryAsync(PettyCashSummary updatedPettyCashSummary)
        {
            try
            {
                // Serializar el objeto de resumen de caja chica actualizado a JSON y encriptarlo
                string pettyCashSummaryJson = JsonSerializer.Serialize(updatedPettyCashSummary);
                string encryptedData = EncryptionHelper.Encrypt(pettyCashSummaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/PettyCashSummary/Update";

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
                    Console.WriteLine("Failed to update petty cash summary: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update petty cash summary: " + ex.Message);
                return false;
            }
        }
    }
}
