using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [SerializeField] private Button[] m_tabButtons;
    [SerializeField] private GameObject[] m_tabs;
    
    public void ChangeTab(int tabNumber)
    {
        tabNumber = Mathf.Clamp(tabNumber, 0, m_tabs.Length);
        
        // Disable other tabs and enable requested tab
        for (int i = 0; i < m_tabs.Length; i++)
        {
            if (m_tabs[i] is not null)
            {
                m_tabs[i].SetActive(i == tabNumber);
            }
        }
        
        // Disable requested tab button and enable other tab buttons
        for (int i = 0; i < m_tabButtons.Length; i++)
        {
            if (m_tabButtons[i] is not null)
            {
                m_tabButtons[i].interactable = i != tabNumber;
            }
        }
    }
}
