using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Scannable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the instanced versions of all materials, and replace the originals with them. This way we can change the properties of the materials.
        // without Unity modifying the originals.
        var renderer = GetComponent<Renderer>();
        var materialInstances = new List<Material>();

        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        {
            if (MaterialInstanceFactory.Instance.TryGetMaterialInstance(renderer.sharedMaterials[i], out Material materialInstance))
            {
                materialInstances.Add(materialInstance);
            }
            else
            {
                materialInstances.Add(null);
            }
        }

        var materials = renderer.materials;
        for (int i = 0; i < materialInstances.Count; i++)
        {
            if (materialInstances[i] != null)
            {
                materials[i] = materialInstances[i];
            }
        }
        renderer.materials = materials;
    }
}
