namespace Tiltify.Responses
{
    [System.Serializable]
    public class TiltifyAuthenticationResponse
    {
        public string access_token;
        public int expires_in;
        public string refresh_token;
        public string token_type;
    }
}