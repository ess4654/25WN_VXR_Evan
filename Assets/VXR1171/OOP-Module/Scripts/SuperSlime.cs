using UnityEngine;

/// <summary>
///     Super slime will spawn child slime enemies.
/// </summary>
public class SuperSlime : Slime
{
    [SerializeField] private float spawnTime = 5f;

    private float aliveTime = 0;
    private bool spawnedChild;

    protected override void Update()
    {
        base.Update();

        aliveTime += Time.deltaTime;
        if(!spawnedChild && aliveTime >= spawnTime)
            SpawnChild();
    }

    #region ENGINE

    protected override void WithinRange()
    {
        base.WithinRange();
        
        if(!spawnedChild)
            SpawnChild();
    }

    protected override void HandleDeath()
    {
        SpawnChild();
        base.HandleDeath();
    }

    #endregion

    /// <summary>
    ///     Spawns a child slime.
    /// </summary>
    private void SpawnChild()
    {
        spawnedChild = true;
        var newSpawn = Instantiate(this, transform.position, Quaternion.identity);
        newSpawn.spawnedChild = true; //prevents infinite spawn loop

        Debug.Log($"Spawned new Child Slime {newSpawn.transform.position}");
    }
}
