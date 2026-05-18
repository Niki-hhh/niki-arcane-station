using Content.Server.Preferences.Managers;
using Content.Shared._Arcane.ERP;
using Content.Shared.Humanoid;
using Content.Shared.Preferences;
using Robust.Shared.Player;

namespace Content.Server._Arcane.ERP;

public sealed class ErpStatusSystem : EntitySystem
{
    [Dependency] private readonly IServerPreferencesManager _prefs = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HumanoidAppearanceComponent, PlayerAttachedEvent>(OnPlayerAttached);
    }

    private void OnPlayerAttached(EntityUid uid, HumanoidAppearanceComponent humanoid, ref PlayerAttachedEvent args)
    {
        var profile = _prefs.GetPreferences(args.Player.UserId).SelectedCharacter as HumanoidCharacterProfile;
        var preference = profile?.ErpPreference ?? ErpPreference.Ask;

        EnsureComp<ArousalComponent>(uid);

        var comp = EnsureComp<ErpStatusComponent>(uid);
        var oldPreference = comp.Preference;
        comp.Preference = preference;
        Dirty(uid, comp);

        if (oldPreference != preference)
            RaiseLocalEvent(uid, new ErpPreferenceChangedEvent(oldPreference, preference));
    }
}
