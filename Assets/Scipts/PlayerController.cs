using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isGameOver = false;

    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPt;

    private Rigidbody rb;
    private CapsuleCollider col;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver) return;
        
        Move();        

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, bulletSpawnPt.position, transform.rotation);
        }
    }

    void Move()
    {
        //movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        rb.AddForce (moveDirection * moveSpeed);        

        //rotation
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(mouseRay, out rayLength))
        {
            Vector3 pointToLook = mouseRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            health--;
            Debug.Log(health);
            if(health <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Debug.Log("game over");
        isGameOver = true;
    }
}
