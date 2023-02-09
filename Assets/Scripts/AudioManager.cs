using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource[] sounds;

	public static AudioSource standardTurretFire;
	public static AudioSource missileTurretFire;
	public static AudioSource baseTurretFire;
	public static AudioSource explosion;

	public static AudioSource enemyCommanderFire;
	public static AudioSource enemyScoutFire;
	public static AudioSource enemyVanguardFire;
	public static AudioSource enemyVanguardPowerup;
	public static AudioSource enemyVanguardTeleport;
	public static AudioSource enemyWizardFire;

	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource>();

		standardTurretFire = sounds[0];
		missileTurretFire = sounds[1];
		explosion = sounds[2];
		enemyCommanderFire = sounds[3];
		enemyScoutFire = sounds[4];
		enemyVanguardFire = sounds[5];
		enemyWizardFire = sounds[6];
		baseTurretFire = sounds[7];
		enemyVanguardPowerup = sounds[8];
		enemyVanguardTeleport = sounds[9];
	}
}
