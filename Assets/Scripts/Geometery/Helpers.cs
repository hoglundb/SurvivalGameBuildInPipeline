using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geometery 
{
    public static class Helpers
    {
        public static float GetDistanceSquared(Vector3 pointA, Vector3 pointB)
        {
            return  (pointA.x - pointB.x) * (pointA.x - pointB.x) + 
                    (pointA.y - pointB.y) * (pointA.y - pointB.y) + 
                    (pointA.z - pointB.z) * (pointA.z - pointB.z);            
        }

    }

}
