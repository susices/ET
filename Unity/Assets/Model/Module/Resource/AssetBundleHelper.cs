using UnityEngine;

namespace ET
{
    // 用于字符串转换，减少GC
    public static class AssetBundleHelper
    {
        public static async ETTask<AssetBundle> UnityLoadBundleAsync(string path)
        {
            var tcs = ETTask<AssetBundle>.Create(true);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            request.completed += operation => { tcs.SetResult(request.assetBundle); };
            return await tcs;
        }

        public static async ETTask<UnityEngine.Object[]> UnityLoadAssetAsync(AssetBundle assetBundle)
        {
            var tcs = ETTask<UnityEngine.Object[]>.Create(true);
            AssetBundleRequest request = assetBundle.LoadAllAssetsAsync();
            request.completed += operation => { tcs.SetResult(request.allAssets); };
            return await tcs;
        }

        public static string IntToString(this int value)
        {
            string result;
            if (ResourcesComponent.Instance.IntToStringDict.TryGetValue(value, out result))
            {
                return result;
            }

            result = value.ToString();
            ResourcesComponent.Instance.IntToStringDict[value] = result;
            return result;
        }

        public static string StringToAB(this string value)
        {
            string result;
            if (ResourcesComponent.Instance.StringToABDict.TryGetValue(value, out result))
            {
                return result;
            }

            result = value + ".unity3d";
            ResourcesComponent.Instance.StringToABDict[value] = result;
            return result;
        }

        public static string IntToAB(this int value)
        {
            return value.IntToString().StringToAB();
        }

        public static string BundleNameToLower(this string value)
        {
            string result;
            if (ResourcesComponent.Instance.BundleNameToLowerDict.TryGetValue(value, out result))
            {
                return result;
            }

            result = value.ToLower();
            ResourcesComponent.Instance.BundleNameToLowerDict[value] = result;
            return result;
        }
    }
}