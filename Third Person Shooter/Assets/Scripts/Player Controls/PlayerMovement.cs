using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Public movement vars.
    public int MovementSpeed; //How fast the player can move.

    //Vars for Camera Set to Player

    public Camera PlayerCam; //Camera set to player
    public int CamRayLength; //Distance for detecting mouse position on ground layer.
    public float CamTrailingDist; //How far the camera stays behind the player.
    

    private int groundMask; //Layer mask for the map floor. 

    private Rigidbody playerRigidbody;



    // Start is called before the first frame update
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");

        playerRigidbody = GetComponent<Rigidbody>();

        if (playerRigidbody == null) {
            Debug.Log("[Player Movement] No Rigid Body is set on Player Object");
        } 
    }

    // Fixed Update is called every .2 seconds with the physics layer.
    public void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movePosition(h,v);
        rotatePosition();
    }

    private void movePosition(float horizontal, float vertical)
    {

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = movement.normalized * MovementSpeed * Time.deltaTime;

        Rigidbody playerBody = GetComponent<Rigidbody>();
        playerBody.MovePosition(transform.position + movement);

        updateCameraPos();
    }

    private void updateCameraPos() {
        Vector3 curPos = PlayerCam.transform.position;
        Vector3 newPos = curPos;

        newPos.z = transform.position.z - CamTrailingDist;
        newPos.x = transform.position.x;
        newPos.y = curPos.y;

        Quaternion curRot = PlayerCam.transform.rotation;

        PlayerCam.transform.SetPositionAndRotation(newPos,curRot);
    }

    private void rotatePosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitFloor;

        if (Physics.Raycast(mouseRay, out hitFloor, CamRayLength, groundMask))
        {
            Vector3 lookDir = hitFloor.point - transform.position;

            lookDir.y = 0.0f;

            Quaternion newRot = Quaternion.LookRotation(lookDir);
            playerRigidbody.MoveRotation(newRot);
        }
    }
}
