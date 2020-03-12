﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class MovingPlatform : MonoBehaviour
{
    MovingPlatformsController controller;
    GameObject player = null;

    bool active = false;
    float distance_travelled = 0;

    // Mechanical Move
    bool mechanically_moving = false;
    float start_dist = 0;

    private void Update()
    {
        if (active)
        {
            if (!controller.mechanicalMovement)
            {
                distance_travelled += controller.platformMoveSpeed * Time.deltaTime;
                transform.position = controller.spline.path.GetPointAtDistance(distance_travelled, controller.endInstruction);
            }

            Quaternion spline_rotation = controller.spline.path.GetRotationAtDistance(distance_travelled, controller.endInstruction);
            Vector3 new_rotation = Vector3.zero;

            if (controller.rotateXWithSpline)
            {
                new_rotation.x = spline_rotation.eulerAngles.x;
            }
            if (controller.rotateYWithSpline)
            {
                new_rotation.y = spline_rotation.eulerAngles.y;
            }
            if (controller.rotateZWithSpline)
            {
                new_rotation.z = spline_rotation.eulerAngles.z;
            }
            transform.rotation = Quaternion.Euler(new_rotation);

            if (distance_travelled >= controller.spline.path.length)
            {
                End();
            }
        }
    }

    public bool MechanicalMove()
    {
        if (mechanically_moving)
        {
            distance_travelled += controller.platformMoveSpeed * Time.deltaTime;
            transform.position = controller.spline.path.GetPointAtDistance(distance_travelled, controller.endInstruction);

            if (distance_travelled >= start_dist + controller.mechanicalDistance)
            {
                mechanically_moving = false;
                return true;
            }

            return false;
        }
        else
        {
            start_dist = distance_travelled;
            mechanically_moving = true;
            return false;
        }
    }

    public void StartPlatform(MovingPlatformsController platform_controller)
    {
        controller = platform_controller;
        transform.rotation = Quaternion.LookRotation((controller.spline.path.GetPoint(1) - transform.position).normalized);
        active = true;
    }

    void End()
    {
        if (controller.endInstruction == EndOfPathInstruction.Stop)
        {
            if (player != null)
            {
                player.transform.parent = null;
                player = null;
            }

            controller.RemovePlatform(this);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            player = other.gameObject;
            other.gameObject.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            player = null;
            other.gameObject.transform.parent = null;
        }
    }
}