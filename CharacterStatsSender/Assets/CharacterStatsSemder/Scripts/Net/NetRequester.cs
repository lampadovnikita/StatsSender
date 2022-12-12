using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.CharacterStatsSemder.Scripts.Net
{
    public static class NetRequester
    {
        private static readonly int requestTimeout = 15;

        public static string BearerToken;

        public static async Task<ServerResponse> GetRequest(string uri)
        {
            var t = new System.Diagnostics.Stopwatch();
            t.Start();

            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            webRequest.timeout = requestTimeout;

            if (string.IsNullOrEmpty(BearerToken) == false)
            {
                webRequest.SetRequestHeader("Authorization", "Bearer " + BearerToken);
            }

            webRequest.SendWebRequest();

            while (webRequest.isDone == false)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"[{uri}] Connection Error: {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"[{uri}] Data Processing Error: {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"[{uri}] Protocol Error: {webRequest.error}");
                        break;
                }
            }
            else
            {
                Debug.Log($"[{uri}] Success: {webRequest.downloadHandler.text}");

                t.Stop();
                Debug.Log($"{uri}] Request lasted for [{(double)t.ElapsedMilliseconds / 1000:0.000}s].");
            }

            ServerResponse serverResponse = new ServerResponse(webRequest.result, webRequest.downloadHandler.text);            
            return serverResponse;
        }

        public static async Task<ServerResponse> PostRequest(string uri, string data)
        {
            var t = new System.Diagnostics.Stopwatch();
            t.Start();

            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            using UnityWebRequest webRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST);
            webRequest.timeout = requestTimeout;
            
            UploadHandlerRaw uh = new UploadHandlerRaw(dataBytes);
            uh.contentType = "application/json";
            webRequest.uploadHandler = uh;

            webRequest.downloadHandler = new DownloadHandlerBuffer();

            if (string.IsNullOrEmpty(BearerToken) == false)
            {
                webRequest.SetRequestHeader("Authorization", "Bearer " + BearerToken);
            }

            webRequest.SendWebRequest();

            while (webRequest.isDone == false)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"[{uri}] Connection Error: {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"[{uri}] Data Processing Error: {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"[{uri}] Protocol Error: {webRequest.error}");
                        break;
                }
            }
            else
            {
                Debug.Log($"[{uri}] Success: {webRequest.downloadHandler.text}");

                t.Stop();
                Debug.Log($"{uri}] Request lasted for [{(double)t.ElapsedMilliseconds / 1000:0.000}s].");
            }

            ServerResponse serverResponse = new ServerResponse(webRequest.result, webRequest.downloadHandler.text);
            return serverResponse;
        }

        //private IEnumerator PostRequestRoutine<T>(string uri, string contentToSend, Action<bool, T> completedCallback)
        //{
        //    var t = new System.Diagnostics.Stopwatch();
        //    t.Start();

        //    bool isRequestSuccessful = true;

        //    using var webRequest = UnityWebRequest.Post(uri, UnityWebRequest.kHttpVerbPOST);
        //    webRequest.SetRequestHeader("Authorization", $"Bearer {m_BearerToken}");
        //    webRequest.timeout = m_RequestTimeout;

        //    if (string.IsNullOrEmpty(contentToSend) == false)
        //    {
        //        Debug.Log($"[{uri}] Send post request with: {contentToSend}");

        //        var bytes = System.Text.Encoding.UTF8.GetBytes(contentToSend);
        //        webRequest.uploadHandler = new UploadHandlerRaw(bytes) { contentType = "application/json" };
        //    }
        //    else
        //    {
        //        Debug.Log($"[{uri}] Send Post request without content to send");
        //    }

        //    yield return webRequest.SendWebRequest();

        //    T loadedData = default;
        //    if (webRequest.result == UnityWebRequest.Result.Success)
        //    {
        //        Debug.Log($"[{uri}] Success: {webRequest.downloadHandler.text}");

        //        DefaultServerResponseJson<T> loadedDefaultData
        //            = Converter.Newtonsoft.Deserialize<DefaultServerResponseJson<T>>(webRequest.downloadHandler.text);

        //        loadedData = loadedDefaultData.Data;

        //        int requestStatusCode;
        //        bool canParseStatusCode = loadedDefaultData.GetStatus(out requestStatusCode);

        //        if (canParseStatusCode == false)
        //        {
        //            Debug.LogError($"[{uri}] Can't parse status!");

        //            isRequestSuccessful = false;
        //        }

        //        if (requestStatusCode != m_SuccessfulRequestStatusCode)
        //        {
        //            Debug.LogError($"[{uri}] Status = {requestStatusCode}, isn't success!");

        //            isRequestSuccessful = false;
        //        }
        //    }
        //    else
        //    {
        //        isRequestSuccessful = false;

        //        switch (webRequest.result)
        //        {
        //            case UnityWebRequest.Result.ConnectionError:
        //                Debug.LogError($"[{uri}] Connection Error: {webRequest.error}");
        //                break;
        //            case UnityWebRequest.Result.DataProcessingError:
        //                Debug.LogError($"[{uri}] Data Processing Error: {webRequest.error}");
        //                break;
        //            case UnityWebRequest.Result.ProtocolError:
        //                Debug.LogError($"[{uri}] Protocol Error: {webRequest.error}");
        //                break;
        //        }
        //    }

        //    t.Stop();
        //    Debug.Log($"{uri}] Request lasted for [{(double)t.ElapsedMilliseconds / 1000:0.000}s].");

        //    completedCallback?.Invoke(isRequestSuccessful, loadedData);
        //}

    }
}