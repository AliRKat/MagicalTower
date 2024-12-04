using UnityEngine;

public class Tower : HealthSystem
{
    /// <summary>
    /// Overrides the base method to handle the tower's destruction.
    /// </summary>
    protected override void HandleHealthDepleted()
    {
        base.HandleHealthDepleted();
        Debug.Log("The Tower has been destroyed! Game Over!");

        // Notify the GameManager of the game-over state
        GameManager.Instance.GameOver();
    }
}