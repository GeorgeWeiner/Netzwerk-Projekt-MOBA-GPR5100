using System.Collections;
using System.Collections.Generic;
using Mirror;
using S_Player;
using Steamworks;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChampion",menuName = "NewChampion")]
public class ChampionData : ScriptableObject
{
    [Header("ChampionDisplay")]
    [SerializeField] string name;
    [SerializeField] GameObject heroPrefab;
    [SerializeField] GameObject heroVisuals;
    [SerializeField] Sprite championSprite;
    public Sprite ChampionSprite{ get => championSprite; }
    [SerializeField] int championId;
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
