using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthPanel : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private GameObject _panelRoot;
    
    private Entity _boundEntity;

    private void OnDestroy()
    {
        if (_boundEntity != null)
            _boundEntity.OnHealthChanged -= HandleHealthChanged;
    }

    public void Bind(Entity entity)
    {
        if (_boundEntity != null)
            _boundEntity.OnHealthChanged -= HandleHealthChanged;

        _boundEntity = entity;
    
        if (_boundEntity != null)
        {
            _panelRoot.SetActive(true);
            _boundEntity.OnHealthChanged += HandleHealthChanged;
            HandleHealthChanged(_boundEntity.Health);
        }
        else
        {
            _panelRoot.SetActive(false);
        }
    }

    private void HandleHealthChanged(int health)
    {
        _healthText.SetText(health.ToString());
        _healthBar.fillAmount = health / 100f;
    }
}