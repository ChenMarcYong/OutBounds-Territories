using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControlManagement : MonoBehaviour
{
    // Start is called before the first frame update
    public static SwitchControlManagement instance;

    public GameObject player;
    public GameObject spaceship;

    private bool isInSpaceship = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    public void SwitchToPlayerController()
    {

        UnityEngine.Debug.Log("SwitchToPlayerController!");
        spaceship.GetComponent<SpaceShipController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = true;

        isInSpaceship = false;
    }

    public void SwitchToSpaceShipController()
    {
        UnityEngine.Debug.Log("SwitchToSpaceShipController!");
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
