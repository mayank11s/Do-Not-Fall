using UnityEngine;

public class EliminatorCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") || collider.CompareTag("Bot"))
        {
            Debug.Log(collider.gameObject.name);
            GameManager.Instance.CharacterEliminate(collider.gameObject);
        }
    }
}
