using System;
using System.Collections.Generic;
using Bubble;
using JetBrains.Annotations;
using UnityEngine;

namespace PopIt
{
    public class PopItController : MonoBehaviour
    {
        [SerializeField] private List<BubbleController> _bubbles;
        [SerializeField] private GameObject _resetButtonGO;

        private int BubblesAmount => _bubbles.Count;
        private int _bubblesPopped;
        
        private void Start()
        {
            ResetAllBubbles();
            _resetButtonGO.SetActive(false);
            
            SubscribeToBubbles();
        }

        [UsedImplicitly]
        public void OnResetBubbles()
        {
            ResetAllBubbles();
            _resetButtonGO.SetActive(false);
        }

        private void ResetAllBubbles()
        {
            if (_bubbles == null || _bubbles.Count == 0)
            {
                return;
            }

            foreach (var bubbleController in _bubbles)
            {
                bubbleController.ResetBubble();
            }

            _bubblesPopped = 0;
        }

        private void SubscribeToBubbles()
        {
            foreach (var bubbleController in _bubbles)
            {
                bubbleController.OnPopped += CheckAllowReset;
            }
        }
        
        private void UnsubscribeToBubbles()
        {
            foreach (var bubbleController in _bubbles)
            {
                bubbleController.OnPopped -= CheckAllowReset;
            }
        }

        private void CheckAllowReset()
        {
            _bubblesPopped++;

            if (_bubblesPopped == BubblesAmount)
            {
                _resetButtonGO.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            UnsubscribeToBubbles();
        }
    }
}

