using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class playerScript : NetworkBehaviour
{
    public bool bigGuy;
    public float raycastYDist;
    public Vector2 mSens;
    public bool canJump;
    bool spaceUp;
    public Camera cam;
    Rigidbody rb;
    float jumpSpeed;
    void Start()
    {
        transform.position = GameObject.Find("spawner1").transform.position;
        if (!isLocalPlayer)
        {
            return;
            Destroy(cam);
        }
        if (bigGuy)
        {
            transform.localScale *= 50;
            GetComponent<CapsuleCollider>().radius = 0.1f;
        }
        transform.position = GameObject.Find("spawner2").transform.position;
        jumpSpeed = 1;
        raycastYDist *= transform.localScale.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (bigGuy)
        {
            jumpSpeed = 4;
            canJump = false;
        }
        else
            canJump = (Physics.Raycast(transform.position, Vector3.down, raycastYDist));
        Vector3 localEA = transform.localEulerAngles;
        transform.Rotate(-localEA.x, 0, -localEA.z);
        Vector3 vel = rb.velocity;
        Vector3 nVel = (transform.forward * Input.GetAxis("Vertical") * 5 + transform.right * Input.GetAxis("Horizontal") * 5)*jumpSpeed;
        nVel.y = vel.y;
        if (!Input.GetKey(KeyCode.Space))
            spaceUp = true;
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            if (!spaceUp)
                jumpSpeed += 0.2f;
            else
                jumpSpeed = 1;
            spaceUp = false;
            nVel.y = 10;

        }
        if (canJump && !Input.GetKey(KeyCode.Space))
        {
            jumpSpeed = 0;
            spaceUp = true;
        }
        
        rb.velocity = nVel;
        transform.localEulerAngles = localEA + new Vector3(0, Input.GetAxis("Mouse X") * mSens.x, 0) * Time.deltaTime;
        cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x - Input.GetAxis("Mouse Y"), 0, 0);
    }
}
