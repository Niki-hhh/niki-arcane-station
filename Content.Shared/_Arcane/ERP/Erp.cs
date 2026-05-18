namespace Content.Shared._Arcane.ERP;

public enum ErpPreference : byte
{
    Yes,
    Ask,
    No,
}

public sealed class ErpPreferenceChangedEvent : EntityEventArgs
{
    public ErpPreference OldPreference;
    public ErpPreference NewPreference;

    public ErpPreferenceChangedEvent(ErpPreference oldPref, ErpPreference newPref)
    {
        OldPreference = oldPref;
        NewPreference = newPref;
    }
}
