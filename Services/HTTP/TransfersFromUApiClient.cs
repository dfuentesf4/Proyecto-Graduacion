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
    public class TransfersFromUApiClient
    {
        private readonly string _apiBaseUrl;

        public TransfersFromUApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateTransfersFromUAsync(TransfersFromU newTransfer)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string transferJson = JsonSerializer.Serialize(newTransfer);
                string encryptedData = EncryptionHelper.Encrypt(transferJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersFromUS/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create transfer: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer creation failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<TransfersFromU>> GetTransfersFromUAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersFromUS/Get";

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

                    // Deserializar la lista de transferencias
                    List<TransfersFromU> transfers = JsonSerializer.Deserialize<List<TransfersFromU>>(decryptedResponse);
                    return transfers;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve transfers: " + response.ReasonPhrase);
                    return new List<TransfersFromU>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve transfers: " + ex.Message);
                return new List<TransfersFromU>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateTransfersFromUAsync(TransfersFromU updatedTransfer)
        {
            try
            {
                // Serializar el objeto de transferencia actualizado a JSON y encriptarlo
                string transferJson = JsonSerializer.Serialize(updatedTransfer);
                string encryptedData = EncryptionHelper.Encrypt(transferJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersFromUS/Update";

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
                    Console.WriteLine("Failed to update transfer: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update transfer: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteTransfersFromUAsync(int transferId)
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/TransfersFromU/Delete/{transferId}";

                // Realizar la solicitud DELETE a la API
                var response = await httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to delete transfer: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete transfer: " + ex.Message);
                return false;
            }
        }
    }
}
