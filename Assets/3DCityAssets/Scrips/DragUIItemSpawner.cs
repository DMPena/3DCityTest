using System.Collections;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DragUIItemSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField]
    public GameObject prefabToSpawn;
    [SerializeField]
    private GameObject spawnedObjectParent;

    [SerializeField]
    private LayerMask placeableLayerMask;

    [SerializeField]
    private RaycastHit iconHitInfo;

    private ObjectPlacementHelper objectPlacementHelper;

    private Vector3 objectPlacementHelperOffset = new Vector3(0, 1, 0);
    private RectTransform iconDefaultLocalLocation;
    private GameObject iconParent;
    private bool isGrabed = false;
    private bool allowedToSpawn = false;

    void Awake()
    {
        objectPlacementHelper = FindAnyObjectByType<ObjectPlacementHelper>();
        iconDefaultLocalLocation = GetComponent<RectTransform>();
        iconParent = transform.parent != null ? transform.parent.gameObject : null;
    }

    void Update()
    {
        if (GetIsGrabed() && IsValidPlaceableLocation(out iconHitInfo))
        {
            allowedToSpawn = true;
        }
        else
        {
            allowedToSpawn = false;
        }
    }

    public void SetIsGrabed(bool value)
    {
        isGrabed = value;
    }

    public bool GetIsGrabed()
    {
        return isGrabed;
    }

    public void ResetIcon()
    {
        if (objectPlacementHelper != null)
        {
            objectPlacementHelper.HideAllReticles();
        }

        if (iconParent == null || !iconParent.activeInHierarchy)
        {
            return;
        }

        // Re-parent the GameObject to the original parent in the UI menu
        gameObject.transform.SetParent(iconParent.transform, false);

        // Reset position, rotation, and scale to match the original UI state
        gameObject.transform.localPosition = iconDefaultLocalLocation.localPosition;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
    }

    // Cast a ray from the GameObject's position downwards to check for a "Placeable Surface" layer
    public bool IsValidPlaceableLocation(out RaycastHit hitInfo)
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 100f;

        if (Physics.Raycast(origin, direction, out hitInfo, maxDistance))
        {
            Debug.Log("Placement layer is:" + hitInfo.transform.gameObject.layer);
            // Check if the hit object is on the "InvalidPlace" layer
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("InvalidPlace"))
            {
                objectPlacementHelper.IsValidPlacement = false;
                objectPlacementHelper.ShowInvalidReticle(hitInfo.point);
                Debug.Log("Placement is invalid.");
                return false;
            }

            objectPlacementHelper.IsValidPlacement = true;
            objectPlacementHelper.ShowValidReticle(hitInfo.point);
            objectPlacementHelper.transform.position = hitInfo.point;

            // Set the rotation so the up vector matches the hit normal, but preserve this transform's Y rotation
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            float yRotation = transform.eulerAngles.y;
            objectPlacementHelper.transform.rotation = Quaternion.Euler(0, yRotation, 0) * surfaceRotation;

            // Debug.Log("Valid placeable location found at: " + hitInfo.point);
            return true;
        }

        objectPlacementHelper.IsValidPlacement = false;
        objectPlacementHelper.HideAllReticles();
        Debug.Log("No hit found");
        return false;
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn != null && spawnedObjectParent != null)
        {
            if (iconHitInfo.transform != null)
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, iconHitInfo.point, objectPlacementHelper.transform.rotation);
                spawnedObject.transform.SetParent(spawnedObjectParent.transform, true);

                // Trigger the ScaleAndPopEffect
                ScaleAndPopEffect scaleAndPopEffect = spawnedObject.GetComponent<ScaleAndPopEffect>();
                if (scaleAndPopEffect != null)
                {
                    scaleAndPopEffect.StartScaleAndPopEffect();
                }
                else
                {
                    Debug.LogWarning("ScaleAndPopEffect script is not attached to the prefab.");
                }
            }
            else
            {
                Debug.LogWarning("iconHitInfo is not valid. Ensure a valid placeable location is detected.");
            }
        }
        else
        {
            Debug.LogWarning("Assign prefab to spawn.");
        }
    }

    public void TryToSpawnPrefab()
    {
        if (allowedToSpawn)
        {
            SpawnPrefab();
        }
        else
        {
            Debug.Log("Not allowed to spawn. Ensure a valid placeable location is detected." + isGrabed + iconHitInfo.point);
        }
    }

}

