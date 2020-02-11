﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool close_after_time;
    public float close_delay;
    float close_timer = 0;

    Animator door_animator;

    private void Awake()
    {
        door_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (close_after_time && door_animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            close_timer += Time.deltaTime;

            if (close_timer >= close_delay)
            {
                Close();
                close_timer = 0;
            }
        }
    }

    public void Open()
    {
        door_animator.SetTrigger("open");
    }

    public void Close()
    {
        door_animator.SetTrigger("close");
    }
}