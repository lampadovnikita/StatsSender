using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.CharacterStatsSemder.Scripts.Net
{
    public struct ServerResponse
    {
        public UnityWebRequest.Result Result { get; private set; }

        public string Text { get; private set; }

        public bool IsSuccess => Result == UnityWebRequest.Result.Success;

        public ServerResponse(UnityWebRequest.Result result, string text)
        {
            Result = result;

            Text = text;
        }
    }
}