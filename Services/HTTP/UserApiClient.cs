using HFPMapp.Models;
using HFPMapp.Services.EncryptionService;
using HFPMapp.ViewModels.Login;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HFPMapp.Services.HTTP
{
    public class UserApiClient
    {
        private string _apiBaseUrl;

        public UserApiClient(AppSettings.AppSettings appSettings)
        {
            _apiBaseUrl = appSettings.ApiBaseUrl;
        }

        public async Task<bool> LoginAsync(string userName, string password, LoginViewModel viewModel)
        {
            try
            {
                var loginRequest = new User()
                {
                    UserName = userName,
                    Password = password
                };

                // Serializar el objeto a JSON y encriptarlo
                string loginJson = JsonSerializer.Serialize(loginRequest);
                string encryptedData = EncryptionHelper.Encrypt(loginJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Users/Login";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    var jsonString = Encoding.UTF8.GetString(responseBytes);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true // Esto permite ignorar mayúsculas y minúsculas en los nombres de las propiedades
                    };
                    EncryptedResponse encryptedResponse = JsonSerializer.Deserialize<EncryptedResponse>(jsonString, options);

                    // Desencriptar la respuesta
                    string decryptedResponse = EncryptionHelper.Decrypt(encryptedResponse.EncryptedData);

                    
                    User responseUser = JsonSerializer.Deserialize<User>(decryptedResponse);
                    UserSessionService.CurrentUser = responseUser;
                    return true; 
                }
                else
                {
                    UserSessionService.ClearSession();
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Login failed: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendRecoveryToken(string user)
        {
            try
            {
                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = EncryptionHelper.Encrypt(user)
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Users/RequestPasswordReset";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to send recovery token: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string userName, string newPassword, string token)
        {
            try
            {
                // Crear el objeto de solicitud
                var resetPasswordRequest = new ResetPasswordRequest
                {
                    UserName = userName,
                    NewPassword = newPassword,
                    Token = token
                };

                // Serializar la solicitud a JSON
                string requestData = JsonSerializer.Serialize(resetPasswordRequest);
                string encryptedData = EncryptionHelper.Encrypt(requestData);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Users/ResetPassword";

                // Serializar la solicitud encriptada y crear el contenido de la solicitud
                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");

                // Realizar la solicitud POST a la API
                var response = await httpClient.PostAsync(apiUrl, content);

                // Evaluar si la solicitud fue exitosa
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Manejar excepciones y registrar errores
                Console.WriteLine($"Error al restablecer la contraseña: {ex.Message}");
                return false;
            }
        }

        public async Task<int> CreateUserAsync(User newUser)
        {
            try
            {
                // Serializar el objeto a JSON y encriptarlo
                string userJson = JsonSerializer.Serialize(newUser);
                string encryptedData = EncryptionHelper.Encrypt(userJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Users/Create";

                var content = new StringContent(JsonSerializer.Serialize(encryptedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
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

                    // Deserializar el usuario de la respuesta desencriptada
                    User responseUser = JsonSerializer.Deserialize<User>(decryptedResponse);

                    // Retornar el ID del usuario creado
                    return responseUser.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("User insertion failed: " + ex.Message);
                return 0;
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Users/Get"; 

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
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    };

                    // Deserializar la respuesta en un objeto EncryptedResponse
                    EncryptedResponse encryptedResponse = JsonSerializer.Deserialize<EncryptedResponse>(jsonString, options);

                    // Desencriptar la respuesta
                    string decryptedResponse = EncryptionHelper.Decrypt(encryptedResponse.EncryptedData);

                    // Deserializar la lista de usuarios
                    List<User> users = JsonSerializer.Deserialize<List<User>>(decryptedResponse, options);
                    return users;
                }
                else
                {
                    // Manejar el error de la API
                    Console.WriteLine("Failed to retrieve users: " + response.ReasonPhrase);
                    return new List<User>(); // Retorna una lista vacía en caso de fallo
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine("Failed to retrieve users: " + ex.Message);
                return new List<User>(); // Retorna una lista vacía en caso de excepción
            }
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            try
            {
                // Serializar el objeto de usuario actualizado a JSON y encriptarlo
                string userJson = JsonSerializer.Serialize(updatedUser);
                string encryptedData = EncryptionHelper.Encrypt(userJson);

                // Crear la solicitud encriptada
                var encryptedRequest = new EncryptedRequest
                {
                    EncryptedData = encryptedData
                };

                // Configurar HttpClient
                using var httpClient = new HttpClient();
                string apiUrl = $"{_apiBaseUrl}/Users/Update";

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
                    Console.WriteLine("Failed to update user: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update user: " + ex.Message);
                return false;
            }
        }

    }


    public class ResetPasswordRequest
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
