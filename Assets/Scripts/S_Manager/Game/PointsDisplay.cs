using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text pointsRedTeam;
    [SerializeField] TMP_Text pointsBlueTeam;
    void Update()
    {
        DisplayPoints();
    }

    void DisplayPoints()
    {
        foreach (var team in GameManager.Instance.points)
        {
            if (team.Key == Team.blueSide)
            {
                pointsBlueTeam.text = team.Value.ToString();
            }
            else if (team.Key == Team.redSide)
            {
                pointsRedTeam.text = team.Value.ToString();
            }
        }
    }
}
