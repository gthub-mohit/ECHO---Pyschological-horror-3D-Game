using UnityEngine;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float interactDistance = 3.5f;
    public LayerMask interactLayer;

    [Header("UI Reference")]
    public TextMeshProUGUI promptTextUI;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        if (promptTextUI != null) promptTextUI.text = "";
    }

    void Update()
    {
        if (cam == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        // Visual Raycast debug in Scene view (Red line)
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (promptTextUI != null)
                {
                    promptTextUI.text = interactable.GetPromptText();
                }

                // Check Interact Key (E)
                bool pressedE = false;
                #if ENABLE_INPUT_SYSTEM
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    pressedE = true;
                }
                #else
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pressedE = true;
                }
                #endif

                if (pressedE)
                {
                    interactable.Interact();
                }
                return;
            }
        }

        if (promptTextUI != null && promptTextUI.text != "")
        {
            promptTextUI.text = "";
        }
    }
}