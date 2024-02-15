using Pong.Scripts;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public PowerUps powerUps;

    //
    void OnTriggerEnter(Collider other)
    {
        gameManager.OnGoalTrigger(this);
        powerUps.resetOnGoal();
        
    }
}
