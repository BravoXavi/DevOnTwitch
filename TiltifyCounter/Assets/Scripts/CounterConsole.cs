using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterConsole : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI consoleTextbox;

    [SerializeField] 
    private ScrollRect scrolLRect;

    public void Log(string text, string colorCode = "white", bool forceScroll = false)
    {
        if (consoleTextbox == null)
        {
            return;
        }
        
        string currentTime = System.DateTime.Now.ToString("HH:mm:ss");
        consoleTextbox.text += "[" + currentTime + "] " + 
                        "<color=" + colorCode + ">" +
                        text +
                        "</color> \n";

        if (forceScroll)
        {
            ScrollToBottom();
        }
    }

    public void LogError(string text)
    {
        Log(text, "red", true);
    }

    public void LogWarning(string text)
    {
        Log(text, "yellow", true);
    }
    
    public void LogSuccess(string text)
    {
        Log(text, "green", true);
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        scrolLRect.verticalNormalizedPosition = 0;
    }
}