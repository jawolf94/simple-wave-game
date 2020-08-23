
/// <summary>
/// Class representing destroyable barricades
/// Note: Not currently used - may be re-introduced.
/// </summary>
public class Wall : Shootable
{
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Action prefromed when Update() is called.
    /// </summary>
    public override void UpdateBehavior() {
        return;
    }

    /// <summary>
    /// Action prefomred when Destory() is called.
    /// </summary>
    public override void DestroyShootable()
    {
        base.DestroyShootable();
        Destroy(this.gameObject);
    }


}
