public class ChooseSellTower : ChooseTower
{
    protected override void Start()
    {
        base.Start();
    }

    public override void OnClicked()
    {
        node.DestroyTower();
    }
}
