using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsMovement : MonoBehaviour, IPlayerAction
{
    //Unity set variables

    /// <summary>
    /// How has the player can move. 
    /// </summary>
    public int MovementSpeed;

    /// <summary>
    /// Camera used to view Player Object.
    /// </summary>
    public Camera PlayerCam;

    /// <summary>
    /// How far the Camera stays behind the player.
    /// </summary>
    public float CamTrailingDist;

    /// <summary>
    /// Ray Tracing distance used to translate muse position to ground coordiantes
    /// </summary>
    public int CamRayLength;   


    //Properties

    /// <summary>
    /// The Player controller referencing this object.  
    /// </summary>
    public PlayerController PlayerController { get; set; }

    //Private Vars

    /// <summary>
    /// Layer Mask for the map floor.
    /// </summary>
    private int groundMask;

    /// <summary>
    /// The Rigidbody object of the Player Object
    /// </summary>
    private Rigidbody playerRigidbody;

    //Functions

    // Start is called before the first frame update
    void Start()
    {
        //Set Ground Layer mask
        groundMask = LayerMask.GetMask("Ground");

        //Find the Player's Rigidbody component
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null) {
            Debug.Log("[Player Movement] No Rigid Body is set on Player Object");
        } 
    }

    /// <summary>
    /// Executes before the PlayerController checks for Player Input.
    /// All actions that should be done before the player performs an action should be placed here. 
    /// </summary>
    public void PreAction()
    {
        return;
    }

    /// <summary>
    /// Executes After the PlayerController checks for Player Input
    /// All actions that should be done after the player performs an action should be placed here.
    /// </summary>
    public void PostAction()
    {
        return;
    }

    /// <summary>
    /// Updates the player'position using physics engine
    /// </summary>
    /// <param name="horizontal">The amount of force applied in the horizontal direction</param>
    /// <param name="vertical">The amount of force applied in the veritcal direction</param>
    public void MovePosition(float horizontal, float vertical)
    {
        //Create new force vector 
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = movement.normalized * MovementSpeed * Time.deltaTime;

        //Move Player's position along force vector
        Rigidbody playerBody = GetComponent<Rigidbody>();
        playerBody.MovePosition(transform.position + movement);

        //Move the camera to stay behind the player
        updateCameraPos();
    }

    /// <summary>
    /// Rotates the pPlayer object based on the mouse's current position.
    /// </summary>
    public void RotatePosition()
    {
        //Create Ray using the mouse postion
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitFloor;

        //Determine if the mouse ray intersects the plane of the ground layer
        if (Physics.Raycast(mouseRay, out hitFloor, CamRayLength, groundMask))
        {
            //Hit detected

            //Get vector representing the difference between the mouse and player
            Vector3 lookDir = hitFloor.point - transform.position;

            //Player cannot look "up"
            lookDir.y = 0.0f;

            //Calculate rotation between current direction and look vector
            Quaternion newRot = Quaternion.LookRotation(lookDir);
            
            //Update players rotation 
            playerRigidbody.MoveRotation(newRot);
        }
    }

    /// <summary>
    /// Moves the Player's assinged camera behind the players current position
    /// </summary>
    private void updateCameraPos() {
        
        //Get the Camera's current position
        Vector3 curPos = PlayerCam.transform.position;
        Vector3 newPos = curPos;

        //Get the Player's current rotation
        Quaternion curRot = PlayerCam.transform.rotation;

        //Update the coordinates of the camera to match the player's accounting for trailing dist.
        newPos.z = transform.position.z - CamTrailingDist;
        newPos.x = transform.position.x;
        newPos.y = curPos.y;

        //Update the postion and rotation of the camera
        PlayerCam.transform.SetPositionAndRotation(newPos,curRot);
    }

}
