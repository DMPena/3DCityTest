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
        socket.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        socket.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        var icon = args.interactableObject.transform.GetComponent<UIIconGrab>();
        if (icon != null && icon.prefabToSpawn != null)
        {
            Instantiate(icon.prefabToSpawn, socket.transform.position, socket.transform.rotation);
            Destroy(icon.gameObject);
        }
    }
}

