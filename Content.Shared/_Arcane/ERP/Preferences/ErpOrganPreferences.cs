using Content.Shared.Humanoid;
using Robust.Shared.Maths;
using Robust.Shared.Serialization;

namespace Content.Shared._Arcane.ERP.Preferences;

/// <summary>
/// Per-character organ appearance configuration. Stored separately from HumanoidCharacterProfile.
/// </summary>
[Serializable, NetSerializable]
public sealed class ErpOrganPreferences
{
    /// <summary>Organ configs keyed by organ slot id ("penis", "vagina", "breasts", "testicles", "anus").</summary>
    public Dictionary<string, ErpOrganConfig> Organs { get; set; } = new();

    public ErpOrganConfig GetOrgan(string slotId)
        => Organs.TryGetValue(slotId, out var cfg) ? cfg : new ErpOrganConfig();

    public void SetOrgan(string slotId, ErpOrganConfig cfg)
        => Organs[slotId] = cfg;

    public static ErpOrganPreferences Default() => new();
}

[Serializable, NetSerializable]
public sealed class ErpOrganConfig
{
    /// <summary>Visual variant from RSI, e.g. "human", "knotted", "equine".</summary>
    public string Variant { get; set; } = "human";

    /// <summary>Size index 1–8.</summary>
    public int Size { get; set; } = 3;

    /// <summary>Tint color. Null = use character skin color.</summary>
    public Color? Color { get; set; } = null;
}

/// <summary>Organ slot ids used as keys in ErpOrganPreferences.</summary>
public static class ErpOrganSlots
{
    public const string Penis = "penis";
    public const string Vagina = "vagina";
    public const string Breasts = "breasts";
    public const string Testicles = "testicles";
    public const string Anus = "anus";
    public const string Butt = "butt";

    public static readonly IReadOnlyList<string> All = [Vagina, Breasts, Testicles, Penis, Butt, Anus];

    public static readonly IReadOnlyList<string> EditorVisible = [Vagina, Breasts, Penis];

    /// <summary>Slots not listed here are visible for all sexes.</summary>
    public static readonly IReadOnlyDictionary<string, Sex[]> SexFilter =
        new Dictionary<string, Sex[]>
        {
            [Penis]     = [Sex.Male, Sex.Futanari],
            [Testicles] = [Sex.Male, Sex.Futanari],
            [Vagina]    = [Sex.Female, Sex.Futanari],
            [Breasts]   = [Sex.Female, Sex.Futanari],
        };

    /// <summary>Available visual variants per slot. Slots not listed have no variant selector.</summary>
    public static readonly IReadOnlyDictionary<string, string[]> Variants =
        new Dictionary<string, string[]>
        {
            [Penis]     = ["human", "knotted", "barbknot", "flared", "tentacle", "hemi", "hemiknot", "tapered", "thick"],
            [Vagina]    = ["human", "gaping", "hairy", "spade"],
            [Testicles] = ["single", "sheath", "hidden"],
            [Anus]      = ["donut", "squished"],
        };

    /// <summary>
    /// Optional species whitelist per slot variant. Variants not listed here are available to every species.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string[]>> VariantSpeciesFilter =
        new Dictionary<string, IReadOnlyDictionary<string, string[]>>
        {
            [Penis] = new Dictionary<string, string[]>
            {
                ["knotted"]  = ["Felinid", "Tajaran", "Vulpkanin", "Yowie", "Kobold", "Rodentia"],
                ["barbknot"] = ["Felinid", "Tajaran"],
                ["flared"]   = ["Reptilian", "Vox", "Harpy", "Resomi", "Kobold", "Arachnid"],
                ["tentacle"] = ["Diona", "SlimePerson", "Demon", "HumanoidXeno"],
                ["hemi"]     = ["Reptilian", "Kobold", "Vox", "HumanoidXeno"],
                ["hemiknot"] = ["Reptilian", "Kobold", "Vox", "HumanoidXeno"],
                ["tapered"]  = ["Reptilian", "Vox", "Harpy", "Resomi", "Kobold", "Arachnid"],
            },
        };

    public static string[] GetVariantsForSpecies(string slotId, string species)
    {
        if (!Variants.TryGetValue(slotId, out var variants))
            return [];

        if (!VariantSpeciesFilter.TryGetValue(slotId, out var filters))
            return variants;

        var result = new List<string>();
        foreach (var variant in variants)
        {
            if (!filters.TryGetValue(variant, out var allowedSpecies) ||
                allowedSpecies.Length == 0 ||
                Array.IndexOf(allowedSpecies, species) >= 0)
            {
                result.Add(variant);
            }
        }

        return result.Count > 0 ? result.ToArray() : [variants[0]];
    }

    /// <summary>Maximum size index per slot (slider range 1..N). Slots not listed have no size control.</summary>
    public static readonly IReadOnlyDictionary<string, int> MaxSize =
        new Dictionary<string, int>
        {
            [Penis]     = 5,
            [Breasts]   = 4,
            [Testicles] = 5,
            [Butt]      = 5,
            [Anus]      = 8,
        };
}
