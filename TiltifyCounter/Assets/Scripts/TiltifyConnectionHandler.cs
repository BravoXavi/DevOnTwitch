using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Tiltify.Responses;
using UnityEngine;

public class TiltifyConnectionHandler
{
    private readonly HttpClient _httpClient = new();

    private string _accessToken = string.Empty;
    private float _timeOfLastRetrieval = 0.0f;
    private float _tokenExpirationTime = 0.0f;

    private string _clientId;
    private string _clientSecret;
    
    public void SetupAuthorizationData(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }
    
    public async UniTask<bool> RetrieveToken(bool forceRetrieve = false)
    {
        if (!forceRetrieve && _accessToken != string.Empty && IsAccessTokenFresh())
        {
            return true;
        }

        var response = await TryAuthentify();
        if (response == null)
        {
            Debug.Log("Error retrieving token. Response is null.");
            return false;
        }

        _accessToken = response.access_token;
        _tokenExpirationTime = response.expires_in;
        _timeOfLastRetrieval = Time.realtimeSinceStartup;
        return true;
    }

    public bool IsAccessTokenFresh()
    {
        if (_accessToken == string.Empty)
        {
            return false;
        }

        var timePassed = Time.realtimeSinceStartup - _timeOfLastRetrieval;
        return timePassed < _tokenExpirationTime;
    }

    private async UniTask<TiltifyAuthenticationResponse> TryAuthentify()
    {
        Dictionary<string, string> requestBody = new()
        {
            { "grant_type", "client_credentials" },
            { "client_id", _clientId },
            { "client_secret", _clientSecret}
        };
        
        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(ConnectionConstants.TokenRequestUrl, content);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            //TODO Return the type of error
            return null;
        }
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        return TiltifyResponseHelper.JsonToAuthenticationResponse(responseContent);
    }
    
    public async UniTask<TeamData> RetrieveTeamData(string teamSlug)
    {
        string url = ConnectionConstants.TeamRequestUrl + teamSlug;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        
        var httpResponse = await _httpClient.GetAsync(url);
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        var parsedResponse = TiltifyResponseHelper.JsonToTeamDataResponse(responseContent);
        
        return parsedResponse.data;
    }
}
