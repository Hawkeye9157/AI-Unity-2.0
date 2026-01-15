using UnityEngine;

public class Autonomous_Agent : AI_Agent
{
    [SerializeField] Movement movement;
    [SerializeField] Perception perception;

    private void Update()
    {
        movement.ApplyForce(Vector3.forward);
        movement.transform.position = Utilities.Wrap(transform.position, new Vector3(-15, -15, -15),
            new Vector3(15, 15, 15));
    }
}
