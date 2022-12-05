﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Editor
{
    [InitializeOnLoad]
    public static class EntryPoint
    {
        /// <summary>
        /// Event for when package is imported or when project with package is opened.
        /// </summary>
        public static Action Startup;

        private const string SESSION_STARTED_KEY = "SessionStarted";

        static EntryPoint()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (SessionState.GetBool(SESSION_STARTED_KEY, false)) return;
            SessionState.SetBool(SESSION_STARTED_KEY, true);
            AddRpmDefineSymbol();
            Startup?.Invoke();
#if !DISABLE_AUTO_INSTALLER
            ModuleInstaller.Init();
#endif
            
            EditorApplication.update -= Update;
        }

        [MenuItem("Ready Player Me/Check For Updates")]
        private static void Test()
        {
            ModuleUpdater.GetCurrentRelease();
        }
        
                
        private const string RPM_SYMBOL = "READY_PLAYER_ME";

        private static void AddRpmDefineSymbol()
        {
            BuildTargetGroup target = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defineString = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            var symbols = new HashSet<string>(defineString.Split(';')) { RPM_SYMBOL };
            var newDefineString = string.Join(";", symbols.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, newDefineString);
        }

    }
}
