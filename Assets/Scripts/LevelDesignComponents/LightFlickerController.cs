using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerController : MonoBehaviour
{
    [SerializeField] private new Light light;
    [Header("Min Maxes")]
    [Range(0f, 8f), Tooltip("Must not exceed 8")]
    [SerializeField] private float maxIntensity = 2.5f;
    [Range(0f, 8f), Tooltip("Must not exceed 8, Should be larger than max")]
    [SerializeField] private float minIntensity = 1.5f;
    [Range(0.1f, 2.5f)]
    [SerializeField] private float maxSpeed = 2f;
    [Range(0.1f, 2.5f)]
    [SerializeField] private float minSpeed = 1f;

    [Header("Functionality")]
    [SerializeField] private bool activeOnStart = true;
    [SerializeField] private bool fluctuateSpeed = true;
    [Header("Speed and time randomization")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float flucationTime = 2f;
    [SerializeField] private float flucationIntensity = 0.25f;


    [SerializeField] private float minHoldTime = 0.5f;


    private bool currentlyActive;

    private Coroutine enumerator;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (light == null)
            light = GetComponent<Light>();
        if (light == null)
            light = GetComponentInChildren<Light>();
        if (light == null)
            Debug.LogError("Light is null! Please add a light as a component to this object or child component", gameObject);
#endif
        if (fluctuateSpeed)
            StartCoroutine(FlucuateSpeed());
    }

    private void Start()
    {
        light.enabled = activeOnStart;
        currentlyActive = activeOnStart;
        StartCoroutine(ControlLight());
    }

    private IEnumerator ControlLight()
    {
        while (currentlyActive)
        {
            light.intensity = Mathf.PingPong(Time.time * speed, maxIntensity);
            if (light.intensity < minIntensity)
                light.intensity = minIntensity;
            yield return null;
        }
    }

    private IEnumerator FlucuateSpeed()
    {
        while (isActiveAndEnabled)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            yield return new WaitForSeconds(Random.Range(flucationTime - flucationIntensity, flucationTime + flucationIntensity));
            var hold = new WaitForSeconds(minHoldTime);
        }
    }

    public void SetLightActive()
    {
        currentlyActive = true;
        light.enabled = true;
        StopCoroutine(enumerator); // stop it in case it exists
        enumerator = StartCoroutine(ControlLight());
    }
    public void SetLightInactive()
    {
        currentlyActive = false;
        light.enabled = false;
        StopCoroutine(enumerator);
    }
}
