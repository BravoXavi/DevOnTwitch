using Tiltify.Responses;
using UnityEngine;

public static class TiltifyResponseHelper
{
    public static TiltifyAuthenticationResponse JsonToAuthenticationResponse(string rawJson)
        => JsonUtility.FromJson<TiltifyAuthenticationResponse>(rawJson);

    public static TiltifyRetrieveTeamDataResponse JsonToTeamDataResponse(string rawJson)
    {
        var response = JsonUtility.FromJson<TiltifyRetrieveTeamDataResponse>(rawJson);
        return response;
    }
}