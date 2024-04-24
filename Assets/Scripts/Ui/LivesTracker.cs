using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text m_livesText;

    public void OnLivesChanged(int currentLives, int maxLives)
    {
        if (m_livesText is null)
        {
            Debug.LogError($"LivesTracker on {name} is missing reference to LivesText", this);
            return;
        }

        m_livesText.text = $"{currentLives:00}/{maxLives:00}";
    }
}
