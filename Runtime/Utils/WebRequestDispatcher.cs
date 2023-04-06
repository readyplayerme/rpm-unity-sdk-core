﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public enum Method
    {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE
    }

    public class WebRequestDispatcher
    {
        public int Timeout = 240;

        public Action<float> ProgressChanged;

        public async Task<T> SendRequest<T>(
            string url,
            Method method,
            Dictionary<string, string> headers = null,
            string payload = null,
            DownloadHandler downloadHandler = default,
            CancellationToken ctx = new CancellationToken()) where T : IResponse, new()
        {
            using var request = new UnityWebRequest();
            request.timeout = Timeout;
            request.url = url;
            request.method = method.ToString();

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            downloadHandler ??= new DownloadHandlerBuffer();

            request.downloadHandler = downloadHandler;

            if (!string.IsNullOrEmpty(payload))
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(payload);
                request.uploadHandler = new UploadHandlerRaw(bytes);
            }

            var startTime = Time.realtimeSinceStartup;
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone && !ctx.IsCancellationRequested)
            {
                await Task.Yield();
                ProgressChanged?.Invoke(request.downloadProgress);
            }

            var response = new T();

            if (ctx.IsCancellationRequested)
            {
                request.Abort();
                response.Parse(false,request);
                return response;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text + "\n" + url);
                response.Parse(false,request);
                return response;
            }

            var requestDuration = Time.realtimeSinceStartup - startTime;
            response.Parse(true,request);

            return response;
        }
    }
}
