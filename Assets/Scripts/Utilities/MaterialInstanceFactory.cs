using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Returns instances of a material based on the original material passed to the TryGetMaterialInstance method.
/// We do this so we can set properties on a material and having them take effect on every renderer that uses this material,
/// without Unity modifying the original material, resulting in source control issues.
/// </summary>
public class MaterialInstanceFactory : MonoBehaviour
{
    [SerializeField] private Material[] materialsToInstantiate;
    public static MaterialInstanceFactory Instance { get; private set; }

    private readonly Dictionary<Material, Material> materialInstances = new Dictionary<Material, Material>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach (var material in materialsToInstantiate)
        {
            var materialInstance = new Material(material);
            materialInstances.Add(material, materialInstance);
        }
    }

    public bool TryGetMaterialInstance(Material material, out Material materialInstance)
    {
        if (!materialInstances.ContainsKey(material))
        {
            materialInstance = null;
            return false;
        }
        else
        {
            materialInstance = materialInstances[material];
            return true;
        }
    }
}
