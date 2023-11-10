using System;
using UnityEngine;

public class ClickSelectController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public static event Action<Entity> OnSelectedEntityChanged;
    public static Entity SelectedEntity { get; private set; }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                var entity = hitInfo.collider.GetComponent<Entity>();
                SelectedEntity = entity;
                OnSelectedEntityChanged?.Invoke(entity);
            }
        }
      
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectedEntity = null;
            OnSelectedEntityChanged?.Invoke(null);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && SelectedEntity != null)
        {
            SelectedEntity.TakeDamage(10);
        }
    }
}