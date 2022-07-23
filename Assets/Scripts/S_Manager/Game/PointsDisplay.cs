using TMPro;
using UnityEngine;

/// <summary>
/// The class which is responsible for displaying the points of each team
/// </summary>
public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsRedTeam;
    [SerializeField] private TMP_Text pointsBlueTeam;

    private void Update()
    {
        DisplayPoints();
    }
    /// <summary>
    /// this functions loop over both teams and sets their points visually
    /// </summary>
    private void DisplayPoints()
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
