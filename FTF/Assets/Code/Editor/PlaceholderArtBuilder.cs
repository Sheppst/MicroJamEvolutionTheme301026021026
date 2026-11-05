using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Tilemaps;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PlaceholderArtBuilder
{
    const string Root = "Assets/Art/Tileset/Placeholder";
    const int CellSize = 64;
    const float FrameRate = 6;

    static readonly string[] TileSheets = { "ground", "water", "fire", "cloud", "rain", "thunder", "tree_shrub", "tree_young", "tree_medium", "tree_large" };
    static readonly string[] Entities = { "leaf", "fruit", "animal", "sun" };

    [MenuItem("FTF/Art/Build Placeholders")]
    static void Build()
    {
        AssetDatabase.Refresh();
        Directory.CreateDirectory(Root + "/Tiles");
        Directory.CreateDirectory(Root + "/Palette");
        AssetDatabase.Refresh();

        string palettePath = Root + "/Palette/Placeholder.prefab";
        AssetDatabase.DeleteAsset(palettePath);
        GridPaletteUtility.CreateNewPalette(Root + "/Palette", "Placeholder", GridLayout.CellLayout.Rectangle,
            GridPalette.CellSizing.Automatic, Vector3.one, GridLayout.CellSwizzle.XYZ);
        GameObject palette = PrefabUtility.LoadPrefabContents(palettePath);
        Tilemap tilemap = palette.GetComponentInChildren<Tilemap>();

        int offsetX = 0;
        foreach (string sheet in TileSheets)
        {
            string path = $"{Root}/Sprites/{sheet}.png";
            int width = Slice(path, true);
            foreach (Sprite sprite in AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>())
            {
                string tilePath = $"{Root}/Tiles/{sprite.name}.asset";
                Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
                if (!tile)
                {
                    tile = ScriptableObject.CreateInstance<Tile>();
                    AssetDatabase.CreateAsset(tile, tilePath);
                }
                tile.sprite = sprite;
                EditorUtility.SetDirty(tile);
                tilemap.SetTile(new Vector3Int(offsetX + (int)sprite.rect.x / CellSize, (int)sprite.rect.y / CellSize, 0), tile);
            }
            offsetX += width / CellSize + 1;
        }

        PrefabUtility.SaveAsPrefabAsset(palette, palettePath);
        PrefabUtility.UnloadPrefabContents(palette);

        foreach (string entity in Entities)
            BuildEntity(entity);

        AssetDatabase.SaveAssets();
    }

    // Sets pixel art import settings and slices the texture in 64 px cells, skipping empty ones. Returns the texture width.
    static int Slice(string path, bool skipEmpty)
    {
        Texture2D source = new(2, 2);
        source.LoadImage(File.ReadAllBytes(path));

        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritePixelsPerUnit = CellSize;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;

        SpriteDataProviderFactories factories = new();
        factories.Init();
        ISpriteEditorDataProvider provider = factories.GetSpriteEditorDataProviderFromObject(importer);
        provider.InitSpriteEditorDataProvider();
        var existingIds = provider.GetSpriteRects().ToDictionary(r => r.name, r => r.spriteID);

        string name = Path.GetFileNameWithoutExtension(path);
        var rects = new System.Collections.Generic.List<SpriteRect>();
        for (int x = 0; x < source.width / CellSize; x++)
            for (int y = 0; y < source.height / CellSize; y++)
            {
                if (skipEmpty && source.GetPixels(x * CellSize, y * CellSize, CellSize, CellSize).All(p => p.a == 0))
                    continue;
                string spriteName = skipEmpty ? $"{name}_{x}_{y}" : $"{name}_{x}";
                rects.Add(new SpriteRect
                {
                    name = spriteName,
                    rect = new Rect(x * CellSize, y * CellSize, CellSize, CellSize),
                    alignment = SpriteAlignment.Center,
                    pivot = new Vector2(0.5f, 0.5f),
                    spriteID = existingIds.TryGetValue(spriteName, out GUID id) ? id : GUID.Generate()
                });
            }

        provider.SetSpriteRects(rects.ToArray());
        provider.GetDataProvider<ISpriteNameFileIdDataProvider>()
            .SetNameFileIdPairs(rects.Select(r => new SpriteNameFileIdPair(r.name, r.spriteID)));
        provider.Apply();
        importer.SaveAndReimport();

        int width = source.width;
        Object.DestroyImmediate(source);
        return width;
    }

    // Creates a looping sprite animation, its controller and a prefab for an entity strip.
    static void BuildEntity(string entity)
    {
        string basePath = $"{Root}/Entities/{entity}";
        Slice(basePath + ".png", false);
        Sprite[] frames = AssetDatabase.LoadAllAssetsAtPath(basePath + ".png").OfType<Sprite>().OrderBy(s => s.rect.x).ToArray();

        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(basePath + ".anim");
        if (!clip)
        {
            clip = new AnimationClip();
            AssetDatabase.CreateAsset(clip, basePath + ".anim");
        }
        clip.frameRate = FrameRate;
        AnimationUtility.SetObjectReferenceCurve(clip, EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite"),
            frames.Select((s, i) => new ObjectReferenceKeyframe { time = i / FrameRate, value = s })
                .Append(new ObjectReferenceKeyframe { time = frames.Length / FrameRate, value = frames[0] }).ToArray());
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(basePath + ".controller");
        if (!controller)
            controller = AnimatorController.CreateAnimatorControllerAtPathWithClip(basePath + ".controller", clip);

        GameObject go = new(entity);
        go.AddComponent<SpriteRenderer>().sprite = frames[0];
        go.AddComponent<Animator>().runtimeAnimatorController = controller;
        PrefabUtility.SaveAsPrefabAsset(go, basePath + ".prefab");
        Object.DestroyImmediate(go);
    }
}
