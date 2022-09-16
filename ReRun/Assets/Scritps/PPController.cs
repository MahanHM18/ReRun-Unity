using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPController : MonoBehaviour
{
    public static PPController Instance;

    public PostProcessProfile pp;

    private ColorGrading _colorGrading;
    private LensDistortion _lensDistortion;
    private ChromaticAberration _chromaticAberration;
    private Vignette _vignette;

    private float _targetLens;
    private float _targetColorGrading;
    private float _targetChromatic;
    private float _targetVignette;

    public float DamageTime;


    public float SoftTime;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        _colorGrading = pp.GetSetting<ColorGrading>();
        _chromaticAberration = pp.GetSetting<ChromaticAberration>();
        _lensDistortion = pp.GetSetting<LensDistortion>();
        _vignette = pp.GetSetting<Vignette>();
    }

    private void Start()
    {
        _lensDistortion.intensity.value = 0;
        _colorGrading.saturation.value = 10;
        _chromaticAberration.intensity.value = 0.2f;
        _targetVignette = 0;
    }

    void Update()
    {
        _lensDistortion.intensity.value =
            Mathf.Lerp((float)_lensDistortion.intensity.value, _targetLens, Time.deltaTime * SoftTime);
        
        _colorGrading.saturation.value =
            Mathf.Lerp(_colorGrading.saturation.value, _targetColorGrading, Time.deltaTime * SoftTime);
        
        _chromaticAberration.intensity.value =
            Mathf.Lerp(_chromaticAberration.intensity, _targetChromatic, Time.deltaTime * SoftTime);
        
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, _targetVignette, Time.deltaTime * SoftTime);
    }

    public void StartEffect()
    {
        _colorGrading.enabled.value = true;
        _lensDistortion.enabled.value = true;
        _chromaticAberration.enabled.value = true;

        _targetLens = -50;
        _targetChromatic = 0.7f;
        _targetColorGrading = -75;


        Debug.Log("StartAddingEffects");
    }

    public void EndEffect()
    {
        _targetLens = 0;
        _targetColorGrading = 10;
        _targetChromatic = 0.2f;
    }

    public IEnumerator ActiveDamagetEffect()
    {
        _targetVignette = 0.6f;
        yield return new WaitForSeconds(DamageTime);
        _targetVignette = 0;
    }
}