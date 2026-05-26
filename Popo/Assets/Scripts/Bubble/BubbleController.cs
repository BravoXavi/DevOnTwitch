using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Bubble
{
    public class BubbleController : MonoBehaviour
    {
        [SerializeField] private Button _popButton;
        [SerializeField] private GameObject _poppedOverlay;

        public Action OnPopped;

        private void OnEnable()
        {
            if (_popButton == null || _poppedOverlay == null)
            {
                gameObject.SetActive(false);
                return;
            }

            ResetBubble();
        }

        public void ResetBubble()
        {
            _poppedOverlay.SetActive(false);
            _popButton.interactable = true;
        }

        [UsedImplicitly]
        public void OnBubblePopped()
        {
            _poppedOverlay.SetActive(true);
            _popButton.interactable = false;

            OnPopped?.Invoke();
        }
    }
}

