using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100;
    [SerializeField] private float lifespan = 2;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);

        Invoke("DestroyObject", lifespan);
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);

        if(other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
