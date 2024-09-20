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
    public class FolderBankApiClient
    {
        private readonly string _apiBaseUrl;

        public FolderBankApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateFolderBankAsync(FolderBank newFolderBank)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string folderBankJson = JsonSerializer.Serialize(newFolderBank);
                string encryptedData = EncryptionHelper.Encrypt(folderBankJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/FolderBank/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create FolderBank: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FolderBank insertion failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<FolderBank>> GetFolderBanksAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/FolderBank/Get";

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

                    // Deserializar la lista de FolderBanks
                    List<FolderBank> folderBanks = JsonSerializer.Deserialize<List<FolderBank>>(decryptedResponse);
                    return folderBanks;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve FolderBanks: " + response.ReasonPhrase);
                    return new List<FolderBank>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve FolderBanks: " + ex.Message);
                return new List<FolderBank>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateFolderBankAsync(FolderBank updatedFolderBank)
        {
            try
            {
                // Serializar el objeto de FolderBank actualizado a JSON y encriptarlo
                string folderBankJson = JsonSerializer.Serialize(updatedFolderBank);
                string encryptedData = EncryptionHelper.Encrypt(folderBankJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/FolderBank/Update";

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
                    Console.WriteLine("Failed to update FolderBank: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update FolderBank: " + ex.Message);
                return false;
            }
        }
    }
}
