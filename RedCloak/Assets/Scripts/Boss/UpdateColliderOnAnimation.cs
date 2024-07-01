using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class UpdateColliderOnAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        UpdateColliderFromSprite();
    }

    // Animation 이벤트로 호출할 메서드
    public void UpdateColliderFromSprite()
    {
        Sprite sprite = spriteRenderer.sprite;

        if (sprite != null)
        {
            Vector2[] spriteVertices = sprite.vertices;
            Vector2[] worldVertices = new Vector2[spriteVertices.Length];

            // 스프라이트의 로컬 좌표를 월드 좌표로 변환
            for (int i = 0; i < spriteVertices.Length; i++)
            {
                worldVertices[i] = spriteRenderer.transform.TransformPoint(spriteVertices[i]);
            }

            // Polygon Collider 2D 업데이트
            polygonCollider.SetPath(0, spriteVertices); // worldVertices
        }
    }
}