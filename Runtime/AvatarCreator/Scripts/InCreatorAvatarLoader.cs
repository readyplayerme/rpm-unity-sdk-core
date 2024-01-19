﻿using System.Threading.Tasks;
using ReadyPlayerMe.Core;
using UnityEngine;

namespace ReadyPlayerMe.AvatarCreator
{
    /// <summary>
    /// Avatar loader for importing and processing downloaded avatar through Avatar API endpoints.
    /// </summary>
    public class InCreatorAvatarLoader
    {
        public async Task<GameObject> Load(string avatarId, BodyType bodyType, OutfitGender gender, byte[] data)
        {
            var avatarMetadata = new AvatarMetadata();
            avatarMetadata.BodyType = bodyType;
            avatarMetadata.OutfitGender = gender;

            var context = new AvatarContext();
            context.Bytes = data;
            context.AvatarUri.Guid = avatarId;
            context.AvatarCachingEnabled = false;
            context.Metadata = avatarMetadata;

            var executor = new OperationExecutor<AvatarContext>(new IOperation<AvatarContext>[]
            {
                new GltFastAvatarImporter(),
                new AvatarProcessor()
            });
            
            try
            {
                context = await executor.Execute(context);
            }
            catch (CustomException exception)
            {
                throw new CustomException(executor.IsCancelled ? FailureType.OperationCancelled : exception.FailureType, exception.Message);
            }

            var avatar = (GameObject) context.Data;
            avatar.SetActive(true);
            return avatar;
        }
    }
}
