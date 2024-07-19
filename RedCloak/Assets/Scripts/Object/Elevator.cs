using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private Transform elevator;
    [SerializeField] private Transform Floor1F;
    [SerializeField] private Transform Floor2F;

    public Vector3 destination;

    public bool isMoving = false;

    private Transform playerTransform;
    private float originalGravityScale;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER))
        {
            playerTransform = collision.collider.transform;

            if (playerTransform.parent != elevator)
            {
                playerTransform.SetParent(elevator);
            }
            
            if (!isMoving)
            {
                if (elevator.transform.position.y == Floor1F.position.y)
                {
                    destination = new Vector3(elevator.position.x, Floor2F.position.y, elevator.position.z);
                }
                else
                {
                    destination = new Vector3(elevator.position.x, Floor1F.position.y, elevator.position.z);
                }

                StartCoroutine(MoveElevator());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Define.PLAYER))
        {
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }

    public IEnumerator MoveElevator()
    {
        isMoving = true;

        while ((destination - elevator.position).sqrMagnitude > 0.01f)
        {
            elevator.position = Vector3.MoveTowards(elevator.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        elevator.position = destination;
        isMoving = false;
    }
}
