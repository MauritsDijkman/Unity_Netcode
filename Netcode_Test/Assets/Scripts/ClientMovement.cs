using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class ClientMovement : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform orientation;

        [Header("Jump")]
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float airMultiplier;
        private bool readyToJump;

        [Header("Ground Check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private float groundDrag;
        [SerializeField] public LayerMask WhatIsGround;
        private bool grounded;

        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDirection;
        private Rigidbody rb;

        private void Start()
        {
            if (!IsOwner)
                return;
            else
            {
                rb = GetComponent<Rigidbody>();
                rb.freezeRotation = true;
                readyToJump = true;
            }
        }

        /**
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
                Move();
        }

        
        public void Move()
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                if (IsOwner)
                    SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = transform.position;
        }
        /**/

        private void Update()
        {
            if (IsOwner && NetworkManager.Singleton.IsClient)
            {
                HandleInput();

                //if (IsLocalPlayer)
                //    SubmitPositionRequestServerRpc();
            }
        }

        private void FixedUpdate()
        {
            if (IsOwner && NetworkManager.Singleton.IsClient)
            {
                MovePlayer();

                //if (IsLocalPlayer)
                //    SubmitPositionRequestServerRpc();
            }
        }

        private void HandleInput()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

            MyInput();
            SpeedControl();

            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0.0f;
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        private void MovePlayer()
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f, ForceMode.Force);
            else
                rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f * airMultiplier, ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyToJump = true;
        }
    }
}
