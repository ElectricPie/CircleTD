using System;
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

        private Animator m_animator;
        
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        protected void Update()
        {
            transform.rotation = Camera.transform.rotation;
        }

        public void SetValue(float value, Vector3 position, float playbackSpeed = 1)
        {
            m_valueText.text = $"{value:0.0}";
            transform.position = position;
            
            m_animator.Play("FloatingNumber");
            m_animator.speed = playbackSpeed;
        }
    }
}