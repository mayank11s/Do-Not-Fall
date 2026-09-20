using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 offset = new Vector3(0, 15, -15); 
    public float smoothSpeed = 5f;
    
    void Start()
    {
        transform.position = target.position + offset;
    }
    void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position,targetPosition,smoothSpeed * Time.deltaTime);

        transform.LookAt(target,Vector3.up * 1.5f);
    }
}
