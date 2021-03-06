﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Move : MonoBehaviour {

    public CharacterController pc;

	public float speed;
	public float friction;

	private Vector2 velocity;

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public float getSpeed()
    {
        return this.speed;
    }

    // Use this for initialization
    void Start()
    {
        pc = GetComponent<CharacterController>();
    }

    //NOT FINAL
    // Update is called once per frame
    //Moves the player with the arrow keys and WASD keys
    //Player can choose either
    void Update()
    {
		float currentSpeed = speed;

		if (Input.GetKey(KeyCode.LeftShift))
			currentSpeed *= 3.0f;

		Vector2 accel = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		float len = accel.sqrMagnitude;

		if (len > 1.0f)
			accel *= (1.0f / Mathf.Sqrt(len));

		accel *= currentSpeed;
		accel += (velocity * friction);

		Vector2 delta = accel * 0.5f * Square(Time.deltaTime) + velocity * Time.deltaTime;
		velocity = accel * Time.deltaTime + velocity;

		pc.Move(delta);
    }
}



