using UnityEngine;

public class ObjectPlacementHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject validReticle;
    [SerializeField]
    private GameObject invalidReticle;

    public bool IsValidPlacement { get; set; }

    public void ShowValidReticle(Vector3 position)
    {
        if (validReticle != null)
        {
            validReticle.SetActive(true);
            validReticle.transform.position = position;
        }

        if (invalidReticle != null)
        {
            invalidReticle.SetActive(false);
        }
    }

    public void ShowInvalidReticle(Vector3 position)
    {
        if (invalidReticle != null)
        {
            invalidReticle.SetActive(true);
            invalidReticle.transform.position = position;
        }

        if (validReticle != null)
        {
            validReticle.SetActive(false);
        }
    }

    public void HideAllReticles()
    {
        if (validReticle != null)
        {
            validReticle.SetActive(false);
        }

        if (invalidReticle != null)
        {
            invalidReticle.SetActive(false);
        }
    }
}

