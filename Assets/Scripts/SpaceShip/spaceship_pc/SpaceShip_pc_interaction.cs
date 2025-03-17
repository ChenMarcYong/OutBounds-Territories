using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpaceShip_pc_interaction : MonoBehaviour
{

    public float interactionRadius = 2f; // Rayon d'interaction
    public string playerTag = "Player"; // Tag du joueur
    public Transform teleportPoint;
    public GameObject obj;

        
    private bool isSwitching = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isSwitching) return;

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
                //InteractWithPC();
                InteractWithPC2();
                StartCoroutine(ResetSwitch());
            }


        }
    }

    void InteractWithPC()
    {
        UnityEngine.Debug.Log("Le joueur interagit avec le PC !");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = teleportPoint.position;
        Quaternion targetRotation = teleportPoint.rotation;

        // Inverser uniquement l'axe Z
        Quaternion newRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y - 180, targetRotation.eulerAngles.z);

        // Appliquer la nouvelle rotation au joueur
        player.transform.rotation = newRotation;// Ajoute ici les actions à effectuer (monter dans le vaisseau, ouvrir un menu, etc.)
    }

    void InteractWithPC2()
    {
        //UnityEngine.Debug.Log("Le joueur interagit avec le PC2 !");

        // Vérifier si SwitchControlManagement existe
        if (SwitchControlManagement.instance == null)
        {
            UnityEngine.Debug.LogError("SwitchControlManagement n'est pas trouvé dans la scène !");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Vérifier si le joueur est en mode vaisseau ou non
        if (!SwitchControlManagement.instance.IsInSpaceship())
        {
            // Le joueur entre dans le vaisseau
            player.transform.position = teleportPoint.position;
            Quaternion targetRotation = teleportPoint.rotation;

            Quaternion newRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y - 180, targetRotation.eulerAngles.z);
            player.transform.rotation = newRotation;

            // Changer de contrôleur
            
            SwitchControlManagement.instance.SwitchToSpaceShipController();
        }
        else
        {
            // Le joueur sort du vaisseau
            
            SwitchControlManagement.instance.SwitchToPlayerController();
        }
    }

    // Affiche la sphère dans l'éditeur pour voir le rayon d'interaction
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(obj.transform.position, interactionRadius);
    }



    IEnumerator ResetSwitch()
    {
        yield return new WaitForSeconds(0.2f); // Temps de sécurité avant de réactiver
        isSwitching = false;
    }
}
