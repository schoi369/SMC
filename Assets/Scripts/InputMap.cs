using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    UP, DOWN, LEFT, RIGHT, ATTACK, SLOW, LEAP
}
public class InputMap : Singleton<InputMap>
{
    IDictionary<Action, KeyCode> act = new Dictionary<Action, KeyCode>();
    void Start()
    {
        act.Add(Action.UP, KeyCode.UpArrow);
        act.Add(Action.DOWN, KeyCode.DownArrow);
        act.Add(Action.LEFT, KeyCode.LeftArrow);
        act.Add(Action.RIGHT, KeyCode.RightArrow);
        act.Add(Action.ATTACK, KeyCode.Z);
        act.Add(Action.SLOW, KeyCode.LeftControl);
        act.Add(Action.LEAP, KeyCode.LeftShift);
    }
    public bool GetInput(Action action)
    {
        return Input.GetKey(act[action]);
    }
    void UpdateKeyCode(Action action, KeyCode key)
    {
        act[action] = key;
    }
    bool GetInputUp(Action action)
    {
        return Input.GetKeyUp(act[action]);
    }
    bool GetInputDown(Action action)
    {
        return Input.GetKeyDown(act[action]);
    }
}
