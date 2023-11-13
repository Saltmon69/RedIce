using UnityEngine;

public abstract class BlueprintBaseState
{
    public abstract void EnterState(BlueprintStateMachineManager blueprint);

    public abstract void UpdateState(BlueprintStateMachineManager blueprint);

    public abstract void RayState(BlueprintStateMachineManager blueprint, Ray ray, float distance);

    public abstract void ExitState(BlueprintStateMachineManager blueprint);
}
