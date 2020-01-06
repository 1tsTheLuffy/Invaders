using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spaceship : MonoBehaviour
{
    private Vector2 mousePos;
    private Vector2 movement;
    [SerializeField] float movementSpeed;
    [SerializeField] float destroyTime;

    [SerializeField] GameObject fireParticle;

    [SerializeField] Transform point;

    [SerializeField] CinemachineVirtualCamera virtaulCamera;
    private CinemachineBasicMultiChannelPerlin virtualNoiseCamera;

    [SerializeField] float elapsedTime;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeAmplitude;
    [SerializeField] float shakeFrequency;

    Rigidbody2D rb;
    Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        if (virtaulCamera != null)
        {
            virtualNoiseCamera = virtaulCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }

    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        transform.up = direction;

        if(Input.GetMouseButtonDown(0))
        {
            elapsedTime = shakeDuration;
        }

        CameraShake();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * movementSpeed * Time.fixedDeltaTime, 
            movement.y * movementSpeed * Time.fixedDeltaTime);

        GameObject destroy = Instantiate(fireParticle, point.position, Quaternion.identity);
        Destroy(destroy, destroyTime);
    }

    private void CameraShake()
    {
        if (elapsedTime > 0)
        {
            virtualNoiseCamera.m_AmplitudeGain = shakeAmplitude;
            virtualNoiseCamera.m_FrequencyGain = shakeFrequency;
            elapsedTime -= Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
            virtualNoiseCamera.m_FrequencyGain = 0;
            virtualNoiseCamera.m_AmplitudeGain = 0;
        }
    }
}
