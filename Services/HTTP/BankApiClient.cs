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
    public class BankApiClient
    {
        private readonly string _apiBaseUrl;

        public BankApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateBankAsync(Bank newBank)
        {
            try
            {
                // Serializar el objeto Bank a JSON y encriptarlo
                string bankJson = JsonSerializer.Serialize(newBank);
                string encryptedData = EncryptionHelper.Encrypt(bankJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Banks/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create bank: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bank creation failed: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Bank>> GetBanksAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Banks/Get";

                // Realizar la solicitud GET a la API
                var response = await httpClient.PostAsync(apiUrl,null);

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

                    // Deserializar la lista de bancos
                    List<Bank> banks = JsonSerializer.Deserialize<List<Bank>>(decryptedResponse);
                    return banks;
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve banks: {response.ReasonPhrase}");
                    return new List<Bank>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve banks: {ex.Message}");
                return new List<Bank>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateBankAsync(Bank updatedBank)
        {
            try
            {
                // Serializar el objeto Bank actualizado a JSON y encriptarlo
                string bankJson = JsonSerializer.Serialize(updatedBank);
                string encryptedData = EncryptionHelper.Encrypt(bankJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Banks/Update";

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
                    Console.WriteLine($"Failed to update bank: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update bank: {ex.Message}");
                return false;
            }
        }
    }
}
