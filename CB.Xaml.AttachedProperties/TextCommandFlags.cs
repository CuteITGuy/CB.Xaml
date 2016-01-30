namespace CB.Xaml.AttachedProperties
{
    public enum TextCommandFlags
    {
        None = 0,
        TrimAll = 1,
        TrimEnd = 2,
        TrimStart = 4,
        All = TrimAll | TrimEnd | TrimStart
    }
}