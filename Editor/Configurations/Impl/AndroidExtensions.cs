public static class AndroidExtensions
{
    public static bool IsAab(this AndroidAppBundle appBundle)
    {
        switch (appBundle)
        {
            case AndroidAppBundle.Aab:
            case AndroidAppBundle.AabWithObb:
                return true;

            default:
                return false;
        }
    }
}
