using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class UpdateColliderOnAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    private Sprite saveSprite;

    private bool isUpdating = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (isUpdating) UpdateCollision();
    }

    public void UpdateStart()
    {
        isUpdating = true;
    }

    public void UpdateStop()
    {
        isUpdating = false;
    }



    // Animation 이벤트로 호출할 메서드
    public void UpdateCollision()
    {
        //Sprite sprite = spriteRenderer.sprite;

        //if (sprite != null)
        //{
         //   Vector2[] spriteVertices = sprite.vertices;
         //   polygonCollider.SetPath(0, spriteVertices); // worldVertices
        //}
    }

    public void SaveCollision()
    {
        //saveSprite = spriteRenderer.sprite;

        //if (saveSprite != null)
        //{
        //    Vector2[] spriteVertices = saveSprite.vertices;
        //    polygonCollider.SetPath(0, spriteVertices); // worldVertices
        //}
    }

    public void LoadCollision()
    {
        //if (saveSprite != null)
        //{
        //    Vector2[] spriteVertices = saveSprite.vertices;
        //    polygonCollider.SetPath(0, spriteVertices); // worldVertices
        //}
    }




}