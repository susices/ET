using System;
using System.Collections;
using System.Collections.Generic;
using BM;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace ET
{
    public static class IconHelper
    {
        /// <summary>
        /// 同步加载图集图片资源
        /// </summary>
        /// <OtherParam name="spriteName"></OtherParam>
        /// <returns></returns>
        public static Sprite LoadIconSprite(string atlasPath,  string spriteName)
        {
            try
            {
                SpriteAtlas spriteAtlas = AssetComponent.Load<SpriteAtlas>(atlasPath);
                Sprite sprite = spriteAtlas.GetSprite(spriteName);
                if ( null == sprite )
                {
                    Log.Error($"sprite is null: {spriteName}");
                }
                return sprite;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        /// <summary>
        /// 异步加载图集图片资源
        /// </summary>
        /// <OtherParam name="spriteName"></OtherParam>
        /// <returns></returns>
        public static async ETTask<Sprite> LoadIconSpriteAsync(string atlasPath,  string spriteName)
        {
            try
            {
                SpriteAtlas spriteAtlas = await AssetComponent.LoadAsync<SpriteAtlas>(atlasPath);
                Sprite sprite = spriteAtlas.GetSprite(spriteName);
                if (null == sprite)
                {
                    Log.Error($"sprite is null: {spriteName}");
                }
                return sprite;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}

