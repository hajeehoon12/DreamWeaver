using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UpdateColliderWithSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Collider 초기화
        UpdateCollider();
    }

    void Update()
    {
        // 필요에 따라 Sprite가 변경될 때 Collider를 업데이트합니다.
        if (spriteRenderer.sprite != null)
        {
            UpdateCollider();
        }
    }

    void UpdateCollider()
    {
        // Sprite의 vertices를 가져와서 Collider에 설정합니다.
        Vector2[] spriteVertices = spriteRenderer.sprite.vertices;

        // Polygon Collider 2D의 Path를 초기화합니다.
        polygonCollider.pathCount = 1;

        // Sprite의 vertices를 Collider에 적용합니다.
        Vector2[] colliderPath = new Vector2[spriteVertices.Length];
        for (int i = 0; i < spriteVertices.Length; i++)
        {
            colliderPath[i] = spriteRenderer.transform.TransformPoint(spriteVertices[i]);
        }

        polygonCollider.SetPath(0, spriteVertices);
    }
}