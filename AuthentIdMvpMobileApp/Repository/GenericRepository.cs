using AuthentIdMvpMobileApp.Interfaces.Repository;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AuthentIdMvpMobileApp.Exceptions;

namespace AuthentIdMvpMobileApp.Repository
{
    public class GenericRepository : IGenericRepository
    {
        public Task DeleteAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAsync<T>(Uri uri)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);
                string jsonResult = string.Empty;

                var responseMessage = await Policy.Handle<WebException>(ex =>
                {
                    Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                    return true;
                })
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .ExecuteAsync(async () => await httpClient.GetAsync(uri));
                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<List<T>>(jsonResult);
                    return json;
                }

                if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }

                throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name + " : " + e.Message}");
                throw;
            }
        }

        public async Task<T> GetByIdAsync<T>(Uri uri)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);
                string jsonResult = string.Empty;

                var responseMessage = await Policy.Handle<WebException>(ex =>
                {
                    Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                    return true;
                })
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .ExecuteAsync(async () => await httpClient.GetAsync(uri));
                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }

                if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }

                if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }

                throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name + " : " + e.Message}");
                throw;
            }
        }

        public async Task<T> PostAsync<T>(Uri uri, T data)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string jsonResult = string.Empty;

                var responseMessage = await Policy.Handle<WebException>(ex =>
                {
                    Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                    return true;
                })
                .WaitAndRetryAsync
                (
                    5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () => await httpClient.PostAsync(uri, content));

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }

                if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }

                throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType().Name + " : " + e.Message}");
                throw;
            }
        }

        public Task<R> PostAsync<T, R>(Uri uri, T data)
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAsync<T>(Uri uri, T data)
        {
            throw new NotImplementedException();
        }

        private HttpClient CreateHttpClient(Uri authToken)
        {

            HttpClient httpClient = new();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(authToken.ToString()))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.ToString());
            }
            return httpClient;
        }
    }
}
