using UnityEngine;

public class ObjectPlacementHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject validPlaceReticle;

    [SerializeField]
    private GameObject invalidPlaceReticle;

    private bool isValidPlacement;

    // Public property to set or get the placement validity
    public bool IsValidPlacement
    {
        get => isValidPlacement;
        set
        {
            isValidPlacement = value;
            ToggleReticles(); // Automatically update reticles when the value changes
        }
    }

    // Toggles the visibility of the reticles based on placement validity
    public void ToggleReticles()
    {
        if (validPlaceReticle == null || invalidPlaceReticle == null)
        {
            Debug.LogWarning("Reticle references are not assigned.");
            return;
        }

        validPlaceReticle.SetActive(isValidPlacement);
        invalidPlaceReticle.SetActive(!isValidPlacement);
    }

    // Hides all reticles
    public void HideAllReticles()
    {
        if (validPlaceReticle == null || invalidPlaceReticle == null)
        {
            Debug.LogWarning("Reticle references are not assigned.");
            return;
        }

        validPlaceReticle.SetActive(false);
        invalidPlaceReticle.SetActive(false);
    }

    private void Awake()
    {
        HideAllReticles(); // Ensure reticles are inactive at the start
    }

    // Update the reticle visibility based on the current placement validity
    public void UpdateReticleState()
    {
        ToggleReticles();
    }
}

