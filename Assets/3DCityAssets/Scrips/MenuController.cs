using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
public GameObject mainMenu; // Drag your MainMenu GameObject here
public InputActionReference toggleMenuAction; // Reference to the ToggleMenu action

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