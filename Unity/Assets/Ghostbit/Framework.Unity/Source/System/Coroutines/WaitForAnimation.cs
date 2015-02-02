using UnityEngine;
using System.Collections;
using System;

namespace Ghostbit.Framework.Unity.System
{
    public class WaitForAnimation : CoroutineReturn
    {
        private GameObject _go;
        private string _name;
        private float _time;
        private float _weight;
        //private bool first = true;
        private int startFrame;
        public string name
        {
            get
            {
                return _name;
            }
        }

        public WaitForAnimation(GameObject go, string name, float time = 1f, float weight = -1)
        {
            startFrame = Time.frameCount;
            _go = go;
            _name = name;
            _time = Mathf.Clamp01(time);
            _weight = weight;
        }

        public override bool finished
        {
            get
            {
                if (Time.frameCount <= startFrame + 1)
                {
                    //first = false;
                    return false;
                }
                if (_weight == -1)
                {
                    return !_go.animation[_name].enabled || _go.animation[_name].normalizedTime >= _time || _go.animation[_name].weight == 0 || _go.animation[_name].speed == 0;
                }
                else
                {
                    if (_weight < 0.5)
                    {
                        //var w = _go.animation[_name].weight;
                        return _go.animation[_name].weight <= Mathf.Clamp01(_weight);
                    }
                    return _go.animation[_name].weight >= Mathf.Clamp01(_weight);
                }
            }
            set
            {
                base.finished = value;
            }
        }

    }
}