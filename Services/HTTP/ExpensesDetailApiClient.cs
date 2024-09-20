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
    public class ExpensesDetailApiClient
    {
        private readonly string _apiBaseUrl;

        public ExpensesDetailApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateExpensesDetailAsync(ExpensesDetail newExpensesDetail)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string expensesDetailJson = JsonSerializer.Serialize(newExpensesDetail);
                string encryptedData = EncryptionHelper.Encrypt(expensesDetailJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/ExpensesDetails/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create expenses detail: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expenses detail insertion failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<ExpensesDetail>> GetExpensesDetailsAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/ExpensesDetails/Get";

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

                    // Deserializar la lista de detalles de gastos
                    List<ExpensesDetail> expensesDetails = JsonSerializer.Deserialize<List<ExpensesDetail>>(decryptedResponse);
                    return expensesDetails;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve expenses details: " + response.ReasonPhrase);
                    return new List<ExpensesDetail>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve expenses details: " + ex.Message);
                return new List<ExpensesDetail>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateExpensesDetailAsync(ExpensesDetail updatedExpensesDetail)
        {
            try
            {
                // Serializar el objeto de detalle de gastos actualizado a JSON y encriptarlo
                string expensesDetailJson = JsonSerializer.Serialize(updatedExpensesDetail);
                string encryptedData = EncryptionHelper.Encrypt(expensesDetailJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/ExpensesDetails/Update";

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
                    Console.WriteLine("Failed to update expenses detail: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update expenses detail: " + ex.Message);
                return false;
            }
        }
    }
}
