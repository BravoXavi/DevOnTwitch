using System;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TiltifyCounter : MonoBehaviour
{
    
    private const float BigDuration = 2.0f;
    private const float MidDuration = 1.0f;
    private const float SmallDuration = 0.5f;
    
    [SerializeField] 
    private TextMeshProUGUI _counterText;
    [SerializeField] 
    private TMP_InputField _clientIdParam;
    [SerializeField] 
    private TMP_InputField _clientSecretParam;
    [SerializeField] 
    private TMP_InputField _slugParam;

    [Header("Console")] 
    [SerializeField] private CounterConsole debugConsole;
    
    private TiltifyConnectionHandler _connectionHandler;
    private CancellationTokenSource _reloadLoopCancellationToken;
    private CancellationTokenSource _updateCounterCancellationToken;

    private float _currentCounterValue = 0.00f;
    
    private void Awake()
    {
        _connectionHandler = new TiltifyConnectionHandler();
    }

    private void Start()
    {
        _counterText.text = ConnectionConstants.StoppedMessage;
    }

    public async void OnTap()
    {
        debugConsole.Log("Starting connection!");

        bool success = await UpdateTokenIfNecessary();
        if (!success)
        {
            return;
        }
        
        debugConsole.Log("Retrieving team data...");
        StartReloadLoop();
    }

    private async UniTask<bool> UpdateTokenIfNecessary()
    {
        if (!_connectionHandler.IsAccessTokenFresh())
        {
            bool success = await StartTokenRetrieval();
            if (!success)
            {
                debugConsole.LogError("Error authorizing. Please ensure your ClientID and ClientSecret are correct!");
                return false;
            }
            
            debugConsole.Log("Token obtained! You are authorized! :)");
        }

        return true;
    }

    public void OnTapStopReload()
    {
        StopReloadLoop();
        _counterText.text = ConnectionConstants.StoppedMessage;
        debugConsole.LogWarning("Connection has been stopped.");
    }

    private void StartReloadLoop()
    {
        _reloadLoopCancellationToken = new CancellationTokenSource();
        AutoReloadCounter(5, _reloadLoopCancellationToken.Token).Forget();
    }

    private async UniTask<bool> StartTokenRetrieval()
    {
        debugConsole.Log("Retrieving authorization token...");
            
        _connectionHandler.SetupAuthorizationData(ConnectionConstants.ClientId, ConnectionConstants.ClientSecret);
        return await _connectionHandler.RetrieveToken();
    }

    private async UniTask AutoReloadCounter(int seconds, CancellationToken cancellationToken)
    {
        _updateCounterCancellationToken = new CancellationTokenSource();
        _counterText.text = _currentCounterValue.ToString("F", CultureInfo.InvariantCulture) + " " +
                            ConnectionConstants.Commodity;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!_connectionHandler.IsAccessTokenFresh())
            {
                debugConsole.LogWarning("Auth token has expired. The token will be renewed automatically.");
                bool success = await UpdateTokenIfNecessary();
                if (!success)
                {
                    return;
                }
            }
            
            var teamData = await _connectionHandler.RetrieveTeamData(ConnectionConstants.EcosDelCristalSlug);
            var newBalance = float.Parse(teamData.total_amount_raised.value);
            
            //Do the animation (the value is already stored in case the user stops the execution)
            UpdateWalletAsync(_currentCounterValue, newBalance, _updateCounterCancellationToken.Token).Forget();
            
            //Update the value before doing any animation
            _currentCounterValue = newBalance;
            debugConsole.LogSuccess("Counter successfully updated!");
            
            var milliseconds = seconds * 1000;
            await UniTask.Delay(milliseconds, cancellationToken: cancellationToken);
        }
    }
    
    private void StopReloadLoop()
    {
        _reloadLoopCancellationToken?.Cancel();
        _reloadLoopCancellationToken?.Dispose();
        _reloadLoopCancellationToken = null;
        
        _updateCounterCancellationToken?.Cancel();
        _updateCounterCancellationToken?.Dispose();
        _updateCounterCancellationToken = null;
    }

    private async UniTaskVoid UpdateWalletAsync(float previousValue, float newValue, CancellationToken cancellationToken)
    {
        var difference = newValue - previousValue;
        if (difference < 0.01f)
        {
            return;
        }
        
        var timeInLerp = 0.0f;
        var duration = GetDurationForDifference(difference);
        
        while (timeInLerp < duration && !cancellationToken.IsCancellationRequested)
        {        
            var newLerpedValue = Mathf.Lerp(previousValue, newValue, timeInLerp / duration);
            _counterText.text = Math.Round(newLerpedValue, 2).ToString("F", CultureInfo.InvariantCulture) + " " + ConnectionConstants.Commodity;
            
            timeInLerp += Time.deltaTime;
            await UniTask.Yield();    
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            //Correct the deviation
            _counterText.text = newValue.ToString("F", CultureInfo.InvariantCulture) + " " + ConnectionConstants.Commodity;
        }
    }

    private float GetDurationForDifference(float diff)
    {
        if (diff > 850)
        {
            return BigDuration;
        }

        if (diff > 200)
        {
            return MidDuration;
        }

        return SmallDuration;
    }
}
