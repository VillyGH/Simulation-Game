using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private static SoundManager instance;

	[SerializeField]
	private AudioSource cameraAudioSource;

	[SerializeField]
	private AudioClip robotMusic;

	[SerializeField]
	private AudioClip victoryMusic;

	[SerializeField]
	private AudioClip marineHurt;

	[SerializeField]
	private AudioClip marineDead;

	[SerializeField]
	private AudioClip shoot;

	[SerializeField]
	private AudioClip tripleShoot;

	[SerializeField]
	private AudioClip alienDeath;

	[SerializeField]
	private AudioClip robotDeath;

	[SerializeField]
	private AudioClip pickupAppear;

	[SerializeField]
	private AudioClip pickupTaken;

	[SerializeField]
	private AudioClip missileShot;

	private AudioSource audioSource;

	private bool gameIsWon;

	private bool playRobotMusic;

	private float changeMusicTimer;

	public static SoundManager Instance => instance;

	public AudioClip MarineHurtClip => marineHurt;

	public AudioClip MarineDeadClip => marineDead;

	public AudioClip ShootClip => shoot;

	public AudioClip TripleShootClip => tripleShoot;

	public AudioClip PickupAppear => pickupAppear;

	public AudioClip PickupTaken => pickupTaken;

	public AudioClip MissileShot => missileShot;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
			audioSource = GetComponent<AudioSource>();
		}
		else if (instance != this)
		{
            Destroy(gameObject);
		}
	}

	private void Update()
	{
		if (gameIsWon)
		{
			changeMusicTimer += Time.deltaTime;
			cameraAudioSource.volume = (3f - changeMusicTimer) / 3f;
			if (changeMusicTimer >= 3.1)
			{
				cameraAudioSource.Stop();
				cameraAudioSource.loop = false;
				cameraAudioSource.volume = 1f;
				cameraAudioSource.clip = victoryMusic;
				cameraAudioSource.Play();
				gameIsWon = false;
			}
		}
		else if (playRobotMusic)
		{
			changeMusicTimer += Time.deltaTime;
			cameraAudioSource.volume = (3f - changeMusicTimer) / 3f;
			if (changeMusicTimer >= 3.1)
			{
				cameraAudioSource.Stop();
				cameraAudioSource.volume = 1f;
				cameraAudioSource.clip = robotMusic;
				cameraAudioSource.Play();
				playRobotMusic = false;
				changeMusicTimer = 0f;
			}
		}
	}

	public void playAlienDeath(Transform alienPosition)
	{
        transform.position = alienPosition.position;
		audioSource.PlayOneShot(alienDeath, 1f);
	}

	public void playRobotDeath(Transform robotPosition)
	{
        transform.position = robotPosition.position;
		audioSource.PlayOneShot(robotDeath, 1f);
	}

	public void PlayRobotMusic()
	{
		playRobotMusic = true;
	}

	public void GameIsWon()
	{
		gameIsWon = true;
	}
}
