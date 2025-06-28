using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 5f;
    public float lookXLimit = 50f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private Animator animator;
    private bool isMoving, alreadyAttacked = false, canMove = true;

    // Attacking System //
    public GameObject proyectile;
    public float timeBetweeAttacks;
    public Transform bulletSpawn;

    public float attackDistance = 50f;
    public LayerMask attackLayer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        isMoving = moveDirection.Equals(Vector3.zero);
        //Debug.Log("Trigger: " + animator.GetBool("attack"));

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        //-------| Funcion Ataque Distancia |-------
        if(Input.GetKey(KeyCode.E) && !alreadyAttacked)
        {
            /// Attack code here
            alreadyAttacked = true;

            Rigidbody rb = Instantiate(proyectile, bulletSpawn.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.gameObject.GetComponent<BulletScript>().creador = this.gameObject;
            
            rb.AddForce(bulletSpawn.forward * 32f, ForceMode.Impulse);
            rb.AddForce(bulletSpawn.up * 1f, ForceMode.Impulse);

            Invoke(nameof(ResetAttack), timeBetweeAttacks);
        }

        //------| Raycast |-------
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            //Debug.Log(attackLayer.value);
        }

        //Movimiento
        characterController.Move(moveDirection * Time.deltaTime);
        animator.SetBool("isRunning", !isMoving);

        //-------| Funcion Ataque Meele |-------
        if(Input.GetKey(KeyCode.Q) && !alreadyAttacked)
        {
            canMove = false;
            alreadyAttacked = true;
            animator.SetBool("isAttacking", alreadyAttacked);

            //LLama a una funcion luego de un tiempo definido
            Invoke(nameof(ResetAttack), timeBetweeAttacks);
        }

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void ResetAttack(){
        alreadyAttacked = false;
        canMove = true;
        animator.SetBool("isAttacking", alreadyAttacked);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * attackDistance);
    }
}
