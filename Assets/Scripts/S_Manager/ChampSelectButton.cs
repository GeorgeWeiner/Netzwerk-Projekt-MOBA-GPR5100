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
    [SerializeField] GameObject prefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("HEHEHE");
        GetComponent<Image>().color = Random.ColorHSV();
        champSelect.ChangeChampionPrefabOfPlayer(Instantiate(prefab));
    }
}
