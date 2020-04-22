class B_1_FreeWord : RecentDropdown
{
    public override string PrefsKey
    {
        get
        {
            return "BMCategoryRecent";
        }
    }

    public override int Limit
    {
        get
        {
            return 20;
        }
    }   
}
