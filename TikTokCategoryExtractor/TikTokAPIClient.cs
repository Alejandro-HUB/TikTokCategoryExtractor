using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using System.Net;
using System.Web;
using TikTokCategoryExtractor.Interfaces;
using TikTokCategoryExtractor.Responses;

namespace TikTokCategoryExtractor
{
    public class TikTokAPIClient
    {
        private readonly Uri _baseURI;
        private string _accessToken;
        private string _accessTokenExpiresIn;
        private string _refreshToken;
        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly string _apiVersion;
        private readonly Dictionary<string, string> _commonParameters;
        public static readonly List<string> _errorResponseMessages = new List<string>
        {
            "access_token is invalid",
            "access_token is expired",
            "authoirzaition is expired",
            "authorization is expired",
            "access_token"
        };
        public TikTokAPIClient(Uri baseURI, string accessToken,
            string accessTokenExpiresIn, string refreshToken,
            string appKey, string appSecret, string apiVersion)
        {
            _baseURI = baseURI;
            _accessToken = accessToken;
            _accessTokenExpiresIn = accessTokenExpiresIn;
            _refreshToken = refreshToken;
            _appKey = appKey;
            _appSecret = appSecret;
            _apiVersion = apiVersion;
            _commonParameters = new Dictionary<string, string>
            {
                { "app_key", _appKey },
                { "timestamp", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString() },
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
         Dictionary<string, string> queryParameters = null, bool useCommonParameters = true, string shop_cipher = null) where T : ITikTokResponse, new()
        {
            int retryCount = 0;
            int retryTime = 3000; //Miliseconds (3 seconds)
            bool invalidTimeStamp = false;
            var responseString = string.Empty;
            T responseContent = new T();

        RetryTimestamp:

            try
            {
                // If no access token, throw exception to not make unnecessary API call but continue compilation
                if (string.IsNullOrEmpty(_accessToken))
                {
                    throw new Exception("No TikTok Access Token Found");
                }

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
                        // The new API version "202312" requires shop_cipher and uses different authentication
                        if (!string.IsNullOrEmpty(shop_cipher))
                        {
                            // Add shop cipher
                            queryParams.Add("shop_cipher", shop_cipher);

                            // Update version "202212" parameter to "202312"
                            queryParams["version"] = "202312";

                            // Add access token as a header as well
                            client.DefaultRequestHeaders.Add("x-tts-access-token", _accessToken);
                        }

                        // Initialize params object
                        if (queryParameters == null) { queryParameters = new Dictionary<string, string>(); }

                        // Update the request time stamp
                        queryParams["timestamp"] = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString();

                        foreach (var param in queryParams)
                        {
                            if (!queryParameters.ContainsKey(param.Key))
                            {
                                queryParameters.Add(param.Key, param.Value);
                            }
                            else
                            {
                                queryParameters[param.Key] = param.Value;
                            }
                        }

                        // Generate Signature
                        var signature = GenerateRequestSignature(queryParams, uri);

                        // Check if "sign" key already exists and update it, or add it if it doesn't exist
                        if (queryParameters.ContainsKey("sign"))
                        {
                            queryParameters["sign"] = signature;
                        }
                        else
                        {
                            queryParameters.Add("sign", signature);
                        }
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
                    if ((requestMethod == HttpMethod.Post
                        || requestMethod == HttpMethod.Put
                        || requestMethod == HttpMethod.Delete)
                        && !string.IsNullOrEmpty(body))
                    {
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    //The timestamp is invalid intermittently and sleeping before getting the response fixes this
                    if (retryCount > 0)
                    {
                        Thread.Sleep(retryTime);
                    }

                    // Get Response
                    var response = client.SendAsync(request).Result;
                    responseString = response.Content.ReadAsStringAsync().Result;

                    //Verify error type
                    invalidTimeStamp = !string.IsNullOrEmpty(responseString)
                        && (responseString.ToLower().Contains("timestamp") || responseString.ToLower().Contains("sign"));
                    int maxRetryTime = invalidTimeStamp ? 2 : 1;

                    if (!string.IsNullOrEmpty(responseString) && !ValidateAccessToken(responseString))
                    {
                        responseContent.IsSuccess = false;
                        responseContent.Message = responseString;
                        return responseContent;
                    }
                    else if (response.StatusCode == HttpStatusCode.OK)
                    {
                        responseContent = JsonConvert.DeserializeObject<T>(responseString);
                        responseContent.IsSuccess = true;
                        responseContent.Message = responseString;
                    }
                    else if (response.StatusCode != HttpStatusCode.OK && retryCount <= maxRetryTime)
                    {
                        retryCount++;
                        retryTime = invalidTimeStamp ? (retryTime + 3000) : retryTime; // Add 3 seconds to rety time if invalid timestamp
                        if (queryParameters.ContainsKey("timestamp")) { queryParameters.Remove("timestamp"); }
                        if (queryParameters.ContainsKey("sign")) { queryParameters.Remove("sign"); }
                        goto RetryTimestamp;
                    }
                    else
                    {
                        responseContent.IsSuccess = false;
                        responseContent.Message = $"{errorMessage}, EX: {responseString}";
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

        /// <summary>
        /// Convert unixTimeStamp to DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        protected DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        /// <summary>
        /// Makes an API call and checks the expiry date to check if the access token is expired.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void RefreshAccessToken()
        {
            string? accessTokenResponse = null;
            string? refreshTokenResponse = null;
            string? accessTokenExpiresInResponse = null;

            //If no access token, throw exception but do not stop processing to allow feed compilation
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new Exception("No TikTok Access Token Found");
            }

            //Refresh Token - verified through the expiry date and by making an API call
            var response = SendRequest<AuthorizedShop>(HttpMethod.Get, "/api/shop/get_authorized_shop",
                                        null, $"Failed to verify access token validity");
            var isTokenValid = ValidateAccessToken(response?.Message ?? "", false);

            //Check if the access token expiry date is within 24 hours from now or the expire date has already passed
            if (_accessTokenExpiresIn != null
                && long.TryParse(_accessTokenExpiresIn, out long unixTimeStamp)
                && _accessToken != null)
            {
                DateTime expiryDateTime = UnixTimeStampToDateTime(unixTimeStamp);
                TimeSpan timeDifference = expiryDateTime.Date.AddDays(1) - DateTime.Today;
                if (timeDifference <= TimeSpan.FromHours(24) || DateTime.Now >= expiryDateTime || !isTokenValid)
                {
                    (accessTokenResponse, refreshTokenResponse, accessTokenExpiresInResponse) = getAndSaveNewAccessToken();

                    //Failed to refresh the expired access token 
                    if (string.IsNullOrEmpty(accessTokenResponse) || string.IsNullOrEmpty(refreshTokenResponse) || string.IsNullOrEmpty(accessTokenExpiresInResponse))
                    { throw new Exception("The TikTok Access Token is Expired and the Operation to Refresh the Access Token Failed."); }
                    else
                    {
                        _accessToken = accessTokenResponse;
                        _refreshToken = refreshTokenResponse;
                        _accessTokenExpiresIn = accessTokenExpiresInResponse;
                    }
                }
            }
        }

        /// <summary>
        /// Generates new access token and saves it to the database
        /// </summary>
        /// <returns></returns>
        protected (string?, string?, string?) getAndSaveNewAccessToken()
        {
            try
            {
                //Create a new client that has a different _baseURI
                var accessTokenClient = new TikTokAPIClient(new Uri("https://auth.tiktok-shops.com"), _accessToken, _accessTokenExpiresIn, _refreshToken, _appKey, _appSecret, _apiVersion);

                //Set query params
                var queryParams = new Dictionary<string, string>()
                {
                    { "app_key", _appKey },
                    { "app_secret", _appSecret },
                    { "refresh_token", _refreshToken },
                    { "grant_type", "refresh_token" }
                };

                var refreshAccessTokenResponse = accessTokenClient.SendRequest<RefreshAccessTokenResponse>(HttpMethod.Get,
                    "/api/v2/token/refresh", null,
                    "Failed to GET TikTok Access Token", null, queryParams, false);

                if (refreshAccessTokenResponse.IsSuccess
                    && refreshAccessTokenResponse?.data?.access_token != null
                    && refreshAccessTokenResponse?.data?.refresh_token != null
                    && refreshAccessTokenResponse?.data?.access_token_expire_in != null)
                {
                    //Refresh variables
                    return (refreshAccessTokenResponse?.data?.access_token, refreshAccessTokenResponse?.data?.refresh_token, refreshAccessTokenResponse?.data?.access_token_expire_in.ToString());
                }
                else
                {
                    return (null, null, null);
                }
            }
            catch (Exception)
            {
                return (null, null, null);
            }
        }

        public bool ValidateAccessToken(string response, bool throwException = true)
        {
            if (!string.IsNullOrEmpty(response) && _errorResponseMessages.Any(x => response.ToLower().Contains(x)))
            {
                if (throwException)
                {
                    throw new Exception("The TikTok Access Token is expired");
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}