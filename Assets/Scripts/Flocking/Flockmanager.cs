using System.Collections.Generic;
using UnityEngine;

public static class FlockManager
{
	public static readonly List<FlockAgent> FlockAgents = new List<FlockAgent>();
	public static readonly List<FlockHunter> FlockHunters = new List<FlockHunter>();

	public static void Register(FlockAgent agent)
	{
		if (!FlockAgents.Contains(agent))
			FlockAgents.Add(agent);
	}
	public static void Unregister(FlockAgent agent)
	{
		FlockAgents.Remove(agent);
	}

	public static void Register(FlockHunter hunter)
	{
		if (!FlockHunters.Contains(hunter))
			FlockHunters.Add(hunter);
	}
	public static void Unregister(FlockHunter hunter)
	{
		FlockHunters.Remove(hunter);
	}

	/// functions for finding nearest Hunter / Agent

	public static FlockHunter GetNearestHunter(Vector3 position, out float distance)
	{
		FlockHunter nearest = null;
		distance = float.MaxValue;

		foreach (var hunter in FlockHunters)
		{
			if (hunter == null) continue;
			float d = Vector3.Distance(position, hunter.transform.position);
			if (d < distance)
			{
				distance = d;
				nearest = hunter;
			}
		}

		return nearest;
	}

	public static FlockAgent GetNearestFlockAgent(Vector3 position, out float distance, FlockAgent exclude = null)
	{
		FlockAgent nearest = null;
		distance = float.MaxValue;

		foreach (var agent in FlockAgents)
		{
			if (agent == null || agent == exclude) continue;
			float d = Vector3.Distance(position, agent.transform.position);
			if (d < distance)
			{
				distance = d;
				nearest = agent;
			}
		}
		return nearest;
	}

	/// Rule 4: Speed Limit

	public static Vector3 ClampSpeed(Vector3 velocity, float minSpeed, float maxSpeed, Vector3 fallbackDirection)
	{
		float speed = velocity.magnitude;

		// Accomodate cases where speed is (near) zero so as to not to break the trig functions.
		if (speed < 0.0001f)
		{
			Vector3 fallback;
			
			if (fallbackDirection.sqrMagnitude > 0.001)
			{
				fallback = fallbackDirection.normalized;
			}
			else
			{
				fallback = Vector3.forward;
			}

			return fallback * minSpeed;
		}

		if (speed > maxSpeed)
			return velocity / speed * maxSpeed;
		if (speed < minSpeed)
			return velocity / speed * minSpeed;

		return velocity;
	}
}
