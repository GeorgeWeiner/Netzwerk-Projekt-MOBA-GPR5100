using UnityEngine;
using UnityEngine.UI;

namespace S_Combat
{
    //This used to be called health display. I renamed it to reuse with mana etc...
    public class StatBarDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject statBarParent;
        [SerializeField] private Image statBarImage;

        private void OnMouseEnter()
        {
            statBarParent.SetActive(true);
        }

        private void OnMouseExit()
        {
            statBarParent.SetActive(false);
        }

        private void HandleStatUpdated(int currentAmount, int maxAmount)
        {
            statBarImage.fillAmount = (float)currentAmount / maxAmount;
        }
    }
}