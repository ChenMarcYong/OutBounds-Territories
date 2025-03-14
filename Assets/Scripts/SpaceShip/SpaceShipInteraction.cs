using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpaceShipInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    public float interactionRadius = 5f; // Rayon d'interaction
    public string playerTag = "Player"; // Tag du joueur
    public Transform teleportPoint;
    public GameObject obj;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, interactionRadius);
        bool playerInRange = false;

        foreach (Collider collider in hitColliders)
        {
            //UnityEngine.Debug.Log(collider.tag);
            if (collider.CompareTag(playerTag))
            {
                playerInRange = true;
                break;
            }
        }

        if (playerInRange)
        {
            //UnityEngine.Debug.Log("Appuie sur 'E' pour interagir avec le vaisseau !");
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithSpaceship();
            }
        }
    }

    void InteractWithSpaceship()
    {
        UnityEngine.Debug.Log("Le joueur interagit avec le vaisseau !");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = teleportPoint.position;
        player.transform.rotation = teleportPoint.rotation; // Aligner l'orientation du joueur sur celle du vaisseau
        // Ajoute ici les actions à effectuer (monter dans le vaisseau, ouvrir un menu, etc.)
    }

    // Affiche la sphère dans l'éditeur pour voir le rayon d'interaction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(obj.transform.position, interactionRadius);
    }
}
