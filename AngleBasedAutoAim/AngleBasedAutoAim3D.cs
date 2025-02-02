﻿using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace TopDownEngineExtensions
{
    public class AngleBasedAutoAim3D : WeaponAutoAim3D
    {
        private Transform[] _unobstructedTargets;

        protected override void Initialization()
        {
            base.Initialization();
            _unobstructedTargets = new Transform[_hit.Length];
        }

        protected override bool ScanForTargets()
        {
            Target = null;
            var numberOfHits = Physics.OverlapSphereNonAlloc(Origin, ScanRadius, _hit, TargetsMask);
            
            if (numberOfHits == 0) return false;
            var numberOfUnobstructedTargets = 0;
            for (var i = 0; i < numberOfHits; i++)
            {
                _raycastDirection = _hit[i].transform.position - _raycastOrigin;
                var hit = MMDebug.Raycast3D(_raycastOrigin, _raycastDirection, Vector3.Distance(_hit[i].transform.position, _raycastOrigin), ObstacleMask.value, Color.yellow, true);
                if (hit.collider != null) continue;
                _unobstructedTargets[numberOfUnobstructedTargets] = _hit[i].transform;
                numberOfUnobstructedTargets++;
            }

            if (numberOfUnobstructedTargets == 0) return false;
            var smallestAngle = 180f;
            Target = _unobstructedTargets[0];
            for (var i = 0; i < numberOfUnobstructedTargets; i++)
            {
                var angleToTarget = Vector3.Angle(_topDownController3D.CurrentDirection, _unobstructedTargets[i].position - _topDownController3D.transform.position);
                if (angleToTarget > smallestAngle) continue;
                smallestAngle = angleToTarget;
                Target = _unobstructedTargets[i];
            }

            return true;
        }
    }
}
