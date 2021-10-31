using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameUI
{
    public class PlayerHealthManager : MonoBehaviour
    {
        [SerializeField] private Transform _hydrationProgressBarUI;
        private ProgressBar _hydrationProgessBar;

        [SerializeField] [Range(0f,1f)] private float foo = 0;
        private void Awake()
        {
            _hydrationProgessBar = _hydrationProgressBarUI.GetComponent<ProgressBar>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _hydrationProgessBar.UpdateValue(foo);
        }
    }
}
