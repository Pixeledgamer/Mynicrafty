using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    public float mouseSensitivity = 10f;

    public Transform Player;

    public float gravity = -9.81f;

    public LayerMask Mask;
    bool isgrounded;

    [SerializeField]
    Vector3 velocity;

    public float jumpHeight = 1f;

    public float speed = 5f;

    public string MODE = "WIN";

    public FixedJoystick joy;

    public CharacterController controller;

    float xRot = 0f;

    public Mynicrafty controlMaster;


    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Player = transform;
        controller = transform.GetComponent<CharacterController>();
    }
    private void Awake()
    {
        controlMaster = new Mynicrafty();
        MODE = "WIN";
    }
    private void OnEnable()
    {
        controlMaster.Player.Move.Enable();
        controlMaster.Player.Look.Enable();
        controlMaster.Player.Jump.Enable();
        controlMaster.Player.Fire.Enable();
    }

    private void OnDisable()
    {
        controlMaster.Player.Move.Disable();
        controlMaster.Player.Look.Disable();
        controlMaster.Player.Jump.Disable();
        controlMaster.Player.Fire.Disable();
    }

    float mouseX;
    float mouseY;

    float x;
    float z;

    public void Update()
    {
        x = new float();
        z = new float();

        Vector3 position = Player.position + new Vector3(0, -1f, 0);

        isgrounded = Physics.CheckBox(position, new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity, Mask);
        if (isgrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (MODE == "WIN")
        {
            x = controlMaster.Player.Move.ReadValue<Vector2>().x;
            z = controlMaster.Player.Move.ReadValue<Vector2>().y;
        }

        mouseX = controlMaster.Player.Look.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        mouseY = controlMaster.Player.Look.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

        if (MODE == "AND")
        {
            if (!EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
            {
                x = joy.Horizontal;
                z = joy.Vertical;
            }
        }
        Vector3 move = transform.right * x + transform.forward * z;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(move * Time.deltaTime * speed);

        controller.Move(velocity * Time.deltaTime);

        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.GetChild(0).localRotation = Quaternion.Euler(xRot, 0f, 0f);

        Player.Rotate(Vector3.up * mouseX);

        controlMaster.Player.Jump.performed += Onjump;
        controlMaster.Player.Fire.started += Click;

    }

    public void Onjump(InputAction.CallbackContext obj)
    {
        if (isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }

    public void Click(InputAction.CallbackContext obj)
    {
        RaycastHit HITINFO;
        Ray ray = new Ray();
        ray.direction = transform.GetChild(0).transform.forward;
        ray.origin = transform.GetChild(0).transform.position;

        if (Physics.Raycast(ray, out HITINFO, 5, Mask))
        {

            Vector3 hitBlock = HITINFO.point - HITINFO.normal / 2f;
            Vector3 pos = new Vector3(
                (int)(Mathf.FloorToInt(hitBlock.x) - HITINFO.collider.gameObject.transform.position.x),
                (int)(Mathf.FloorToInt(hitBlock.y) - HITINFO.collider.gameObject.transform.position.y),
                (int)(Mathf.FloorToInt(hitBlock.z) - HITINFO.collider.gameObject.transform.position.z)
                );

            List<ChunkScript> chunks = new List<ChunkScript>();


            if (pos.x == 0)
            {
                try
                {
                    chunks.Add(Worlder.Chunks[new Vector3(
                        (HITINFO.collider.transform.position.x / 16) - 1,
                        HITINFO.collider.transform.position.y / 16,
                        HITINFO.collider.transform.position.z / 16
                        )]);
                }
                catch (KeyNotFoundException)
                {

                }
            }
            if (pos.y == 0)
            {
                try
                {
                    chunks.Add(Worlder.Chunks[new Vector3(
                        HITINFO.collider.transform.position.x / 16,
                        (HITINFO.collider.transform.position.y / 16) - 1,
                        HITINFO.collider.transform.position.z / 16
                        )]);
                }
                catch (KeyNotFoundException)
                {

                }
            }
            if (pos.z == 0)
            {
                try
                {
                    chunks.Add(Worlder.Chunks[new Vector3(
                        HITINFO.collider.transform.position.x / 16,
                        HITINFO.collider.transform.position.y / 16,
                        (HITINFO.collider.transform.position.z / 16) - 1
                        )]);
                }
                catch (KeyNotFoundException)
                {

                }
            }

            if (pos.x == DATAClasses.Chunksize - 1)
            {
                try
                {
                    chunks.Add(Worlder.Chunks[new Vector3(
                        (HITINFO.collider.transform.position.x / 16) + 1,
                        HITINFO.collider.transform.position.y / 16,
                        HITINFO.collider.transform.position.z / 16
                        )]);
                }
                catch (KeyNotFoundException)
                {

                }
            }
            if (pos.y == DATAClasses.Chunksize - 1)
            {
                try
                {
                    chunks.Add(Worlder.Chunks[new Vector3(
                        HITINFO.collider.transform.position.x / 16,
                        (HITINFO.collider.transform.position.y / 16) + 1,
                        HITINFO.collider.transform.position.z / 16
                        )]);
                }
                catch (KeyNotFoundException)
                {

                }
            }
            if (pos.z == DATAClasses.Chunksize - 1)
            {
                try
                {
                    chunks.Add(Worlder.Chunks[new Vector3(
                        HITINFO.collider.transform.position.x / 16,
                        HITINFO.collider.transform.position.y / 16,
                        (HITINFO.collider.transform.position.z / 16) + 1
                        )]);
                }
                catch (KeyNotFoundException)
                {

                }
            }

            Worlder.Chunks[HITINFO.collider.transform.position / 16].Break(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z));

            foreach (ChunkScript g in chunks)
            {
                g.UpdateChunk();
            }
        }


    }
}



