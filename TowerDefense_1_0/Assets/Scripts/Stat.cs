using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class Stat
{
    /// <summary>
    /// The max value of the stat
    /// </summary>
    
    private float maxVal;

    /// <summary>
    /// The current value of the stat
    /// </summary>
    [SerializeField]
    private float currentVal;

    /// <summary>
    /// A Property for accessing and setting the current value
    /// </summary>
    public float CurrentValue
    {
        get
        {
            return currentVal;
        }
        set
        {
            //Clamps the current value between 0 and max
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);


        }
    }

    /// <summary>
    /// A proprty for accessing the max value
    /// </summary>
    public float MaxVal
    {
        get
        {
            return maxVal;
        }
        set
        {

            this.maxVal = value;
        }
    }



    /// <summary>
    /// Initializes the stat
    /// This function needs to be called in awake
    /// </summary>
    public void Initialize()
    {
        
        this.MaxVal = maxVal;
        this.CurrentValue = currentVal;
    }
}

