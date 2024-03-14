using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnHit : MonoBehaviour
{
    [SerializeField] float scaleDownSpeed = -1f;
    GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void ReceiveDamage()
    {

        foreach (var comp in gameObject.GetComponents<Component>())
        {
           if (!(comp is Transform))
           {
               Destroy(comp);
           }
        }
        ScaleDown scaleDown = gameObject.AddComponent<ScaleDown>();
        scaleDown.scaleDownSpeed = scaleDownSpeed;
        gameManager.HareDeath();
    }
}
