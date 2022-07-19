using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// a handcrafted button which takes in the champion you select by clicking on it 
/// </summary>
public class ChampSelectButton : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] ChampSelect champSelect;
    [SerializeField] ChampionData championData;

    void Awake()
    {
        GetComponent<Image>().sprite = championData.ChampionSprite;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        champSelect.ChangeChampionPrefabOfPlayer(championData.ChampionId);
    }
}
