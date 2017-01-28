using UnityEngine;

public class RocketFollower : MonoBehaviour {

    public Transform rocket;
    public float minX, maxX;


    void Start() {
        minX = transform.position.x - 3f;
        maxX = transform.position.x + 3f;
    }

    // Update is called once per frame
    void Update() {
        if(rocket != null) {
            Vector3 position = rocket.position;
            position.z = transform.position.z;
            position.y = position.y + 3;

            if(position.x < minX) {
                position.x = minX;
            } else if(position.x > maxX) {
                position.x = maxX;
            }

            Vector3 lerped = Vector3.Lerp(transform.position, position, 4f * Time.deltaTime);

            transform.position = lerped;
        }
    }
}
