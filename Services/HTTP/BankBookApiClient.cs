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
    public class BankBookApiClient
    {
        private readonly string _apiBaseUrl;

        public BankBookApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateBankBookAsync(BankBook newBankBook)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string bankBookJson = JsonSerializer.Serialize(newBankBook);
                string encryptedData = EncryptionHelper.Encrypt(bankBookJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/BankBook/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create BankBook: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("BankBook creation failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<BankBook>> GetBankBooksAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/BankBook/Get";

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

                    // Deserializar la lista de BankBooks
                    List<BankBook> bankBooks = JsonSerializer.Deserialize<List<BankBook>>(decryptedResponse);
                    return bankBooks;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve BankBooks: " + response.ReasonPhrase);
                    return new List<BankBook>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve BankBooks: " + ex.Message);
                return new List<BankBook>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateBankBookAsync(BankBook updatedBankBook)
        {
            try
            {
                // Serializar el objeto de BankBook actualizado a JSON y encriptarlo
                string bankBookJson = JsonSerializer.Serialize(updatedBankBook);
                string encryptedData = EncryptionHelper.Encrypt(bankBookJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/BankBook/Update";

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
                    Console.WriteLine("Failed to update BankBook: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update BankBook: " + ex.Message);
                return false;
            }
        }
    }
}
