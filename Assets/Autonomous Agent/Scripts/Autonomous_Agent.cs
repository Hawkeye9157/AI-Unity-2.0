using UnityEngine;

public class Autonomous_Agent : AI_Agent
{
    [Header("Movement")]
    [SerializeField] Movement movement;

    [Header("Perception")]
    [SerializeField] Perception seekPerception;
    [SerializeField] Perception fleePerception;

    [Header("Wander")]
    [SerializeField] float wanderRadius = 1.0f;
    [SerializeField] float wanderDistance = 1.0f;
    [SerializeField] float wanderDisplacement = 1.0f;

    float wanderAngle = 0.0f;

    private void Start()
    {
        //random within circle degrees
        wanderAngle = Random.Range(0, 360);
    }
    private void Update()
    {
        bool hasTarget = false;
        if(seekPerception != null)
        {
            var seekGameObjects = seekPerception.GetGameObjects();
            foreach(var sgo in seekGameObjects)
            {
                Debug.DrawLine(transform.position, sgo.transform.position, Color.crimson);
            }
            if (seekGameObjects.Length > 0)
            {
                hasTarget = true;
                Vector3 force = Seek(seekGameObjects[0]);
                movement.ApplyForce(force);
            }
        }
        
        if(fleePerception != null)
        {
            var fleeGameObjects = fleePerception.GetGameObjects();
            foreach(var fgo in fleeGameObjects)
            {
                Debug.DrawLine(transform.position, fgo.transform.position, Color.mintCream);
            }
            if (fleeGameObjects.Length > 0)
            {
                hasTarget = true;
                Vector3 force = Flee(fleeGameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        if (!hasTarget)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }
        
        transform.position = Utilities.Wrap(transform.position, new Vector3(-15, 0, -15),
            new Vector3(15, 0, 15));
        if(movement.Velocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Velocity, Vector3.up);
        }
    }
    Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }
    Vector3 Flee(GameObject go)
    {
        Vector3 direction = transform.position - go.transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }
    Vector3 Wander()
    {
        wanderAngle += Random.Range(-wanderDisplacement, wanderDisplacement);
        Quaternion rotation = Quaternion.AngleAxis(wanderAngle, Vector3.up);
        Vector3 pointOnCircle = rotation * (Vector3.forward * wanderRadius);
        Vector3 circleCenter = movement.Direction * wanderDistance;
        Vector3 force = GetSteeringForce(circleCenter + pointOnCircle);

        Debug.DrawLine(transform.position, transform.position + circleCenter, Color.blue);
        Debug.DrawLine(transform.position, transform.position + circleCenter + pointOnCircle, Color.red);
        return force;
    }
    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);
        return force;
    }
}
