using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SwitchControlManagement : MonoBehaviour
{
    // Start is called before the first frame update
    public static SwitchControlManagement instance;

    public GameObject player;
    public GameObject spaceship;

    private bool isInSpaceship = false;
    private Rigidbody playerRb;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerRb = player.GetComponent<Rigidbody>();
    }


    public void SwitchToPlayerController()
    {

        UnityEngine.Debug.Log("SwitchToPlayerController!");
        if (playerRb != null)
        {
            playerRb.isKinematic = false; // Empêche la physique de faire tomber le joueur
            playerRb.useGravity = true; // Désactive la gravité
        }

        player.transform.SetParent(null);
        spaceship.GetComponent<SpaceShipController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = true;
        
        isInSpaceship = false;
    }

    public void SwitchToSpaceShipController()
    {

        if (spaceship == null || player == null)
        {
            UnityEngine.Debug.LogError("ERREUR : player ou spaceship est NULL !");
            return;
        }
        
        UnityEngine.Debug.Log("SwitchToSpaceShipController!");

        player.transform.SetParent(spaceship.transform);


        if (playerRb != null)
        {
            playerRb.isKinematic = true; // Empêche la physique de faire tomber le joueur
            playerRb.useGravity = false; // Désactive la gravité
        }


        spaceship.GetComponent<SpaceShipController>().enabled = true;
        player.GetComponent<PlayerController>().enabled = false;

        isInSpaceship = true;

    }

    void InteractWithSpaceship()
    {
        if (!SwitchControlManagement.instance.IsInSpaceship())
        {
            // Si on est en mode Joueur, passer au mode Vaisseau
            UnityEngine.Debug.Log("Le joueur contrôle le vaisseau !");

            SwitchControlManagement.instance.SwitchToSpaceShipController();
        }
        else
        {
            // Si on est déjà dans le vaisseau, repasser en mode Joueur
            UnityEngine.Debug.Log("Le joueur ne contrôle plus le vaisseau !");
            SwitchControlManagement.instance.SwitchToPlayerController();
        }
    }


    public bool IsInSpaceship()
    {
        return isInSpaceship;
    }
}
