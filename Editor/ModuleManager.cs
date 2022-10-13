﻿using System;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace ReadyPlayerMe
{
    public static class ModuleManager
    {
        private static ListRequest listRequest;
        private static AddRequest addRequest;
        public static Action OnPackageListUpdate;

        public static void List()
        {
            listRequest = Client.List();
            EditorApplication.update += ListRequestProgress;
        }

        static void ListRequestProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    Debug.Log("success ");
                }
                else if (listRequest.Status >= StatusCode.Failure)
                    Debug.Log(listRequest.Error.message);
                EditorApplication.update -= ListRequestProgress;
                OnPackageListUpdate?.Invoke();
            }
        }

        public static bool CheckPackageExists(string packageName)
        {
            if (listRequest.Status == StatusCode.Success)
                foreach (var package in listRequest.Result)
                {
                    if (package.name == packageName)
                    {
                        Debug.Log("Package found! Name: " + package.name);
                        return true;
                    }

                }
            return false;
        }

        [MenuItem("Window/Add Package Example")]
        public static void Add(string identifier)
        {
            addRequest = Client.Add(identifier);
            EditorApplication.update += AddRequestProgress;
        }

        public static void AddRequestProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (addRequest.Status == StatusCode.Success)
                    Debug.Log("Added Package: " + addRequest.Result.packageId);
                else if (addRequest.Status >= StatusCode.Failure)
                    Debug.Log(addRequest.Error.message);

                EditorApplication.update -= AddRequestProgress;
            }
        }
    }
}
