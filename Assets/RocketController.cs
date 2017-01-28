using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour {

    float speed = 7f;
    float rotationSpeed = 150f;
    float height;
    float toIncrease = 50f;
    Timer timer;
    bool hasCrashed;
    TextMesh text;

    float minX = -5.5f, maxX = 5.5f;

    public GameObject Explosion;
    public Animator animator;
    public GameObject speedHUD;


    void Start() {
        animator = Explosion.GetComponent<Animator>();
        text = speedHUD.GetComponent<TextMesh>();

        timer = new Timer(delegate {
            speed += 0.001f;
            toIncrease -= 0.01f;
        }, null, 0, (int)toIncrease);
    }

    // Update is called once per frame
    void Update() {
        if(hasCrashed)
            return;

        text.text = "Speed: " + speed + " m/s";

        height = transform.position.y;
        float horizontal = Input.GetAxis("Horizontal");
        if(Input.touchCount > 0) {
            float pos = Input.GetTouch(0).position.x;
            int center = (int)pos / 2;
            if(pos < center) {
                horizontal = -1;
            } else {
                horizontal = 1;
            }
        }

        //TODO: on touch event set horizontal

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale > 0)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        } else {
            //Rotate
            Quaternion rotation = transform.rotation;
            float z = rotation.eulerAngles.z;

            z -= horizontal * rotationSpeed * Time.deltaTime;


            if(z > 75 && z <= 180) {
                z = 75;
            } else if(z < 285 && z > 180) {
                z = 285;
            }

            rotation = Quaternion.Euler(0, 0, z);
            transform.rotation = rotation;


            //Move
            Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);


            if(transform.position.x < minX || transform.position.x > maxX) {
                SceneManager.LoadScene("Scene_GameOver");
            }

            transform.position += rotation * velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        transform.position = new Vector3(10, 10, 1);
    }
    void OnCollisionEnter(Collision coll) {
        transform.position = new Vector3(10, 10, 1);
    }
    void OnTriggerEnter(Collider coll) {
        transform.position = new Vector3(10, 10, 1);
    }
    void OnTriggerEnter2D(Collider2D coll) {
        transform.position = new Vector3(10, 10, 1);
    }
}
