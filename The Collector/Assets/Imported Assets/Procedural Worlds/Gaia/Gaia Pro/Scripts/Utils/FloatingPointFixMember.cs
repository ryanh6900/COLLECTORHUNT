﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GAIA_PRO_PRESENT
namespace Gaia
{
 
    public class FloatingPointFixMember : MonoBehaviour
    {

        protected void OnEnable()
        {
            FloatingPointFix.Instance.AddMember(this);
        }

        protected void OnDestroy()
        {
            if (FloatingPointFix.IsActive)
            {
                FloatingPointFix.Instance.RemoveMember(this);
            }
        }
    }

}
#endif