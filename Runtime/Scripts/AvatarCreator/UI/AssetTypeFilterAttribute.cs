using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReadyPlayerMe.AvatarCreator
{
    public class AssetTypeFilterAttribute : PropertyAttribute
    {
        public AssetFilter filter;

        public AssetTypeFilterAttribute(AssetFilter filter)
        {
            this.filter = filter;
        }
    }
    
}
