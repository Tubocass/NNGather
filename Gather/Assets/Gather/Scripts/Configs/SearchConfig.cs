using UnityEngine;

[CreateAssetMenu]
public class SearchConfig : ScriptableObject
{
    public int searchDist;
    public int searchAmount;
    public string searchTag;
    public LayerMask searchLayer;
}
