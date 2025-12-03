using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Takibi/MaterialDatabase")]
public class MaterialDatabase : ScriptableObject
{
    public List<MaterialData> materials;
}
