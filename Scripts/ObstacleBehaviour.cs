using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    enum MovementMode 
    {
        Linear,
        Circular
    };

    [SerializeField] MovementMode movementMode;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period;

    [SerializeField] float speed = 1.5f;
    [SerializeField] float width;
    [SerializeField] float height;
    Vector3 startingPosition;
    Vector3 offset;

    Vector3 circleOffset;

    const float tau = 2 * Mathf.PI;
    float cycles;
    float sineWave;
    float movementFactor;
    float circleX;
    float circleY;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementMode == MovementMode.Linear)
        {
            LinearMove();
        }
        else if (movementMode == MovementMode.Circular)
        {
            CircularMove();
        }
    }

    void LinearMove ()
    {
        if (period < float.Epsilon)
        {
            period = float.Epsilon;
        }
        cycles = Time.time / period;
        sineWave = Mathf.Sin(cycles * tau);
        movementFactor = (sineWave + 1.0f) / 2.0f;
        offset = movementFactor * movementVector;
        transform.position = startingPosition + offset;
    }

    void CircularMove ()
    {
        cycles += Time.deltaTime * speed;
        circleX = Mathf.Cos(cycles) * width;
        circleY = Mathf.Sin(cycles) * height;
        circleOffset = new Vector3 (circleX, circleY, 0.0f);

        transform.position  = startingPosition + circleOffset;
    }
}
