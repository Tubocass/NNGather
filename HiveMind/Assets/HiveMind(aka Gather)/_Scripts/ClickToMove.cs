using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] LayerMask mask;
    MeshRenderer renderer;
    bool hide = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                agent.SetDestination(hit.point);
                Debug.Log(hit.point.ToString());
            }
        }

        if (agent.isOnOffMeshLink)
        {
            if (!hide)
            {
                renderer.enabled = false;
                hide = true;
            }
        }
        else
        {
            if(hide)
            {
                renderer.enabled = true;
                hide = false;
            }
        }
    }
}
