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
    public class SummaryApiClient
    {
        private readonly string _apiBaseUrl;

        public SummaryApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateSummaryAsync(Summary newSummary)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string summaryJson = JsonSerializer.Serialize(newSummary);
                string encryptedData = EncryptionHelper.Encrypt(summaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Summary/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create summary: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Summary insertion failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Summary>> GetSummariesAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Summary/Get";

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

                    // Deserializar la lista de resúmenes
                    List<Summary> summaries = JsonSerializer.Deserialize<List<Summary>>(decryptedResponse);
                    return summaries;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve summaries: " + response.ReasonPhrase);
                    return new List<Summary>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve summaries: " + ex.Message);
                return new List<Summary>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateSummaryAsync(Summary updatedSummary)
        {
            try
            {
                // Serializar el objeto de resumen actualizado a JSON y encriptarlo
                string summaryJson = JsonSerializer.Serialize(updatedSummary);
                string encryptedData = EncryptionHelper.Encrypt(summaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Summary/Update";

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
                    Console.WriteLine("Failed to update summary: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update summary: " + ex.Message);
                return false;
            }
        }
    }
}
