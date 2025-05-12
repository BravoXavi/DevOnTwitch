namespace Tiltify.Responses
{
    [System.Serializable]
    public class TiltifyRetrieveTeamDataResponse
    {
        public TeamData data;
    }
    
    [System.Serializable]
    public class TeamData
    {
        public string id;
        public string name;
        public string description;
        public string url;
        public string slug;
        public Avatar avatar;
        public Social social;
        public TotalAmountRaised total_amount_raised;
        public int legacy_id;
    }
    
    [System.Serializable]
    public class Avatar
    {
        public int width;
        public string alt;
        public string src;
        public int height;
    }
    
    [System.Serializable]
    public class Social
    {
        public string twitch;
        public string twitter;
        public string facebook;
        public string discord;
        public string website;
        public string snapchat;
        public string instagram;
        public string youtube;
        public string tiktok;
    }
    
    [System.Serializable]
    public class TotalAmountRaised
    {
        public string value;
        public string currency;
    }
}