using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerHealthManager : MonoBehaviour
    {
        [SerializeField] private Transform _healthStatsUI;
        [SerializeField] public PlayerHealthState _playerHealthState;

        //References to the progress bars for each health stat. 
        private ProgressBar _bloodProgessBar;
        private ProgressBar _hydrationProgessBar;
        private ProgressBar _carbohydratesProgressBar;
        private ProgressBar _proteinsProgressBar;
        private ProgressBar _mineralsProgressBar;
        private ProgressBar _vitaminsProgressBar;
        private ProgressBar _warmthProgressBar;


        private void Awake()
        {
            _GetUIReferences();
        }

        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
            _UpdateProgressBarsFromCurrentHealthStats();
            _ToggleHealthUI();          
        }


        private void _ToggleHealthUI()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                _healthStatsUI.gameObject.SetActive(!_healthStatsUI.gameObject.activeSelf);
            }
        }

        private void _GetUIReferences()
        {
            _healthStatsUI.gameObject.SetActive(true);
            _bloodProgessBar = GameObject.Find("BloodProgressBar").GetComponent<ProgressBar>();
            _hydrationProgessBar = GameObject.Find("HydrationProgressBar").GetComponent<ProgressBar>();
            _carbohydratesProgressBar = GameObject.Find("CarbohydratesProgressBar").GetComponent<ProgressBar>();
            _vitaminsProgressBar = GameObject.Find("VitaminsProgressBar").GetComponent<ProgressBar>();
            _mineralsProgressBar = GameObject.Find("MineralsProgressBar").GetComponent<ProgressBar>();
            _proteinsProgressBar = GameObject.Find("ProteinsProgressBar").GetComponent<ProgressBar>();
            _warmthProgressBar = GameObject.Find("WarmthProgressBar").GetComponent<ProgressBar>();
            _healthStatsUI.gameObject.SetActive(false);
        }


        private void _UpdateProgressBarsFromCurrentHealthStats()
        {
            _bloodProgessBar.UpdateValue(_playerHealthState._blood);
            _hydrationProgessBar.UpdateValue(_playerHealthState._hydration);
            _carbohydratesProgressBar.UpdateValue(_playerHealthState._carbohydrates);
            _proteinsProgressBar.UpdateValue(_playerHealthState._protiens);
            _mineralsProgressBar.UpdateValue(_playerHealthState._minerals);
            _vitaminsProgressBar.UpdateValue(_playerHealthState._vitamins);
            _warmthProgressBar.UpdateValue(_playerHealthState._warmth);
        }
    }
}
