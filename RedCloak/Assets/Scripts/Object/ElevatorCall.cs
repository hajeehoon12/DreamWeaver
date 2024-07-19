using System.Collections;
using UnityEngine;

public class ElevatorCall : MonoBehaviour, IDamage
{
    private float moveSpeed = 20f;
    [SerializeField] private Transform elevator;

    private Vector3 destination;

    private bool isMoving = false;

    public void GetDamage(float damage)
    {
        AudioManager.instance.PlayPitchSFX("ElevatorButton", 0.2f);
        destination = new Vector3(elevator.position.x, gameObject.transform.position.y, elevator.position.z);
        if (!isMoving)
        {
            StartCoroutine(MoveElevator());
        }
    }

    private IEnumerator MoveElevator()
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
