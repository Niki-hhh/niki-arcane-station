using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;

namespace Content.Client._Arcane.JoinQueue;

internal static class QueueMiniGameAssets
{
    private const string RegularFontPath = "/Fonts/NotoSans/NotoSans-Regular.ttf";
    private const string BoldFontPath = "/Fonts/NotoSans/NotoSans-Bold.ttf";
    private const string SpacepodTexturePath = "/Textures/_Arcane/Interface/MiniGames/spacepod.rsi/spacepod.png";

    public static Font LoadRegularFont(IResourceCache cache, int size)
    {
        return new VectorFont(cache.GetResource<FontResource>(RegularFontPath), size);
    }

    public static Font LoadBoldFont(IResourceCache cache, int size)
    {
        return new VectorFont(cache.GetResource<FontResource>(BoldFontPath), size);
    }

    public static Texture LoadSpacepodTexture(IResourceCache cache)
    {
        return cache.GetResource<TextureResource>(SpacepodTexturePath).Texture;
    }
}
