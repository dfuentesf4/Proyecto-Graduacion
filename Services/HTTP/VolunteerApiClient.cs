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
    public class VolunteerApiClient
    {
        private string _apiBaseUrl;

        public VolunteerApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> CreateVolunteerAsync(Volunteer newVolunteer)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string volunteerJson = JsonSerializer.Serialize(newVolunteer);
                string encryptedData = EncryptionHelper.Encrypt(volunteerJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Volunteers/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to create volunteer: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Volunteer insertion failed: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Volunteer>> GetVolunteersAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Volunteers/Get";

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

                    // Deserializar la lista de voluntarios
                    List<Volunteer> volunteers = JsonSerializer.Deserialize<List<Volunteer>>(decryptedResponse);
                    return volunteers;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve volunteers: " + response.ReasonPhrase);
                    return new List<Volunteer>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve volunteers: " + ex.Message);
                return new List<Volunteer>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateVolunteerAsync(Volunteer updatedVolunteer)
        {
            try
            {
                // Serializar el objeto de voluntario actualizado a JSON y encriptarlo
                string volunteerJson = JsonSerializer.Serialize(updatedVolunteer);
                string encryptedData = EncryptionHelper.Encrypt(volunteerJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Volunteers/Update";

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
                    Console.WriteLine("Failed to update volunteer: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update volunteer: " + ex.Message);
                return false;
            }
        }
    }
}
