using TMPro;
using UnityEngine;

public class UIErrorMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text m_errorText;

    public static UIErrorMessage Instance { get; private set; }

    public void ShowError(string message, float displayTime = 2.0f)
    {
        if (m_errorText is null)
        {
            Debug.LogError($"UIErrorMessage on {name} is missing reference to Error Text TMP Text", this);
            return;
        }

        gameObject.SetActive(true);
        m_errorText.text = message;
        Invoke(nameof(HideMessage), displayTime);
    }
    
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Multiple UIErrorMessages found destroying duplicate");
            Destroy(this);
        }
    }

    private void HideMessage()
    {
        gameObject.SetActive(false);
    }
}
