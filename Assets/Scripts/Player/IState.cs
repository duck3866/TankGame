using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    public void OperateEnter(T sender);
    public void OperateUpdate(T sender);
    public void OperateExit(T sender);
}
