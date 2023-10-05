using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ReadyPlayerMe.Core.Editor
{
    public abstract class AvatarRigCreator
    {
        private const string TAG = nameof(AvatarRigCreator);

        private const string ANIMATION_RIGGING_PACKAGE_NAME = "com.unity.animation.rigging";
        private const string DIALOG_TITLE = "Read Player Me";
        private const string DIALOG_MESSAGE = "This will require installing the Animation Rigging package. Do you want to continue?";
        private const string DIALOG_OK = "Ok";
        private const string DIALOG_CANCEL = "Cancel";

        private const float HINT_OFFSET_Z = 0.08f;

        [MenuItem("Ready Player Me/Add Avatar Rig")]
        public static void AddAvatarRig()
        {
            var selectedObjects = Selection.gameObjects;
            var avatars = selectedObjects.Where(x => x.GetComponent<AvatarData>() != null).ToList();

            if (avatars.Any())
            {
                GetAnimationRiggingPackage();
            }

            foreach (var avatar in avatars)
            {
                SetupRig(avatar);
            }
        }

        private static void SetupRig(GameObject avatar)
        {
            AddBoneRenderer(avatar);

            var rig = AddRigBuilder(avatar);

            SetupRightHand(avatar.transform, rig.transform);
            SetupLeftHand(avatar.transform, rig.transform);
            SetupHead(avatar.transform, rig.transform);
        }

        private static void AddBoneRenderer(GameObject avatar)
        {
            var boneRenderer = avatar.AddComponent<BoneRenderer>();
            var hips = FindTransform(avatar.transform, "Hips");
            var children = GetAllChildren(hips);
            children.Add(hips);
            boneRenderer.transforms = children.ToArray();
        }

        private static GameObject AddRigBuilder(GameObject avatar)
        {
            var rigBuilder = avatar.AddComponent<RigBuilder>();
            var rigPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Ready Player Me/Core/Editor/Prefabs/Rig.prefab");
            var rig = (GameObject) PrefabUtility.InstantiatePrefab(rigPrefab, avatar.transform);
            rigBuilder.layers.Add(new RigLayer(rig.GetComponent<Rig>()));
            return rig;
        }

        private static void SetupLeftHand(Transform avatar, Transform rig)
        {
            var leftArmIK = rig.GetChild(1).GetComponent<TwoBoneIKConstraint>();
            var leftForeArmBone = FindTransform(avatar, "LeftForeArm");
            var leftHandBone = FindTransform(avatar, "LeftHand");
            leftArmIK.data.root = FindTransform(avatar, "LeftArm");
            leftArmIK.data.mid = leftForeArmBone;
            leftArmIK.data.tip = leftHandBone;
            leftArmIK.data.target.position = leftHandBone.position;
            var leftForeArmPos = leftForeArmBone.position;
            leftArmIK.data.hint.position = new Vector3(leftForeArmPos.x, leftForeArmPos.y, leftForeArmPos.z - HINT_OFFSET_Z);
        }

        private static void SetupRightHand(Transform avatar, Transform rig)
        {
            var rightArmIK = rig.GetChild(0).GetComponent<TwoBoneIKConstraint>();
            var rightForeArmBone = FindTransform(avatar, "RightForeArm");
            var rightHandBone = FindTransform(avatar, "RightHand");
            rightArmIK.data.root = FindTransform(avatar, "RightArm");
            rightArmIK.data.mid = rightForeArmBone;
            rightArmIK.data.tip = rightHandBone;
            rightArmIK.data.target.position = rightHandBone.position;
            var rightForeArmPos = rightForeArmBone.position;
            rightArmIK.data.hint.position = new Vector3(rightForeArmPos.x, rightForeArmPos.y, rightForeArmPos.z - HINT_OFFSET_Z);
        }

        private static void SetupHead(Transform avatar, Transform rig)
        {
            var headBone = FindTransform(avatar, "Head");
            var headIK = rig.GetChild(2).GetComponent<MultiParentConstraint>();
            headIK.data.constrainedObject = headBone;
            headIK.data.sourceObjects.Add(new WeightedTransform(headIK.transform, 1));
            headIK.transform.position = headBone.position;
        }

        private static Transform FindTransform(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;
                var result = FindTransform(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }

        private static List<Transform> GetAllChildren(Transform parent)
        {
            var children = new List<Transform>();

            foreach (Transform child in parent)
            {
                children.Add(child);

                if (child.childCount > 0)
                {
                    children.AddRange(GetAllChildren(child));
                }
            }

            return children;
        }

        private static void GetAnimationRiggingPackage()
        {
            if (PackageManagerHelper.IsPackageInstalled(ANIMATION_RIGGING_PACKAGE_NAME))
            {
                SDKLogger.Log(TAG, "Animation Rigging package is installed.");
                return;
            }

            SDKLogger.Log(TAG, "Animation Rigging package is not installed.");

            if (EditorUtility.DisplayDialog(DIALOG_TITLE, DIALOG_MESSAGE, DIALOG_OK, DIALOG_CANCEL))
            {
                PackageManagerHelper.AddPackage(ANIMATION_RIGGING_PACKAGE_NAME);
            }
        }
    }
}
