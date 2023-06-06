using CanasSource;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider sldHp;
    [SerializeField] Image imgHp;

    public Enemy view;

    public void Init(Enemy enemy) => view = enemy;

    private void Awake()
    {
        transform.SetParent(Singleton<GameController>.Instance.Parent_HealthBar.transform);
    }

    public void HpChanged()
    {
        var value = (float) view.stat.currentHP.Value / view.stat.maxHP.Value;
        sldHp.value = value;
        sldHp.gameObject.SetActive(value != 0);
        gameObject.SetActive(view.isAlive);
    }

    private void LateUpdate()
    {
        if(view == null) return;
        transform.position = Camera.main.WorldToScreenPoint(view.transform.position);
    }
}
