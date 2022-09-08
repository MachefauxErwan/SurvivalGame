using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject BigTree;

    [SerializeField]
    private float GrowUpDelay;


    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GrowUp(GrowUpDelay));
    }

    IEnumerator GrowUp(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        GameObject instantiatedRessource = Instantiate(BigTree);
        instantiatedRessource.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }

}
