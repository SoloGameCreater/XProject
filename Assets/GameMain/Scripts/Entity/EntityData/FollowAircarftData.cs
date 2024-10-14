using System;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class FollowAircarftData : AircraftData
    {
        public FollowAircarftData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {
        }
    }
}
