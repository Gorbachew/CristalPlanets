
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector2 startPos;
    private Camera cam;

    private void Awake()
    {
        Debug.Log("Awake CameraMove");
        cam = GetComponent<Camera>();

    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))   
        {
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            float pos = cam.ScreenToWorldPoint(Input.mousePosition).x - startPos.x;
            transform.position = new Vector3(Mathf.Clamp( transform.position.x - pos, 14.5f,26.5f),transform.position.y,transform.position.z);
        }
    }





}
