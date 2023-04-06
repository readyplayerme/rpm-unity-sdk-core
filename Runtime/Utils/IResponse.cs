using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public interface IResponse
    {
        bool IsSuccess { get;  }
        string Error { get; }
        void Parse(bool isSuccess, UnityWebRequest request);
    }
}
