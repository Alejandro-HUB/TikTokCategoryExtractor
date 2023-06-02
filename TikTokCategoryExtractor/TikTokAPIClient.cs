using Newtonsoft.Json;
using TikTokCategoryExtractor.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using System.Net;
using System.Web;

namespace TikTokCategoryExtractor
{
    public class TikTokAPIClient
    {
        private readonly Uri _baseURI;
        private readonly string _accessToken;
        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly string _apiVersion;
        private readonly Dictionary<string, string> _commonParameters;
        public TikTokAPIClient(Uri baseURI, string accessToken,
            string appKey, string appSecret, string apiVersion)
        {
            _baseURI = baseURI;
            _accessToken = accessToken;
            _appKey = appKey;
            _appSecret = appSecret;
            _apiVersion = apiVersion;
            _commonParameters = new Dictionary<string, string>
            {
                { "app_key", _appKey },
                { "timestamp", ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString() },
                { "access_token", _accessToken },
                { "version", _apiVersion },
            };
        }
        /// <summary>
        /// API Request Method For Tik Tok
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestMethod"></param>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <param name="errorMessage"></param>
        /// <param name="headers"></param>
        /// <param name="queryParameters"></param>
        /// <param name="useCommonParameters"></param>
        /// <returns>T</returns>
        public T SendRequest<T>(HttpMethod requestMethod, string uri,
        string body, string errorMessage, Dictionary<string, string> headers = null,
        Dictionary<string, string> queryParameters = null, bool useCommonParameters = true) where T : ITikTokResponse, new()
        {
            T responseContent = new T();

            try
            {
                // Create a new HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Add headers to request
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    //Query Parameters
                    var queryParams = useCommonParameters ? new Dictionary<string, string>(_commonParameters) : new Dictionary<string, string>();
                    if (queryParameters != null)
                    {
                        foreach (var queryParam in queryParameters)
                        {
                            queryParams[queryParam.Key] = queryParam.Value;
                        }
                    }

                    // Add Common Query Parameters
                    if (useCommonParameters)
                    {
                        // Update the request time stamp
                        queryParams["timestamp"] = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString();

                        foreach (var param in queryParams)
                        {
                            if (queryParameters == null || !queryParameters.ContainsKey(param.Key))
                            {
                                queryParameters = queryParameters ?? new Dictionary<string, string>();
                                queryParameters.Add(param.Key, param.Value);
                            }
                        }

                        // Generate Signature
                        var signature = GenerateRequestSignature(queryParams, uri);
                        queryParameters.Add("sign", signature);
                    }

                    // Add Query Parameters to Request Uri
                    var requestUriBuilder = new UriBuilder(getURI(_baseURI, uri));
                    if (queryParameters != null)
                    {
                        var queryParamBuilder = HttpUtility.ParseQueryString(requestUriBuilder.Query);
                        foreach (var queryParam in queryParameters)
                        {
                            queryParamBuilder[queryParam.Key] = queryParam.Value;
                        }

                        requestUriBuilder.Query = queryParamBuilder.ToString();
                    }

                    // Create a new HttpRequestMessage with the specified HttpMethod and Request Uri
                    var request = new HttpRequestMessage(requestMethod, requestUriBuilder.Uri);

                    // Add Body to Request
                    if ((requestMethod == HttpMethod.Post || requestMethod == HttpMethod.Put)
                        && !string.IsNullOrEmpty(body))
                    {
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    // Get Response
                    var response = client.SendAsync(request).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        responseContent = JsonConvert.DeserializeObject<T>(responseString);
                        responseContent.IsSuccess = true;
                        responseContent.Message = responseString;
                    }
                    else
                    {
                        responseContent.IsSuccess = false;
                        responseContent.Message = $"{errorMessage}, EX: {response.StatusCode}";
                    }
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                responseContent.IsSuccess = false;
                responseContent.Message = $"{errorMessage}, EX: {ex}";
                return responseContent;
            }
        }
        /// <summary>
        /// Generates Tik Tok's required request signature
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected string GenerateRequestSignature(Dictionary<string, string> queryParameters, string uri)
        {
            //Reorder the params based on alphabetical order.
            var keys = queryParameters.Keys.ToList();
            keys.Sort();

            //Concat all the param in the format of {key}{value} and append the request path to the beginning
            var input = uri;
            foreach (var key in keys)
            {
                //Skip these parameters
                if (key.ToLower().Equals("sign") || key.ToLower().Equals("access_token"))
                {
                    continue;
                }

                input += key + queryParameters[key];
            }

            //Wrap string generated in up with app_secret.
            input = _appSecret + input + _appSecret;

            //Encode the digest byte stream in hexadecimal and use sha256 to generate sign with salt(secret)
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_appSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Joins a base uri to a path
        /// </summary>
        /// <param name="baseURI"></param>
        /// <param name="postURI"></param>
        /// <returns></returns>
        protected string getURI(Uri baseURI, string postURI)
        {
            try
            {
                var uri = new Uri(baseURI, postURI).OriginalString;
                return uri;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}