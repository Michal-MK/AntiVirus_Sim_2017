using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour {

	private void Awake() {
		Statics.playerParticles = this;
	}

	Transform target;
	public float force = 10.0f;
	private bool run = false;

	ParticleSystem particleSystem;
	ParticleSystem.Particle[] particles;

	ParticleSystem.MainModule particleSystemMainModule;

	void Start() {
		particleSystem = GetComponent<ParticleSystem>();
		particleSystemMainModule = particleSystem.main;
	}

	public void ActivateScript(Transform newTarget, bool active) {
		target = newTarget;
		run = active;
	}

	void LateUpdate() {
		if (run) {
			int maxParticles = particleSystemMainModule.maxParticles;

			if (particles == null || particles.Length < maxParticles) {
				particles = new ParticleSystem.Particle[maxParticles];
			}

			particleSystem.GetParticles(particles);
			float forceDeltaTime = force * Time.deltaTime;

			Vector3 targetTransformedPosition;

			switch (particleSystemMainModule.simulationSpace) {
				case ParticleSystemSimulationSpace.Local: {
					targetTransformedPosition = transform.InverseTransformPoint(target.position);
					break;
				}
				case ParticleSystemSimulationSpace.Custom: {
					targetTransformedPosition = particleSystemMainModule.customSimulationSpace.InverseTransformPoint(target.position);
					break;
				}
				case ParticleSystemSimulationSpace.World: {
					targetTransformedPosition = target.position;
					break;
				}
				default: {
					throw new System.NotSupportedException(

						string.Format("Unsupported simulation space '{0}'.",
						System.Enum.GetName(typeof(ParticleSystemSimulationSpace), particleSystemMainModule.simulationSpace)));
				}
			}

			int particleCount = particleSystem.particleCount;
			for (int j = 0; j < 10; j++) {
				for (int i = 0; i < particleCount; i++) {
					Vector3 directionToTarget = Vector3.Normalize(targetTransformedPosition - particles[i].position);
					Vector3 seekForce = directionToTarget * forceDeltaTime;
					for (int g = 0; g < 10; g++) {
						particles[i].velocity += seekForce * 10;
					}
				}
			}

			particleSystem.SetParticles(particles, particleCount);
		}
	}
	private void OnDestroy() {
		Statics.playerParticles = null;
	}
}