using UnityEngine;

public class ChooseBuildTower : ChooseTower
{
    public string idTower;

    public Material orginMaterial;

    protected override void Start()
    {
        base.Start();
        orginMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }
    public override void OnClicked()
    {
        node.AppearTower(idTower, orginMaterial);
    }
}
