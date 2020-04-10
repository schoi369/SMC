using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    UP, DOWN, LEFT, RIGHT, ATTACK, SLOW, LEAP, THROW, STOMP
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
        act.Add(Action.THROW, KeyCode.X);
        act.Add(Action.STOMP, KeyCode.C);
    }
    public bool GetInput(Action action)
    {
        return Input.GetKey(act[action]);
    }
    void UpdateKeyCode(Action action, KeyCode key)
    {
        act[action] = key;
    }
    public bool GetInputUp(Action action)
    {
        return Input.GetKeyUp(act[action]);
    }
    public bool GetInputDown(Action action)
    {
        return Input.GetKeyDown(act[action]);
    }
}
