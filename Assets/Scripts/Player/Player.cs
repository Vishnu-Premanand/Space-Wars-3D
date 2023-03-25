using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    private FlightControls flightControls;
    private Rigidbody playerRB;
    private float thrust;
    private float sideWay;
    private float roll;
    private Vector2 pitch;
    private float thrustReduction = .999f;
    private float glide = 0;
    private float sideWayGlide = 0;


    [SerializeField] private float pitchTorque;
    [SerializeField] private float thrustForce;
    [SerializeField] private float sidewayThrust;
    [SerializeField] private float rollTorque;
    
    #region Unity Functions
    private void OnEnable()
    {
        flightControls = new FlightControls();
        playerRB = GetComponent<Rigidbody>();
        flightControls.Enable();
    }
    private void OnDisable()
    {
        flightControls.Disable();
    }
    
    void Update()
    {
        ReadControls();
    }
    private void FixedUpdate()
    {
        MoveFlight();
    }
    #endregion


    #region Private Functions
    private void ReadControls()
    {
        thrust = flightControls.Flight.Thrust.ReadValue<float>();
        pitch = flightControls.Flight.Pitch.ReadValue<Vector2>();
        sideWay = flightControls.Flight.SideWays.ReadValue<float>();
        roll = flightControls.Flight.Roll.ReadValue<float>();
        
    }
    private void MoveFlight()
    {
        //Thrust Movement
        float currentThrust = thrustForce;
        if (thrust > 0.1f || thrust < -0.1f)
        {
            playerRB.AddRelativeForce(Vector3.forward * currentThrust * thrust * Time.deltaTime);
            glide = thrustForce;
        }
        else
        {
            
            playerRB.AddRelativeForce(Vector3.forward * glide * thrust * Time.deltaTime);
            glide *= thrustReduction;
            if (glide < 0.1f)
            {
                glide = 1;
            }
        }

        //SideWay Movement
        float currentSideWayThrust = sidewayThrust;
        if (sideWay > 0.1f || sideWay < -0.1f)
        {
            playerRB.AddRelativeForce(Vector3.right * currentSideWayThrust * sideWay * Time.deltaTime);
            sideWayGlide = sideWay*sidewayThrust;
        }
        else
        {

            playerRB.AddRelativeForce(Vector3.right * sideWayGlide * thrust * Time.deltaTime);
            sideWayGlide *= thrustReduction;
            if (sideWayGlide < 0.1f)
            {
                sideWayGlide = 1;
            }
        }

        //PitchYaw Movement
        playerRB.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitch.y, -1, 1) * pitchTorque * Time.deltaTime);

        //Roll Movement

        playerRB.AddRelativeTorque(Vector3.back * roll * rollTorque * Time.deltaTime);
      


    }
    #endregion
}
