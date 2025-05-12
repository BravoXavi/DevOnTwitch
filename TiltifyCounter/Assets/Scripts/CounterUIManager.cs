using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public enum CounterBackgroundType
{
    Old,
    New,
    Simple
}

public class CounterUIManager : MonoBehaviour
{
    [SerializeField] private GameObject basePanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private Sprite oldBackground;
    [SerializeField] private Sprite newBackground;
    [SerializeField] private Sprite simpleBackground;
    
    [SerializeField] private Image background;

    [SerializeField] private RectTransform counterTextRectTransform;

    [UsedImplicitly]
    public void OldBackgroundTapped()
    {
        SetupNewBackground(CounterBackgroundType.Old);
        counterTextRectTransform.offsetMin = new Vector2(-80, 0);
    }

    [UsedImplicitly]
    public void SimpleBackgroundTapped()
    {
        SetupNewBackground(CounterBackgroundType.Simple);
        counterTextRectTransform.offsetMin = new Vector2(0, 15);
    }

    [UsedImplicitly]
    public void NewBackgroundTapped()
    {
        SetupNewBackground(CounterBackgroundType.New);
        counterTextRectTransform.offsetMin = new Vector2(0, 15);
    }
    
    private void SetupNewBackground(CounterBackgroundType type)
    {
        switch (type)
        {
            case CounterBackgroundType.New:
                background.sprite = newBackground;
                break;
            case CounterBackgroundType.Old:
                background.sprite = oldBackground;
                break;
            case CounterBackgroundType.Simple:
                background.sprite = simpleBackground;
                break;
        }
    }

    public void OnOpenSettings()
    {
        basePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        settingsPanel.SetActive(false);
        basePanel.SetActive(true);
    }
}