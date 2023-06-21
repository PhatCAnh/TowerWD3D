using CanasSource;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider sldHp;
    [SerializeField] Image imgHp;

    public Enemy view;
    private Camera _cam;

    public void Init(Enemy enemy) => view = enemy;

    private void Start()
    {
        _cam = Camera.main;
        transform.SetParent(Singleton<InGameController>.Instance.Parent_HealthBar.transform);
    }

    public void HpChanged()
    {
        var value = (float) view.model.CurrentHp / view.model.MaxHp;
        sldHp.value = value;
        sldHp.gameObject.SetActive(value != 0);
        gameObject.SetActive(view.isAlive);
    }

    private void LateUpdate()
    {
        if(view == null) return;
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        transform.position = view.healthBarPos.position;
    }
}