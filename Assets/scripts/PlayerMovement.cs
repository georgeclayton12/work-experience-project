using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera view;
    private float horizontal;
    private float speed = 8f;
    [SerializeField] private float jumpingPower = 5f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    private void Start()
    {
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            Mine();
        }


        // if I click with the right mouse button
        // Call the mine fucntion

        Flip();


        Vector3 mousePosition = Input.mousePosition;

        Vector3 mouseWorldPosition = view.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -view.transform.position.z));

        Vector2 direction = (Vector2)mouseWorldPosition - (Vector2)transform.position;

        direction.Normalize();

        Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 10, Color.yellow);
        
    }

    public void Mine()
    {
        //Vector3 mousePos = view.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8));
        //Vector3 direction = mousePos - transform.position;

        //direction.z = 0;

        //Vector3 mouseLocation = transform.position + direction;

        //Debug.DrawLine(transform.position, mouseLocation, Color.magenta, 10f);

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10f, groundLayer);
        //if (hit)
        //{
        //    Debug.Log("hit - " + hit.collider.gameObject.name);
        //    Destroy(hit.collider.gameObject);


        //}

        Vector3 mousePosition = Input.mousePosition;

        Vector3 mouseWorldPosition = view.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -view.transform.position.z));

        Vector2 direction = (Vector2)mouseWorldPosition - (Vector2)transform.position;

        direction.Normalize();

        Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 10, Color.yellow);


        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10f, groundLayer);
        if (hit)
        {
            Destroy(hit.collider.gameObject);
        }
    }


    // Mine function
    // Do a 2D raycast from the play towards the mouse point
    // if that hits an object
    // if that object is dirt
    // destroy that dirt piece game object

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded() 
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }
    private void Flip() 
    { 
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 LocalScale = transform.localScale;
            LocalScale.x *= -1f;
            transform.localScale = LocalScale;

        }  
    }
}
