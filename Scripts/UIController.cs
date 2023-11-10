using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private StatsList _statsList;
    [SerializeField] private HealthPanel _healthPanel;

    private void Awake()
    {
        ClickSelectController.OnSelectedEntityChanged += HandleSelectedEntityChanged;
        
        HandleSelectedEntityChanged(ClickSelectController.SelectedEntity);
    }

    private void OnDestroy()
    {
        ClickSelectController.OnSelectedEntityChanged -= HandleSelectedEntityChanged;
    }

    private void HandleSelectedEntityChanged(Entity entity)
    {
        if (_statsList != null)
            _statsList.Bind(entity?.Stats);

        if (_healthPanel != null)
            _healthPanel.Bind(entity);
    }

    private void OnValidate()
    {
        if (_statsList == null)
            _statsList = GetComponentInChildren<StatsList>();
    }
}