using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketDropHandler : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntering);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntering);
    }

    private void OnSelectEntering(SelectEnterEventArgs args)
    {
        var icon = args.interactableObject.transform.GetComponent<UIIconGrab>();
        if (icon != null && icon.prefabToSpawn != null)
        {
            Instantiate(icon.prefabToSpawn, socket.transform.position, socket.transform.rotation);
        }
    }

    public void showInfo()
    {
        Debug.Log("show info working");
    }
}

