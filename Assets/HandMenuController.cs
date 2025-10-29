using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class HandMenuController : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject handMenuPrefab;     // assign your HandMenu prefab
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 0.08f, 0.12f);
    [SerializeField] private Vector3 localEuler = new Vector3(0f, 0f, 0f);
    [SerializeField] private bool faceCamera = true;

    [Header("Input")]
    [SerializeField] private InputActionReference openMenuAction; // <XRController>{LeftHand}/primaryButton

    private GameObject _menuInstance;
    private Transform _xrCamera;

    private void Awake()
    {
        var origin = FindObjectOfType<XROrigin>();
        _xrCamera = origin != null ? origin.Camera.transform : Camera.main?.transform;

        if (handMenuPrefab != null)
        {
            _menuInstance = Instantiate(handMenuPrefab);
            _menuInstance.transform.SetParent(transform, worldPositionStays: false);
            _menuInstance.transform.localPosition = localOffset;
            _menuInstance.transform.localEulerAngles = localEuler;
            _menuInstance.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (openMenuAction != null)
        {
            openMenuAction.action.performed += OnOpenMenu;
            openMenuAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (openMenuAction != null)
        {
            openMenuAction.action.performed -= OnOpenMenu;
            openMenuAction.action.Disable();
        }
    }

    private void LateUpdate()
    {
        if (faceCamera && _menuInstance != null && _menuInstance.activeSelf && _xrCamera != null)
        {
            // Billboard toward the HMD (keep upright)
            Vector3 dir = _xrCamera.position - _menuInstance.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                _menuInstance.transform.forward = dir.normalized;
        }
    }

    private void OnOpenMenu(InputAction.CallbackContext _)
    {
        if (_menuInstance == null) return;
        _menuInstance.SetActive(!_menuInstance.activeSelf);
    }
}
