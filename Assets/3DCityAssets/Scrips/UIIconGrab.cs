using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIIconGrab : MonoBehaviour
{

    public UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor rayInteractor;

    [Tooltip("The 3D prefab to instantiate when dropped into a socket.")]
    public GameObject prefabToSpawn;

    private RectTransform iconPosition;

    private void Awake()
    {
         rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor>();
    }

    private void OnEnable()
    {
        if (rayInteractor != null)
        {
            Debug.Log("Ray is not null" + rayInteractor.name);
            rayInteractor.selectEntered.AddListener(OnSelectEnter);
            rayInteractor.selectExited.AddListener(OnSelectExited);
        }
        else
        {
            Debug.Log("Ray is null");
        }
        
    }

    private void OnSelectEnter(SelectEnterEventArgs arg0)
    {
        iconPosition.position = transform.position;
        Debug.Log("Icon pos" + iconPosition.position + "this position" + transform.position);
        throw new NotImplementedException();
    }
    private void OnSelectExited(SelectExitEventArgs arg0)
    {
        
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        rayInteractor.selectEntered.RemoveListener(OnSelectEnter);
        rayInteractor.selectExited.RemoveListener(OnSelectExited);
    }


    public void SpawnPrefab()
    {
        Vector3 spawnPosition = transform.position;

        // Raycast down
        if (Physics.Raycast(spawnPosition + Vector3.up, Vector3.down, out RaycastHit hit, 10f))
        {
            if (hit.collider.gameObject.tag == "PlaceableSurface")
            {
                Debug.Log("PlaceableSurface");
            }
            spawnPosition = hit.point;
        }
        Debug.Log("spawn Instance " + transform.position);
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

    }

}

