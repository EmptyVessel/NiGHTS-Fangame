using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public FlightMovement movement;
    public float drillGauge;
    public float drillGaugeMax = 50;
    public bool canDash;
    public float currentLink;
    public float lastLink;
    public float linkTimer;

    public float chipCount;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<FlightMovement>();
        drillGauge = drillGaugeMax / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Corrections();
        DashGauge();
    }

    private void DashGauge()
    {
        if (drillGauge > 0)
        {
            canDash = true;
        }
        else canDash = false;

        if (movement.drillDash && canDash)
        {
            drillGauge -= Time.deltaTime * 4;
        }
        else if (drillGauge < drillGaugeMax && !movement.drillDash) IncreaseGauge(0.01f, false);
    }

    private void Corrections()
    {
        if (drillGauge < 0)
        {
            drillGauge = 0;
        }
        if (drillGauge > drillGaugeMax)
        {
            drillGauge = drillGaugeMax;
        }

        if (chipCount > 99)
        {
            chipCount = 99;
        }
        if (chipCount < 0)
        {
            chipCount = 0;
        }

        if (linkTimer < 0 && currentLink == lastLink)
        {
            linkTimer = 0;
            currentLink = 0;
        }
    }
    private void IncreaseGauge(float add, bool link)
    {
        if (link)
        {
            drillGauge += add * currentLink;
        }
        else drillGauge += add;
    }
}
