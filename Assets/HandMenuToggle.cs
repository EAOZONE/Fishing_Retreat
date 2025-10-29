using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class HandMenuToggle : MonoBehaviour
{
    public InputActionReference openMenuAction;
    public GameObject menuPrefab;

    private GameObject instance;
    private Transform cam;

    void Awake()
    {
        var origin = FindObjectOfType<XROrigin>();
        cam = origin ? origin.Camera.transform : Camera.main.transform;

        instance = Instantiate(menuPrefab, transform);
        instance.SetActive(false);
    }

    void OnEnable()
    {
        openMenuAction.action.performed += Toggle;
        openMenuAction.action.Enable();
    }

    void OnDisable()
    {
        openMenuAction.action.performed -= Toggle;
        openMenuAction.action.Disable();
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        instance.SetActive(!instance.activeSelf);
    }
}
