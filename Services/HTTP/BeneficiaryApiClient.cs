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
    public class BeneficiaryApiClient
    {
        private string _apiBaseUrl;

        public BeneficiaryApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateBeneficiaryAsync(Beneficiary newBeneficiary)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string beneficiaryJson = JsonSerializer.Serialize(newBeneficiary);
                string encryptedData = EncryptionHelper.Encrypt(beneficiaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Beneficiaries/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create beneficiary: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Beneficiary insertion failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Beneficiary>> GetBeneficiariesAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Beneficiaries/Get";

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

                    // Deserializar la lista de beneficiarios
                    List<Beneficiary> beneficiaries = JsonSerializer.Deserialize<List<Beneficiary>>(decryptedResponse);
                    return beneficiaries;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve beneficiaries: " + response.ReasonPhrase);
                    return new List<Beneficiary>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve beneficiaries: " + ex.Message);
                return new List<Beneficiary>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateBeneficiaryAsync(Beneficiary updatedBeneficiary)
        {
            try
            {
                // Serializar el objeto de beneficiario actualizado a JSON y encriptarlo
                string beneficiaryJson = JsonSerializer.Serialize(updatedBeneficiary);
                string encryptedData = EncryptionHelper.Encrypt(beneficiaryJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Beneficiaries/Update";

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
                    Console.WriteLine("Failed to update beneficiary: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update beneficiary: " + ex.Message);
                return false;
            }
        }
    }
}
