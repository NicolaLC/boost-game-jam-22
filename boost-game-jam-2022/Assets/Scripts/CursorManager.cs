using UnityEngine;

public class CursorManager : SingletonPattern<CursorManager>
{
    private enum CursorType
    {
        Default,
        Interact,
        Denied
    }
    
    [SerializeField] 
    private Texture2D m_DefaultCursor = null;
    [SerializeField] 
    private Texture2D m_InteractCursor = null;
    [SerializeField] 
    private Texture2D m_DeniedCursor = null;

    private bool m_bLocked = false;

    public static void SetDefault()
    {
        Instance.Internal_SetCursor(CursorType.Default);
    }
    
    public static void SetInteract()
    {
        Instance.Internal_SetCursor(CursorType.Interact);
    }
    
    public static void SetDenied()
    {
        Instance.Internal_SetCursor(CursorType.Denied);
    }

    protected override void Awake()
    {
        base.Awake();
        
        SetDefault();
    }

    private void Internal_SetCursor(CursorType i_Type)
    {
        if (m_bLocked)
        {
            return;
        }
        
        Texture2D targetCursor = i_Type switch
        {
            CursorType.Default => m_DefaultCursor,
            CursorType.Denied => m_DeniedCursor,
            CursorType.Interact => m_InteractCursor,
            _ => m_DefaultCursor
        };

        Cursor.SetCursor(targetCursor, new Vector2(10, 5), CursorMode.ForceSoftware);
    }

    public static void Lock()
    {
        Instance.m_bLocked = true;
    }
    
    public static void Unlock()
    {
        Instance.m_bLocked = false;
    }
}
