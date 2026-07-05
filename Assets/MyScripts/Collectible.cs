using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Einstellungen")]
    public KeyCode pickupKey = KeyCode.Mouse0;

    [Header("Optional")]
    public GameObject pickupEffect;
    public string itemName = "Buch";

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(pickupKey))
        {
            Collect();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Collect()
    {
        Debug.Log(itemName + " wurde eingesammelt!");

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
}