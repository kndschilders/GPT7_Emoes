using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    private Transform player;
    private Material icon;
    private MeshRenderer renderer;

    private Color color;

    public float FadeInDistance = 0.5f;
    public float VisibleDistance = 3f;
    
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        icon = GetComponent<Renderer>().material;
        renderer = GetComponent<MeshRenderer>();
        color = icon.color;
        renderer.enabled = false;
	}
	void Update () {
        transform.Rotate(new Vector3(0, 40 * Time.deltaTime, 0));

        float _distance = Vector3.Distance(transform.position, player.position);

        if(_distance < VisibleDistance)
        {
            renderer.enabled = true;
            //float _FadedInDistance = VisibleDistance - FadeInDistance;
            //_distance -= _FadedInDistance;

            //color.a = Mathf.Clamp(1 - _distance / FadeInDistance, 0f, 1f);
            //icon.color = color;
        } else
        {
            renderer.enabled = false;
            //color.a = 0;
            //icon.color = color;
        }
	}
}
