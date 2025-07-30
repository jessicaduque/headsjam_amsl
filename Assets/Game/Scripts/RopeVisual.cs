using UnityEngine;

public class RopeVisual : MonoBehaviour
{
    [SerializeField] Transform playerOne;
    [SerializeField] Transform playerTwo;
    public int points = 20;
    public float maxSag = 1f;
    public float maxRopeDistance = 4f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = points;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;

        line.textureMode = LineTextureMode.Tile;
    }

    void Update()
    {
        float distance = Vector3.Distance(playerOne.position, playerTwo.position);
        float t = Mathf.Clamp01(1f - (distance / maxRopeDistance));
        float sagAmount = maxSag * t;
        float verticalDrop = sagAmount * 0.5f;

        for (int i = 0; i < points; i++)
        {
            float step = i / (float)(points - 1);
            Vector3 pos = Vector3.Lerp(playerOne.position, playerTwo.position, step);
            float curve = Mathf.Sin(step * Mathf.PI);
            pos.y -= curve * sagAmount + verticalDrop;
            line.SetPosition(i, pos);
        }

        line.material.mainTextureScale = new Vector2(distance * 5f, 1);
    }
}
