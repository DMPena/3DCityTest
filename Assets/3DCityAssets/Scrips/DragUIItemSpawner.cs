using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DragUIItemSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject dragableUIPrefab;
    public Transform spawnLocation;

    private XRInteractionManager interactionManager;

    private void Awake()
    {
        interactionManager = FindAnyObjectByType<XRInteractionManager>();
        if (interactionManager == null)
        {
            Debug.LogError("XRInteractionManager not found in the scene.");
        }
    }

    public void SpawnAndAttach()
    {
        GameObject spawnedItem = Instantiate(dragableUIPrefab, spawnLocation.position, spawnLocation.rotation);

        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = spawnedItem.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("Spawned item does not have XRGrabInteractable.");
            return;
        }
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor closestInteractor = FindNearestInteractor(spawnLocation.position);

        if (closestInteractor == null)
        {
            Debug.LogWarning("No interactor found.");
            return;
        }

        StartCoroutine(AttachAfterFrame(grabInteractable, closestInteractor));
    }

    private IEnumerator AttachAfterFrame(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable, UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
    {
        yield return null;

        if (interactionManager != null &&
            interactor is UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor selectInteractor &&
            interactable is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable)
        {
            interactionManager.SelectEnter(selectInteractor, selectInteractable);
        }
    }

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor FindNearestInteractor(Vector3 fromPosition)
    {
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor[] interactors = FindObjectsByType<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor>(FindObjectsSortMode.None);
        float minDistance = float.MaxValue;
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor nearest = null;

        foreach (var interactor in interactors)
        {
            if (!interactor.isActiveAndEnabled)
                continue;

            float dist = Vector3.Distance(interactor.transform.position, fromPosition);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = interactor;
            }
        }

        return nearest;
    }
}
