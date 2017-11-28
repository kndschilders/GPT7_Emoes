using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    private Transform player;
    private SpriteRenderer icon;

    private Color color;

    public float FadeInDistance = 0.5f;
    public float VisibleDistance = 3f;
    
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        icon = GetComponent<SpriteRenderer>();
        color = new Color(1, 1, 1, 1);
	}
	void Update () {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        float _distance = Vector3.Distance(transform.position, player.position);

        if(_distance < VisibleDistance)
        {
            float _FadedInDistance = VisibleDistance - FadeInDistance;
            _distance -= _FadedInDistance;

            color.a = Mathf.Clamp(1 - _distance / FadeInDistance, 0f, 1f);
            icon.color = color;
        } else
        {
            color.a = 0;
            icon.color = color;
        }
	}
}
