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
        LinkTimer();
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

    private void LinkTimer()
    {
        if (currentLink > lastLink)
        {
            linkTimer = 7;
            lastLink = currentLink;
        }

        if (linkTimer > 0)
        {
            linkTimer -= Time.deltaTime * (currentLink * 0.05f + 1);
        }

        if (linkTimer < 0)
        {
            linkTimer = 0;
            currentLink = 0;
            lastLink = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Link")
        {
            currentLink++;
            IncreaseGauge(0.1f, true);
            Debug.Log("Picked Up Link Object");
            Destroy(other.gameObject);
        }
        if (other.tag == "Chip")
        {
            currentLink++;
            IncreaseGauge(0.2f, true);
            chipCount++;
            Debug.Log("Picked Up Chip");
            Destroy(other.gameObject);
        }
    }
}
