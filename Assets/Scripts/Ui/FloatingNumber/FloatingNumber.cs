using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Ui.FloatingNumber
{
    public class FloatingNumber : MonoBehaviour
    {
        public Transform Camera = null;
        
        [SerializeField, Required] private TMP_Text m_valueText;

        protected void Update()
        {
            transform.rotation = Camera.transform.rotation;
        }

        public void SetValue(float value)
        {
            m_valueText.text = $"{value:0.0}";
        }

        public void SetTarget(Vector3 position)
        {
            transform.position = position;
        }
    }
}