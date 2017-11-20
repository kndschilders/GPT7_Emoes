using UnityEngine;

public class playerFollower : MonoBehaviour {

    public Transform player;
    private float heightDifference = 0;

    void Start()
    {
        heightDifference = transform.position.y - player.position.y;
    }

	void Update () {
        transform.position = player.position + Vector3.up * heightDifference;
	}
}
