using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
public GameObject mainMenu; 
public InputActionReference toggleMenuAction;

private void OnEnable()
{
    toggleMenuAction.action.Enable();
    toggleMenuAction.action.performed += OnToggleMenu;
}

private void OnDisable()
{
    toggleMenuAction.action.performed -= OnToggleMenu;
    toggleMenuAction.action.Disable();
}

private void OnToggleMenu(InputAction.CallbackContext context)
{
    if (mainMenu != null)
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
    }
}
}