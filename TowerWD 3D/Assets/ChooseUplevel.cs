public class ChooseUplevel : ChooseTower
{
    protected override void Start()
    {
        base.Start();
    }

    public override void OnClicked()
    {
        node.LevelUp();
    }
}
