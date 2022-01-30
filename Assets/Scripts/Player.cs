using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraHolder;
    public Transform _camera;
    public Transform flashlight;
    public FlashlightPower flashlightPower;
    public Power power;
    public StaminaMeter staminaMeter;

    public float runSpeed = 2.5f;
    public float walkSpeed = 1.5f;
    public float exhaustedSpeed = 1f;

    [HideInInspector]
    public bool dead = false;

    // Movement and camera
    private float currentLookY;
    private float baseCameraFieldOfView;
    private float headBobTimer;
    private Vector3 velocity = Vector3.zero;

    [HideInInspector]
    public Vector3 startPosition;

    // Stamina variables
    private float currentStamina = 0;
    public float maximumStamina = 2f;
    private float rechargeWait = 1;
    private bool runCoolDown;
    private bool staminaMeterFlash;
    private float staminaMeterFlashTimer;

    private float flashlightIntensity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Office")
        {
            power.inOffice = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Office")
        {
            power.inOffice = false;
        }
    }

    private void Start()
    {
        flashlightIntensity = flashlight.GetComponent<Light>().intensity;
        flashlight.GetComponent<Light>().intensity = 0f;
        baseCameraFieldOfView = _camera.GetComponent<Camera>().fieldOfView;
        startPosition = gameObject.transform.position;
        if (!Application.isMobilePlatform)
            Cursor.lockState = CursorLockMode.Locked;
    }

    void PlayerMovement()
    {
        // Look & move
        var lookX = Input.GetAxis("Mouse X") * 4f;
        var lookY = Input.GetAxis("Mouse Y") * 4f;
        var moveSide = Input.GetAxisRaw("Horizontal");
        var moveForward = Input.GetAxisRaw("Vertical");

        // Manage momentum
        if (moveSide != 0)
        {

        }
        if (moveForward != 0)
        {

        }

        // Move the character
        Vector3 move = (moveSide * gameObject.transform.right) + (moveForward * gameObject.transform.forward);

        var desiredFOV = baseCameraFieldOfView;
        float maxSpeed = walkSpeed;

        // If no movement has occurred, the expected maximum speed should be zero
        if (move.magnitude <= 0)
            maxSpeed = 0;

        // Stamina & Run
        if (Input.GetButton("Run") && move.magnitude > 0 && (characterController.velocity.x != 0 || characterController.velocity.z != 0) && !runCoolDown && moveForward > 0)
        {
            if (currentStamina > 0 && !runCoolDown)
            {
                desiredFOV += 5;
                rechargeWait = 0;
                currentStamina -= Time.deltaTime;
                maxSpeed = runSpeed;
            }
            else if (currentStamina <= 0)
            {
                runCoolDown = true;
                rechargeWait = 1f;
            }
        }
        else
        {
            if (rechargeWait < 1)
            {
                rechargeWait += Time.deltaTime;
            }
            else if (rechargeWait >= 1)
            {
                rechargeWait = 1;
                if (currentStamina < maximumStamina)
                {
                    if (!runCoolDown)
                        currentStamina += Time.deltaTime * 2;
                    else
                        currentStamina += Time.deltaTime * 0.5f;
                    if (currentStamina > maximumStamina)
                    {
                        runCoolDown = false;
                    }
                }
            }
        }

        // Ran out of stamina
        if (runCoolDown)
        {
            desiredFOV -= 5;
            staminaMeterFlashTimer += Time.deltaTime;
            if (staminaMeterFlashTimer > 0.125f)
            {
                staminaMeterFlashTimer = 0;
                staminaMeterFlashTimer -= 0.125f;
                staminaMeterFlash = !staminaMeterFlash;
            }
            staminaMeter.gameObject.SetActive(staminaMeterFlash);
            staminaMeter.color = staminaMeter.exhaustedColor;
            maxSpeed = exhaustedSpeed;
        }
        else
        {
            staminaMeterFlash = false;
            staminaMeterFlashTimer = 0;
            staminaMeter.gameObject.SetActive(true);
            staminaMeter.color = staminaMeter.staminaMeterColor;
        }

        staminaMeter.value = Mathf.Clamp01(currentStamina / maximumStamina);

        if (move.magnitude > 1)
            move = Vector3.ClampMagnitude(move, 1);

        // Move the camera
        currentLookY = Mathf.Clamp(currentLookY - lookY, -90, 90);
        cameraHolder.localRotation = Quaternion.Euler(currentLookY, 0, 0);
        gameObject.transform.Rotate(new Vector3(0, lookX, 0));

        // Handle camera bobbing
        if (characterController.velocity.magnitude > 0.5f && characterController.isGrounded)
        {
            headBobTimer += Time.deltaTime * (maxSpeed * 10);
            _camera.localPosition = new Vector3(Mathf.Lerp(_camera.localPosition.x, Mathf.Cos(headBobTimer / 2) * 0.04f, Time.deltaTime * 6.0f), Mathf.Lerp(_camera.localPosition.y, Mathf.Sin(headBobTimer) * 0.02f, Time.deltaTime * 6.0f), 0);
        }
        else
        {
            headBobTimer = 0;
            _camera.localPosition = new Vector3(Mathf.Lerp(_camera.localPosition.x, 0, Time.deltaTime * 6.0f), Mathf.Lerp(_camera.localPosition.y, 0, Time.deltaTime * 6.0f), 0);
        }

        _camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_camera.GetComponent<Camera>().fieldOfView, desiredFOV, Time.deltaTime * 8f);

        /*if (Input.GetButtonDown("Jump"))
        {
            velocity.y = 2f;
        }*/

        if (!characterController.isGrounded)
            velocity.y += -6f * Time.deltaTime;
        else if (velocity.y <= 0)
            velocity.y = -2f;

        velocity.x = 0;
        velocity.z = 0;

        characterController.Move(((move * maxSpeed) + velocity) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerPosition();
        }

        if (flashlight.GetComponent<Light>().intensity > 0)
        {
            flashlight.GetChild(0).gameObject.SetActive(true);
            if (flashlightPower.flashlightPower > 0)
                flashlightPower.flashlightPower -= Time.deltaTime;
            if (flashlightPower.flashlightPower <= 0)
            {
                flashlight.GetComponent<Light>().intensity = 0;
                flashlight.GetComponent<AudioSource>().Play();
                flashlightPower.flashlightPower = 0;
            }
        }
        else
        {
            flashlight.GetChild(0).gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ToggleFlashlight();
        }
    }

    private void Update()
    {
        if (dead)
        {
            _camera.localRotation = Quaternion.identity;
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, Vector3.zero, 5.0f * Time.deltaTime);
            flashlight.GetComponent<Light>().intensity = 0;
        }
        else
        {
            PlayerMovement();
        }

        flashlight.transform.position = cameraHolder.transform.position;
        flashlight.transform.rotation = Quaternion.Lerp(flashlight.transform.rotation, cameraHolder.transform.rotation, 30.0f * Time.deltaTime);
    }

    private void ToggleFlashlight()
    {
        if (flashlightPower.flashlightPower <= 0)
        {
            return;
        }
        var light = flashlight.GetComponent<Light>();
        if (light.intensity == flashlightIntensity)
        {
            light.intensity = 0;
        }
        else
        {
            light.intensity = flashlightIntensity;
        }
        flashlight.GetComponent<AudioSource>().Play();
    }

    public void ResetPlayerPosition()
    {
        velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }
}