using UnityEngine;

public class EchoObjectTest : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptMessage = "[E] Inspect Object";

    public string GetPromptText()
    {
        return promptMessage;
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
    }
}