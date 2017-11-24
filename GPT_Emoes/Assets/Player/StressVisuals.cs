using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class StressVisuals : MonoBehaviour {

    PostProcessingProfile m_Profile;

    Camera cam;

    Material fogMat;

    public FloatVariable stressLevel;
	// Use this for initialization

	void Start () {
        cam = Camera.main;
        m_Profile = cam.GetComponent<PostProcessingBehaviour>().profile;
        fogMat = cam.GetComponent<RenderWithShader>().material;
	}
	
	// Update is called once per frame
	void Update () {
        ChromaticAberrationModel.Settings chromaticAberration = m_Profile.chromaticAberration.settings;
        chromaticAberration.intensity = stressLevel.Value*1.5f;
        m_Profile.chromaticAberration.settings = chromaticAberration;
        fogMat.SetFloat("_depth", stressLevel.Value);
    }
}
