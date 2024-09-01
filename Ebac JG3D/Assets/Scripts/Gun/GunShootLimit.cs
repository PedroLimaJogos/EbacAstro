using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GunShootLimit : GunBase
{
    public List<UIFillUpdate> uiGunUpdaters;
    public float maxShoot = 5f;
    public float timeToRecharge = 1f;

    private float _currentShoots;
    private bool _recharging = false;
    
    private void Awake() {
        GetAllUIS();
    }

    protected override IEnumerator ShootCoroutine(){

        if(_recharging) yield break;

        while(true){
            if(_currentShoots < maxShoot){
                Shoot();
                _currentShoots++;
                CheckRecharge();
                UpdateUI();
                yield return new WaitForSeconds(timeBetweenShoot);
            }
        }
    }

    private void CheckRecharge(){
        if(_currentShoots >=  maxShoot){
            StopShoot();
            StartRecharge();
        }
    }

    private void StartRecharge()
    {
        _recharging = true;
        StartCoroutine(rechargeCoroutine());
    }

    IEnumerator rechargeCoroutine()
    {
        float time = 0;
        while(time < timeToRecharge)
        {
            time += Time.deltaTime;
            uiGunUpdaters.ForEach(i => i.UpdateValue(time/timeToRecharge));
            yield return new WaitForEndOfFrame();
        }
        _currentShoots = 0;
        _recharging = false;
    }

    private void UpdateUI()
    {
        uiGunUpdaters.ForEach(i => i.UpdateValue(maxShoot,_currentShoots));
    }

    private void GetAllUIS()
    {
        uiGunUpdaters = GameObject.FindObjectsOfType<UIFillUpdate>().ToList();
    }
}