using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Ui.FloatingNumber
{
    public class FloatingNumbersPool : MonoBehaviour
    {
        [SerializeField, RequiredIn(PrefabKind.InstanceInScene)]
        private TowerWaves m_towerWaves;
        [SerializeField, RequiredIn(PrefabKind.InstanceInScene)]
        private Transform m_cameraTransform;
        
        [SerializeField, Required] private FloatingNumber m_floatingNumberPrefab;
        [SerializeField, Min(1)] private int m_poolSize = 20;
        [SerializeField, Min(0)] private float m_showTime = 0.5f;
        [SerializeField] private Vector3 m_offsetFromUnit = new Vector3(0.0f, 2.0f, 0.0f);
        
        private ObjectPool<FloatingNumber> m_numberPool;
        
        protected void Awake()
        {
            m_numberPool = new ObjectPool<FloatingNumber>(() =>
                {
                    FloatingNumber newFloatingNumber = Instantiate(m_floatingNumberPrefab);
                    newFloatingNumber.Camera = m_cameraTransform;
                    return newFloatingNumber;
                },
                floatingNumber =>
                {
                    floatingNumber.gameObject.SetActive(true);
                    Debug.Log("Activated Number");
                },
                floatingNumber =>
                {
                    floatingNumber.gameObject.SetActive(false);
                    floatingNumber.transform.position = Vector3.zero;
                    Debug.Log("Deactivated Number");
                },
                floatingNumber =>
                {
                    Destroy(floatingNumber);
                }, true, m_poolSize);

            m_towerWaves.OnUnitSpanwedEvent.AddListener(newUnit =>
            {
                newUnit.HealthComponent.OnDamageTakenEvent += OnUnitTakeDamage;
            });
        }

        private void OnUnitTakeDamage(GameObject unit, float damage)
        {
            FloatingNumber floatingNumber = m_numberPool.Get();
            floatingNumber.SetValue(damage, unit.transform.position + m_offsetFromUnit, 1 / m_showTime);

            StartCoroutine(HideNumber(floatingNumber));
            Debug.Log("Set Number");
        }

        private IEnumerator HideNumber(FloatingNumber floatingNumber)
        {
            yield return new WaitForSeconds(m_showTime);
            
            m_numberPool.Release(floatingNumber);
        }
    }
}