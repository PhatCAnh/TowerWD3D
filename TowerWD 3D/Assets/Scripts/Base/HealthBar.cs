using System;
using CanasSource;
using Cysharp.Threading.Tasks;
using Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider sldHp;
    [SerializeField] Image imgHp;

    public Enemy enemy;
    private Camera _cam;

    public void Init(Enemy enemyModel)
    {
        enemy = enemyModel;
        Singleton<Observer>.Instance.AddListenerDataChange(enemy.model.Id, HpChanged);
    }

    private void Start()
    {
        _cam = Camera.main;
        
    }

    public void HpChanged(DataEventChange param)
    {
        if (enemy.model.Id == param.ModleID && param.NameOfType == "CurrentHp")
        {
            var value = (float) Convert.ToInt32(param.Value)  / enemy.model.MaxHp;
            sldHp.value = value;
            sldHp.gameObject.SetActive(value != 0);
            gameObject.SetActive(enemy.isAlive);
        }
    }

    private void LateUpdate()
    {
        if(enemy == null) return;
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        transform.position = enemy.healthBarPos.position;
    }
}