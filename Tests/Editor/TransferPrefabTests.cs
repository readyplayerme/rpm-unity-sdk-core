using NUnit.Framework;
using ReadyPlayerMe.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Core.Tests
{
    public class TransferPrefabTests : MonoBehaviour
    {
        private const string PREFAB_ASSET_NAME = "t:prefab glTFast-TimeBudgetPerFramDeferAgent";
        private const string INCORRECT_ASSET_NAME = "t:prefab asdaasdas";
        private const string NEW_PREFAB_PATH = "Assets/Tests/TestPrefab.prefab";

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (AssetDatabase.LoadAssetAtPath(NEW_PREFAB_PATH, typeof(GameObject)))
            {
                AssetDatabase.DeleteAsset(NEW_PREFAB_PATH);
            }
        }

        [Test]
        public void Transfer_Prefab_By_Guid()
        {
            var guids = AssetDatabase.FindAssets(PREFAB_ASSET_NAME);
            if (guids.Length == 0)
            {
                Assert.Fail();
            }
            PrefabTransferHelper.TransferPrefabByGuid(guids[0], NEW_PREFAB_PATH);
            if (AssetDatabase.LoadAssetAtPath(NEW_PREFAB_PATH, typeof(GameObject)) != null)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Fail_Transfer_Prefab()
        {
            PrefabTransferHelper.TransferPrefabByGuid(INCORRECT_ASSET_NAME, NEW_PREFAB_PATH);
            if (AssetDatabase.LoadAssetAtPath(NEW_PREFAB_PATH, typeof(GameObject)) == null)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}
