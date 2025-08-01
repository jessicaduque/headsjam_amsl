using DG.Tweening;
using UnityEngine;
using Utils.Singleton;

public class RopeVisual : Singleton<RopeVisual>
{
    [SerializeField] Transform playerOne;
    [SerializeField] Transform playerTwo;
    public int points = 20;
    public float maxSag = 1f;
    public float maxRopeDistance = 4f;

    private LineRenderer line;

    protected override void Awake()
    {
        base.Awake();
        
        line = GetComponent<LineRenderer>();
        line.positionCount = points;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;

        line.textureMode = LineTextureMode.Tile;
    }

    void Update()
    {
        float distance = Vector3.Distance(playerOne.localPosition, playerTwo.localPosition);
        float t = Mathf.Clamp01(1f - (distance / maxRopeDistance));
        float sagAmount = maxSag * t;
        float verticalDrop = sagAmount * 0.5f;

        for (int i = 0; i < points; i++)
        {
            float step = i / (float)(points - 1);
            Vector3 pos = Vector3.Lerp(playerOne.localPosition, playerTwo.localPosition, step);
            float curve = Mathf.Sin(step * Mathf.PI);
            pos.y -= curve * sagAmount + verticalDrop;
            line.SetPosition(i, pos);
        }

        line.material.mainTextureScale = new Vector2(distance * 5f, 1);
    }

    public void RopeFadeOut(float time)
    {
        line.DOColor(new Color2(Color.white, Color.white), new Color2(Color.clear, Color.clear), time);
    }
}
