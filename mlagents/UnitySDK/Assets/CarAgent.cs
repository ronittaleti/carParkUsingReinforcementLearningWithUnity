using System;
using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;
using UnityEngine.UI;

public class CarAgent : Agent
{

    Rigidbody rBody;

    float closestDistance = 10000000;

    Quaternion beginRotation;
    Vector3 beginPosition;

    bool rayforward = false;
    bool rayforwardleft = false;
    bool rayforwardright = false;


    bool rayleft = false;
    bool rayright = false;

    bool rayback = false;
    bool raybackleft = false;
    bool raybackright = false;

    public RaycastHit hit1;
    public RaycastHit hit2;
    public RaycastHit hit3;
    public RaycastHit hit4;
    public RaycastHit hit5;
    public RaycastHit hit6;
    public RaycastHit hit7;
    public RaycastHit hit8;

    public Text generationText;
    public Text succeededText;
    public Text failedText;

    int generation = 1928;
    int failed = 1781;
    int succeeded = 147;

    bool didSucceed = false;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        beginRotation = this.transform.rotation;
        beginPosition = this.transform.position;
    }

    public Transform Target;

    public override void AgentReset()
    {

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.position = beginPosition;
        this.transform.rotation = beginRotation;

        // Move the target to a new spot
        /*Target.position = new Vector3(UnityEngine.Random.value * 12 - 4,
                                      .51f,
                                      UnityEngine.Random.value * 12 - 4);
                                      */
        // Make sure to reset if the car is inside the parking area
        CheckIfInside.isInside = false;
        CheckIfInside.isAlmostInside = false;
        Collision.touching = false;
        CarCollision.touching = false;

        closestDistance = 10000000;

        generation++;
        generationText.text = "Generation: " + generation;

        if (!didSucceed)
        {

            failed++;
            failedText.text = "Failed: " + failed;
        }
        else
        {

            succeeded++;
            succeededText.text = "Succeeded: " + succeeded;
        }

        Debug.Log("Generation: " + generation + ". Failed: " + failed + ". Succeeded: " + succeeded);
    }

    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(Target.position);
        AddVectorObs(Target.rotation);
        AddVectorObs(this.transform.position);
        AddVectorObs(this.transform.rotation);

        // Raycast Bools
        AddVectorObs(rayforward);
        AddVectorObs(rayforwardleft);
        AddVectorObs(rayforwardright);
        AddVectorObs(rayleft);
        AddVectorObs(rayright);
        AddVectorObs(raybackleft);
        AddVectorObs(raybackright);
        AddVectorObs(rayback);

        // Raycast
        AddVectorObs(hit1.distance);
        AddVectorObs(hit2.distance);
        AddVectorObs(hit3.distance);
        AddVectorObs(hit4.distance);
        AddVectorObs(hit5.distance);
        AddVectorObs(hit6.distance);
        AddVectorObs(hit7.distance);
        AddVectorObs(hit8.distance);


        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }

    public float speed = 10;
    public float turnSpeed = 5;
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        Vector3 turnSignal = Vector3.zero;
        controlSignal.z = vectorAction[1];
        turnSignal.x = vectorAction[0];



        rBody.AddRelativeForce(controlSignal * speed);

        transform.Rotate(Vector3.up, turnSignal.x * turnSpeed * Time.deltaTime);

        hit1 = new RaycastHit();
        Ray forwardRay = new Ray(transform.position, transform.forward);

        hit2 = new RaycastHit();
        Quaternion forwardLeftAngle = Quaternion.AngleAxis(-45, new Vector3(0, 1, 0));
        Ray forwardLeftRay = new Ray(transform.position, forwardLeftAngle * transform.forward);

        hit3 = new RaycastHit();
        Quaternion forwardRightAngle = Quaternion.AngleAxis(45, new Vector3(0, 1, 0));
        Ray forwardRightRay = new Ray(transform.position, forwardRightAngle * transform.forward);

        hit4 = new RaycastHit();
        Quaternion leftAngle = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));
        Ray leftRay = new Ray(transform.position, leftAngle * transform.forward);

        hit5 = new RaycastHit();
        Quaternion rightAngle = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
        Ray rightRay = new Ray(transform.position, rightAngle * transform.forward);

        hit6 = new RaycastHit();
        Quaternion backLeftAngle = Quaternion.AngleAxis(-135, new Vector3(0, 1, 0));
        Ray backLeftRay = new Ray(transform.position, backLeftAngle * transform.forward);

        hit7 = new RaycastHit();
        Quaternion backRightAngle = Quaternion.AngleAxis(135, new Vector3(0, 1, 0));
        Ray backRightRay = new Ray(transform.position, backRightAngle * transform.forward);

        hit8 = new RaycastHit();
        Quaternion backAngle = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
        Ray backRay = new Ray(transform.position, backAngle * transform.forward);

        int layerMask = 1 << 14;
        layerMask = ~layerMask;

        if (Physics.Raycast(forwardRay, out hit1, 5f, layerMask)) {

            Debug.DrawRay(forwardRay.origin, forwardRay.direction * hit1.distance, Color.red);
            rayforward = true;
        } else
        {
            Debug.DrawRay(forwardRay.origin, forwardRay.direction * 5f, Color.green);
            rayforward = false;
        }


        if (Physics.Raycast(forwardLeftRay, out hit2, 5f, layerMask))
        {

            Debug.DrawRay(forwardLeftRay.origin, forwardLeftRay.direction * hit2.distance, Color.red);
            rayforwardleft = true;
        }
        else
        {
            Debug.DrawRay(forwardLeftRay.origin, forwardLeftRay.direction * 5f, Color.green);
            rayforwardleft = false;
        }


        if (Physics.Raycast(forwardRightRay, out hit3, 5f, layerMask))
        {

            Debug.DrawRay(forwardRightRay.origin, forwardRightRay.direction * hit3.distance, Color.red);
            rayforwardright = true;
        }
        else
        {
            Debug.DrawRay(forwardRightRay.origin, forwardRightRay.direction * 5f, Color.green);
            rayforwardright = false;
        }


        if (Physics.Raycast(leftRay, out hit4, 5f, layerMask))
        {

            Debug.DrawRay(leftRay.origin, leftRay.direction * hit4.distance, Color.red);
            rayleft = true;
        }
        else
        {
            Debug.DrawRay(leftRay.origin, leftRay.direction * 5f, Color.green);
            rayleft = false;
        }


        if (Physics.Raycast(rightRay, out hit5, 5f, layerMask))
        {

            Debug.DrawRay(rightRay.origin, rightRay.direction * hit5.distance, Color.red);
            rayright = true;
        }
        else
        {
            Debug.DrawRay(rightRay.origin, rightRay.direction * 5f, Color.green);
            rayright = false;
        }


        if (Physics.Raycast(backLeftRay, out hit6, 5f, layerMask))
        {

            Debug.DrawRay(backLeftRay.origin, backLeftRay.direction * hit6.distance, Color.red);
            raybackright = true;
        }
        else
        {
            Debug.DrawRay(backLeftRay.origin, backLeftRay.direction * 5f, Color.green);
            raybackright = false;
        }


        if (Physics.Raycast(backRightRay, out hit7, 5f, layerMask))
        {

            Debug.DrawRay(backRightRay.origin, backRightRay.direction * hit7.distance, Color.red);
            raybackleft = true;
        }
        else
        {
            Debug.DrawRay(backRightRay.origin, backRightRay.direction * 5f, Color.green);
            raybackleft = false;
        }


        if (Physics.Raycast(backRay, out hit8, 5f, layerMask))
        {

            Debug.DrawRay(backRay.origin, backRay.direction * hit8.distance, Color.red);
            rayback = true;
        }
        else
        {
            Debug.DrawRay(backRay.origin, backRay.direction * 5f, Color.green);
            rayback = false;
        }


        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position, Target.position);

        if (distanceToTarget < closestDistance)
        {
            closestDistance = distanceToTarget;
            SetReward(1f);
        }

        // Reached target
        if (CheckIfInside.isInside)
        {
            SetReward(5.0f);
            didSucceed = true;
            Done();
        }

        if (Collision.touching)
        {
            SetReward(-1f);
            Debug.Log("touched");
            didSucceed = false;
            Done();
        }

        if (CarCollision.touching)
        {
            SetReward(-.1f);
        }

        if (CheckIfInside.isAlmostInside)
        {
            SetReward(3f);
        }

        // Fell off platform
        if (this.transform.position.y < 0)
        {
            SetReward(-1f);
            Done();
        }

    }
}
