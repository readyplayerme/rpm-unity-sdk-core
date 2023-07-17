﻿using System.IO;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.AvatarLoader.Tests
{
    public static class TestUtils
    {
        public const string GLB_SUFFIX = ".glb";
        public const string JSON_SUFFIX = ".json";

        public const string MODELS_URL_PREFIX = "https://models.readyplayer.me/";
        public const string API_URL_PREFIX = "https://api.readyplayer.me/v1/avatars/";
        public const string TEST_WRONG_GUID = "wrong-guid";
        public static readonly string RelativeTestAvatarsFolder = "/Ready Player Me/Test/Avatars";
        public static readonly string TestAvatarDirectory = $"{Application.persistentDataPath}{RelativeTestAvatarsFolder}";

        public static readonly string TestJsonFilePath =
            $"{DirectoryUtility.GetAvatarSaveDirectory(TestAvatarData.DefaultAvatarUri.Guid)}/test.json";

        public static readonly string MockAvatarGlbWrongPath =
            $"{DirectoryUtility.GetAvatarSaveDirectory(TEST_WRONG_GUID)}/Tests/Common/wrong.glb";

        public static void DeleteAvatarDirectoryIfExists(string avatarGuid, bool recursive = false)
        {
            var path = $"{TestAvatarDirectory}/{avatarGuid}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
            }
        }
    }
}
