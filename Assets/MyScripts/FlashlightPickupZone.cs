using UnityEngine;

public class FlashlightPickupZone : MonoBehaviour
{
    public GameObject promptUI;       // PickupPromptText Objekt
    public GameObject flashlightIcon; // FlashlightIcon Objekt
    public GameObject flashlightWorldObject; // die Taschenlampe in der Welt (zum Verstecken/Zerstören)

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetMouseButtonDown(0)) // Rechte Maustaste
        {
            PickUpFlashlight();
        }
    }

    private void PickUpFlashlight()
    {
        promptUI.SetActive(false);
        flashlightIcon.SetActive(true);

        if (flashlightWorldObject != null)
            flashlightWorldObject.SetActive(false); // oder Destroy(flashlightWorldObject);

        playerInRange = false;

        // Optional: globalen Zustand setzen, z.B. für Taschenlampen-Funktion
        // PlayerInventory.Instance.HasFlashlight = true;
    }
}