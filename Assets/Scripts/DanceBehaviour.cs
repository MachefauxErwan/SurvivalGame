using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private KeyCode danceButton;              // Default Dance button.

    private bool isDancing = false;                     // Boolean to determine whether or not the player activated fly mode.

    void Update()
    {
        // Toggle fly by input, only if there is no overriding state or temporary transitions.
        if (Input.GetKeyDown(danceButton))
        {
            isDancing = !isDancing;

            // Player is flying.
            if (isDancing)
            {
                playerAnimator.SetBool("Dance",true);
                //playerAnimator.Play("Dance");
            }
            else
            {
                playerAnimator.SetBool("Dance", false);
                //playerAnimator.Play("No Aiming");
            }

    
        }
        

    }
}
