using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public NetworkVariable<Color> PlayerColor = new NetworkVariable<Color>(Color.red);

    public float movementSpeed = 50f;
    public float rotationSpeed = 130f;
    private Camera playerCamera;
    private GameObject playerBody;

    private void NetworkInit() {
    playerBody = transform.Find("PlayerBody").gameObject;

    playerCamera = transform.Find("Camera").GetComponent<Camera>();
    playerCamera.enabled = IsOwner;
    playerCamera.GetComponent<AudioListener>().enabled = IsOwner;

    ApplyPlayerColor();
    PlayerColor.OnValueChanged += OnPlayerColorChanged;
    }

    private void Awake() {
        NetworkHelper.Log(this, "Awake");
    }

    void Start(){
        NetworkHelper.Log(this, "Start");
    }


    public override void OnNetworkSpawn() {
        NetworkHelper.Log(this, "OnNetworkSpawn");
        NetworkInit();
        base.OnNetworkSpawn();
    }

         void Update() {
        if (IsOwner) {
            OwnerHandleMovementInput();
        }
    }

        public void OnPlayerColorChanged(Color previous, Color current) {
        ApplyPlayerColor();
    }
    
    [ServerRpc]
    private void MoveServerRpc(Vector3 posChange, Vector3 rotChange)
    {
        transform.Translate(posChange, Space.World);
        transform.Rotate(rotChange);
    }

    // Rotate around the y-axis when shift isn't pressed
    private Vector3 CalcRotation() {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Vector3 rotVect = Vector3.zero;

        if (!isShiftKeyDown)
        {
            rotVect = new Vector3(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);
        }

        return rotVect;
    }

    // Move up and back, and strafe when shift is pressed
    private Vector3 CalcMovement()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float x_move = 0.0f;
        float z_move = Input.GetAxis("Vertical");

        if (isShiftKeyDown)
        {
            x_move = Input.GetAxis("Horizontal");
        }

        Vector3 moveVect = new Vector3(x_move, 0, z_move);
        moveVect *= movementSpeed * Time.deltaTime;

        return moveVect;
    }

    private void OwnerHandleMovementInput() {
        Vector3 movement = CalcMovement();
        Vector3 rotation = CalcRotation();

        if (movement != Vector3.zero || rotation != Vector3.zero) {
            MoveServerRpc(movement, rotation);
        }
    }


    public void ApplyPlayerColor() {
        NetworkHelper.Log(this, $"Applying color {PlayerColor.Value}");
        playerBody.GetComponent<MeshRenderer>().material.color = PlayerColor.Value;
    }
}
