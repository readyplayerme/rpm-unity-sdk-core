﻿using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
#if GLTFAST
using GLTFast;
using System;
using System.IO;
#endif

namespace ReadyPlayerMe.Core
{
    public class GltfImporter
    {
        private readonly CancellationToken token;

        public GltfImporter(CancellationToken token)
        {
            this.token = token;
        }

        public async Task<GameObject> Import(string path)
        {
            GameObject avatar = null;
#if GLTFAST
            var data = File.ReadAllBytes(path);
            var gltf = new GltfImport(deferAgent: new UninterruptedDeferAgent());

            var success = await gltf.LoadGltfBinary(
                data,
                new Uri(path),
                cancellationToken: token
            );

            if (success)
            {
                avatar = new GameObject();
                avatar.SetActive(false);
                var customInstantiator = new GltFastGameObjectInstantiator(gltf, avatar.transform);

                await gltf.InstantiateMainSceneAsync(customInstantiator, token);
            }
#endif
            return avatar;
        }

        public async Task<GameObject> Import(byte[] bytes, GLTFDeferAgent gltfDeferAgent = null)
        {
            GameObject avatar = null;
#if GLTFAST
            IDeferAgent agent = gltfDeferAgent == null ? new UninterruptedDeferAgent() : gltfDeferAgent.GetGLTFastDeferAgent();

            var gltf = new GltfImport(deferAgent: agent);
            var success = await gltf.LoadGltfBinary(bytes, cancellationToken: token);
            if (success)
            {
                avatar = new GameObject();
                avatar.SetActive(false);
                var customInstantiator = new GltFastGameObjectInstantiator(gltf, avatar.transform);

                await gltf.InstantiateMainSceneAsync(customInstantiator, token);
            }
#endif
            return avatar;
        }

    }
}
