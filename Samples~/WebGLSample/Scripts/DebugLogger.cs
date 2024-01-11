using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe.Samples
{
    public class DebugLogger : MonoBehaviour
    {
        [SerializeField] private Text debugText;

        public void LogMessage(string messages)
        {
            debugText.text = messages;
        }
    }
}