using UnityEngine;

[CreateAssetMenu(fileName = "NewChampion",menuName = "NewChampion")]
public class ChampionData : ScriptableObject
{
    [Header("ChampionDisplay")]
    [SerializeField]
    private string championName;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private GameObject heroVisuals;
    [SerializeField] private Sprite championSprite;
    public Sprite ChampionSprite{ get => championSprite; }
    [SerializeField] private int championId;
    public int ChampionId{ get => championId; }

    public GameObject GetVisualDisplay()
    {
        return heroVisuals;
    }

    public GameObject GetCurrentChampion()
    {
        return heroPrefab;
    }
}
