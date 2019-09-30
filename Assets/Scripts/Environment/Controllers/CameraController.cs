using UnityEngine;

public class CameraController : MonoBehaviour
{


    //controller script, courtesy of Phill


    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float acceleration = 4f;

    [SerializeField]
    private float sensitivity = 3f;

    [SerializeField]
    private bool invert = false;

    private Vector3 velocity;

    public GameObject iciclePrefab;

    private void FixedUpdate()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        Movement();
        if (Input.GetMouseButton(0))
        {
            LookingAround();
        }
        FireIcicle();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = 0f;
        float lateral = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Q))
        {
            vertical--;
        }

        if (Input.GetKey(KeyCode.E))
        {
            vertical++;
        }

        Vector3 direction = new Vector3(horizontal, vertical, lateral).normalized;
        direction = transform.TransformDirection(direction);

        float multiplier = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            multiplier = 1.65f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            multiplier = 0.5f;
        }

        velocity = Vector3.MoveTowards(velocity, direction, Time.smoothDeltaTime * acceleration);
        transform.position += velocity * Time.smoothDeltaTime * speed * multiplier;
    }

    private void LookingAround()
    {
        float sourceRatio = 7.2f / (4090909090909f / 250000000000f); //for making a repeating 16.3636
        Vector2 mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouse *= sourceRatio * sensitivity;

        if (invert)
        {
            mouse.y *= -1;
        }

        transform.eulerAngles += new Vector3(mouse.y, mouse.x, 0);
    }

    private void FireIcicle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject icicle = Instantiate(iciclePrefab, transform.position + (-transform.up * 5), transform.rotation);
        }
    }


}
