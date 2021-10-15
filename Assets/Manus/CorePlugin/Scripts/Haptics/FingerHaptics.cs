﻿using Manus.Interaction;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manus.Haptics
{
	/// <summary>
	/// This is the class which needs to be on a finger of a hand, it automatically creates Joint Haptics on children with colliders.
	/// Each of the colliders attributes to a percentage of the haptic strength.
	/// If all colliders are colliding with an object the maximum amount of haptic is triggered.
	/// For example if 3 Colliders exist on the finger, and only 2 of these are touching a collider, the haptic value will be 0.666f.
	/// </summary>
	[DisallowMultipleComponent]
	[AddComponentMenu("Manus/Haptics/Finger (Haptics)")]
	public class FingerHaptics : MonoBehaviour
	{
		public Utility.FingerType fingerType = Utility.FingerType.Invalid;

		private Dictionary<Collider, CollisionInfo> m_Collisions;

		private Collider[] m_OwnColliders;

		public LayerMask layerMask = ~0;
		[Range(0.1f, 10f)] public float collisionRange = .8f;

		private void Start()
		{
			m_Collisions = new Dictionary<Collider, CollisionInfo>();

			HandHaptics t_ParentObject = GetComponentInParent<HandHaptics>();
			m_OwnColliders = t_ParentObject.GetComponentsInChildren<Collider>();
		}

		private void FixedUpdate()
		{
			var t_Collisions = Physics.OverlapSphere(GetCollisionPosition(), collisionRange / 100f, layerMask);
			t_Collisions = FilterOutOwnColliders(t_Collisions);

			UpdateCollisionInformation(t_Collisions);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(GetCollisionPosition(), collisionRange / 100f);
		}

		private Collider[] FilterOutOwnColliders(Collider[] p_CurrentCollisions)
		{
			var t_Colliders = new List<Collider>();
			foreach (var t_Collision in p_CurrentCollisions)
			{
				bool t_UseCollider = true;
				foreach (var t_OwnCollider in m_OwnColliders)
				{
					if (t_OwnCollider == t_Collision)
					{
						t_UseCollider = false;
						break;
					}

					if (t_UseCollider)
						t_Colliders.Add(t_Collision);
				}
			}

			return t_Colliders.ToArray();
		}

		private void UpdateCollisionInformation(Collider[] p_Collisions)
		{
			// Remove non existing collisions
			var t_CollisionColliders = m_Collisions.Keys.ToArray();
			foreach (var t_Collider in t_CollisionColliders)
			{
				bool t_Exists = false;

				foreach (var t_OtherCollider in p_Collisions)
				{
					if (t_Collider == t_OtherCollider)
					{
						t_Exists = true;
						break;
					}
				}

				if (!t_Exists)
				{
					if (m_Collisions[t_Collider].UpdateTimeout(Time.fixedDeltaTime)) ;
					m_Collisions.Remove(t_Collider);
				}
			}

			// Add non existing collision and update existing ones
			foreach (var t_Collision in p_Collisions)
			{
				if (t_Collision.isTrigger)
					continue;

				if (m_Collisions.TryGetValue(t_Collision, out var t_CollisionInfo))
				{
					t_CollisionInfo.UpdateInfo(Time.fixedDeltaTime);
				}
				else
				{
					m_Collisions.Add(t_Collision, new CollisionInfo(t_Collision, this));
				}
			}
		}

		private Vector3 GetCollisionPosition()
		{
			return (transform.position * 3f + transform.parent.position) / 4f;
		}

		/// <summary>
		/// Returns the amount of haptics generated by this finger.
		/// All JointHaptics contribute equally to the amount of haptics generated, with the maximum being 1.0f.
		/// </summary>
		/// <returns></returns>
		public float GetHapticValue()
		{
			float t_Strength = 0;

			if (m_Collisions.Count > 0)
			{
				foreach (var t_Collision in m_Collisions)
				{
					t_Strength += t_Collision.Value.GetStrength();
				}
			}

			return t_Strength;
		}
	}

	public class CollisionInfo
	{
		private Collider m_Collider;
		private FingerHaptics m_Finger;
		private Vector3 m_LastFingerLocalPosition;

		private float m_DeltaTime = 1f;

		public CollisionInfo(Collider p_Collider, FingerHaptics p_Finger)
		{
			m_Collider = p_Collider;
			m_Finger = p_Finger;

			Matrix4x4 t_ColliderMatrixInverse = Matrix4x4.TRS(m_Collider.transform.position, m_Collider.transform.rotation, Vector3.one).inverse;
			m_LastFingerLocalPosition = t_ColliderMatrixInverse.MultiplyPoint3x4(m_Finger.transform.position);
		}

		public float collisionTime { get; private set; }

		private float m_CollisionTimeout = 0.5f;

		public void UpdateInfo(float p_DeltaTime)
		{
			m_DeltaTime = p_DeltaTime;
			m_CollisionTimeout = 0.5f;
			collisionTime += p_DeltaTime;
		}

		public bool UpdateTimeout(float p_DeltaTime)
		{
			m_DeltaTime = p_DeltaTime;
			m_CollisionTimeout -= p_DeltaTime;

			if (m_CollisionTimeout < 0)
				return true;

			return false;
		}

		public float GetStrength()
		{
			float t_Strength = 0;
			t_Strength += GetStartStrength() * .3f;
			t_Strength += GetRelativeSpeedStrength();

			return Mathf.Clamp01(t_Strength);
		}

		private float GetStartStrength()
		{
			float t_Time = Mathf.Clamp01(collisionTime / 0.1f);
			return 1f - (-(Mathf.Cos(Mathf.PI * t_Time) - 1f) / 2f);
		}

		private float GetRelativeSpeedStrength()
		{
			var t_GrabbedObject = m_Collider.GetComponentInParent<GrabbedObject>();
			if (t_GrabbedObject != null)
				return 0;

			Matrix4x4 t_ColliderMatrixInverse = Matrix4x4.TRS(m_Collider.transform.position, m_Collider.transform.rotation, Vector3.one).inverse;
			Vector3 t_FingerLocalPosition = t_ColliderMatrixInverse.MultiplyPoint3x4(m_Finger.transform.position);

			float t_RelativeSpeed = Vector3.Distance(t_FingerLocalPosition, m_LastFingerLocalPosition);
			t_RelativeSpeed /= m_DeltaTime;

			t_RelativeSpeed -= 0.03f;
			t_RelativeSpeed *= .8f;
			t_RelativeSpeed = Mathf.Clamp01(t_RelativeSpeed);

			m_LastFingerLocalPosition = t_FingerLocalPosition;

			return t_RelativeSpeed;
		}
	}
}