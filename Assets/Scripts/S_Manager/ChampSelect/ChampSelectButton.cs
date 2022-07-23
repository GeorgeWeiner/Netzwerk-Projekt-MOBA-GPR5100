using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// a handcrafted button which takes in the champion you select by clicking on it 
/// </summary>
public class ChampSelectButton : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private ChampSelect champSelect;
    [SerializeField] private ChampionData championData;

    private void Awake()
    {
        GetComponent<Image>().sprite = championData.ChampionSprite;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        champSelect.ChangeChampionPrefabOfPlayer(championData.ChampionId);
    }
}
