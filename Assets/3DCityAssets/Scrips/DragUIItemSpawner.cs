using System.Collections;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DragUIItemSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public GameObject prefabToSpawn;

    private RectTransform iconDefaultLocalLocation;
    private GameObject iconParent;


    private bool isGrabed = false;
    private bool allowedToSpawn = false;

    [SerializeField]
    private RaycastHit iconHitInfo;
    [SerializeField]
    private LayerMask placeableLayerMask = 0;

    // [SerializeField]
    // private ObjectPlacementHelper objectPlacementHelper;
    [SerializeField]
    private Vector3 objectPlacementHelperOffset = new Vector3(0,1,0);

    // Example of set and get methods for IsGrabed

    void Awake()
    {
        // objectPlacementHelper = FindAnyObjectByType<ObjectPlacementHelper>();
        iconDefaultLocalLocation = GetComponent<RectTransform>();
        iconParent = transform.parent != null ? transform.parent.gameObject : null;
    }

    void Update()
    {
        if (isGrabed && IsValidPlaceableLocation(out iconHitInfo))
        {

            allowedToSpawn = true;
        }
        else
        {
            allowedToSpawn = false; // Only allow spawning if a valid location is found
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
        // if (objectPlacementHelper != null)
        // {
        //     objectPlacementHelper.HideAllReticles();
        // }
        
        // Check if the parent GameObject is active in the hierarchy
        if (iconParent == null || !iconParent.activeInHierarchy)
        {
            return;
        }

        // Re-parent the GameObject to the original parent in the UI menu
        gameObject.transform.SetParent(iconParent.transform, false); // 'false' ensures local positioning

        // Reset position, rotation, and scale to match the original UI state
        gameObject.transform.localPosition = iconDefaultLocalLocation.localPosition;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
    }


    // FindPlaceableLocation
    // Cast a ray from the GameObject's position downwards to check for a "Placeable Surface" layer


    public bool IsValidPlaceableLocation(out RaycastHit hitInfo)
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 100f; // Adjust as needed

        if (Physics.Raycast(origin, direction, out hitInfo, maxDistance, placeableLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            if (iconHitInfo.transform != null) // Check if the hit info is valid
            {
                Instantiate(prefabToSpawn, iconHitInfo.point, iconHitInfo.transform.rotation);
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
