using UnityEngine;

public class ShadowEffect : MonoBehaviour
{
    [Header("Shadow Settings")]
    public Color shadowColor = new Color(0, 0, 0, 0.5f); // Semi-transparent black

    public void ApplyShadowToChildren(GameObject prefab)
    {
        // Get all Renderer components in the prefab, including its children
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Modify the material color to the shadow color
            foreach (Material material in renderer.materials)
            {
                material.color = shadowColor;
            }
        }
    }

    public void ResetToOriginalColors(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.color = Color.white; // Reset to white or any default color
            }
        }
    }
}
