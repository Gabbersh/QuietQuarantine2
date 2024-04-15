using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFocusHighlight : MonoBehaviour
{
    //Add all renderers from an object(from childobjects aswell) in the inspector
    [SerializeField] private List<Renderer> renderers;
    private Color color = new Color (255, 255, 255, 145);

    private List<Material> materials = new List<Material>();

    private void Awake()
    {
        foreach(var renderer in renderers)
        {
            DynamicGI.SetEmissive(renderer, color);
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val)
    {
        if(val)
        {
            foreach(var material in materials)
            {
                material.EnableKeyword("_EMISSION");
               //why no work? material.SetColor("_EmissionColor", color * -1f);
            }
        }
        else
        {
            foreach(var material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
