using UnityEngine;

public class ElevatorCall : MonoBehaviour, IDamage
{
    private Vector3 destination;

    [SerializeField] private Elevator elevatorStatus;

    public void GetDamage(float damage)
    {
        AudioManager.instance.PlayPitchSFX("ElevatorButton", 0.2f);
        destination = new Vector3(elevatorStatus.transform.position.x, gameObject.transform.position.y,elevatorStatus.transform.position.z);
        if (!elevatorStatus.isMoving)
        {
            elevatorStatus.destination = destination;
            StartCoroutine(elevatorStatus.MoveElevator());
        }
    }
}
