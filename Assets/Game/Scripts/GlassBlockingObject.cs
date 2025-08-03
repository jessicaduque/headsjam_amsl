using Unity;
using UnityEngine;

public class GlassBlockingObject : GlassObject
{
    public static event System.Action OnBreak;
    
    public override void BreakGlassObject()
    {
        base.BreakGlassObject();
        OnBreak?.Invoke();
    }
}