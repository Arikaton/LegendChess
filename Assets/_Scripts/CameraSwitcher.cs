using System;
using UnityEngine;

namespace LegendChess
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private Transform whiteViewPosition;
        [SerializeField] private Transform blackViewPosition;
        private bool blackViewActive;

        private void Start()
        {
            transform.position = whiteViewPosition.position;
            transform.rotation = whiteViewPosition.rotation;
        }

        public void SwitchPosition()
        {
            var destination = blackViewActive ? whiteViewPosition : blackViewPosition;
            blackViewActive = !blackViewActive;
            transform.position = destination.position;
            transform.rotation = destination.rotation;
        }
    }
}