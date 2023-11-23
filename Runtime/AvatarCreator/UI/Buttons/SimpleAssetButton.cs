using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.AvatarCreator
{
    public class SimpleAssetButton : MonoBehaviour
    {
        public Button Button => button;
        public RawImage RawImage => rawImage;

        [SerializeField] private Button button;
        [SerializeField] private RawImage rawImage;

        public void AddListener(Action action)
        {
            button.onClick.AddListener(action.Invoke);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
