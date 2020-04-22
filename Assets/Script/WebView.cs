
class WebView : WebViewObject
{
    private static WebView m_instance;
    public static WebView Instance
    {
        get
        {
            return m_instance;
        }
        private set
        {
            m_instance = value;
        }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Instance = null;
    }
}
