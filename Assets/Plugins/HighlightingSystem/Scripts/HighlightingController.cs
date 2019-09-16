using System.Collections;
using UnityEngine;

public class HighlightingController : MonoBehaviour
{
    protected HighlightableObject ho;
    public Color clickColor = Color.red;

    void Awake()
    {
        ho = gameObject.GetOrAddComponent<HighlightableObject>();
    }

    void OnMouseEnter()
    {
        ho.ConstantParams(clickColor);
        ho.ConstantSwitchImmediate();
    }

    void OnMouseExit()
    {
        ho.Off();
    }

    protected virtual void AfterUpdate() { }
}