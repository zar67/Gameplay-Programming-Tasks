﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : Switch
{
    public override bool Triggered()
    {
        return target.GetComponent<MovingPlatformsController>().triggered;
    }

    public override bool ReleaseButton()
    {
        return GetComponent<Cutscene>().state == Cutscene.CutsceneState.NONE && clicked && !target.GetComponent<MovingPlatformsController>().triggered;
    }
}
