using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChampSelectButton : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] ChampSelect champSelect;
    [SerializeField] Sprite ChampionSprite;
    [SerializeField] int championId;
    

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().color = Random.ColorHSV();
        champSelect.ChangeChampionPrefabOfPlayer(championId);
    }
}
