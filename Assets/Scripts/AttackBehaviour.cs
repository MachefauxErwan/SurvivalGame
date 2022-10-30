using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [Header("Referencies")]

    [SerializeField]
    private Animator animator;

    [SerializeField]
     private Equipment equipmentSystem;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private InteractBehaviour interactBehaviour;

    [Header("Configuration")]
    private bool isAttacking;
    // Start is called before the first frame update
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private Vector3 attackOffset;

    [SerializeField]
    private LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red);
        if(Input.GetMouseButtonDown(0) && CanAttack())
        {
            isAttacking = true;
            SendAttack();
            animator.SetTrigger("Attack"); 
        }
            
    }
     void SendAttack()
    {
        Debug.Log("Attack sent !");

        RaycastHit hit;
        if(Physics.Raycast(transform.position + attackOffset, transform.forward,out hit ,attackRange,layerMask))
        {
            if(hit.transform.CompareTag("AI"))
            {
                EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
                enemy.TakeDammage(equipmentSystem.equipedWeaponItem.AttackPoints);
            }
        }
    }

    bool CanAttack()
    {
        /*pour attaquer on doit : 
            - Avoir une arme équipée
            - Ne pas être en train d'attaquer
            - Ne pas avoir l'inventaire ouvert
            - Ne pas être occupée (couper,miner, etc...)
        */
        return equipmentSystem.equipedWeaponItem != null && !isAttacking && !uiManager.atLeastOnePanelOpened && !interactBehaviour.isBusy;
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }
}
