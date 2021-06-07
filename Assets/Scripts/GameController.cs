using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ScoreController PlayerScoreController;
    public ScoreController ComputerScoreController;
    public Rigidbody2D Ball;

    public int PlayerScore;
    public int ComputerScore;

    public Collider2D PlayerGoal;
    public Collider2D ComputerGoal;

    public int MaxScore = 2;

    public bool PlayerWin;

    public GameObject GetReady;
    public GameObject GoalText;
    public Animator[] PlayerGoalLights;
    public Animator[] ComputerGoalLights;

    private bool _BallInPlay = false;
    public bool BallInPlay {
        get {
            return _BallInPlay;
        }
        set
        {
            _BallInPlay = value;
            HandleBannerState();
        }
    }

    public UIController UIController;

    #region Sounds
    public AudioClip SoundGameOver;
    public AudioClip SoundThrow;
    public AudioClip[] SoundsGoal;
    public AudioClip SoundOoh;
    #endregion

    public GameObject Player;
    public GameObject Computer;
    public GameObject ComputerTouchPoint;

    [Range(1, 10)]
    public int AdByGameOver = 2;

    private Vector2 _ballStartupPosition;
    private Vector2 _playerStartupPosition;
    private Vector2 _computerStartupPosition;

    private AudioSource _audioSource;
    private SettingsController _settingController;
    private AdController _adController;
    private GameObject _ComputerTouchPointOnStart;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _settingController = GetComponent<SettingsController>();
        _adController = GetComponent<AdController>();

        _ComputerTouchPointOnStart = ComputerTouchPoint;

        if (!_settingController.SoundsOn)
            AudioListener.volume = 0f;

        UIController.Activate(UIController.UIPanels.Start);

        StopAnimations(PlayerGoalLights);
        StopAnimations(ComputerGoalLights);

        _ballStartupPosition = Ball.transform.position;
        _playerStartupPosition = Player.transform.position;
        _computerStartupPosition = Computer.transform.position;

        UIController.UIChanged.AddListener(HandleBannerState);
    }

    private void HandleBannerState()
    {
        if (_adController.BannerActive)
        {
            if (BallInPlay)
            {
                _adController.HideBanner();
            }
            else if (UIController.IsActiveAny())
            {
                if(UIController.ActivePanelId() == (int)UIController.UIPanels.Start)
                    _adController.HideBanner();
                else
                    _adController.ShowBanner();
            }
            else
            {
                _adController.HideBanner();
            }
        }
    }

    void Update()
    {
        // TODO:
        Ball.GetComponent<Collider2D>().enabled = BallInPlay;
        Ball.GetComponent<Rigidbody2D>().simulated = BallInPlay;
    }

    #region GamePlay
    private float _gameStartedAt = 0f;

    public void StartGame()
    {
        StartGameInternal();
    }

    public void StartGameP(bool playVsBot)
    {
        _settingController.PlayVsComputer = playVsBot;
        StartGameInternal();
    }



    private void StartGameInternal()
    {
        _gameStartedAt = Time.time;

        ResetScore();

        UpdatePlayVsComputer();

        Player.transform.position = _playerStartupPosition;
        Computer.transform.position = _computerStartupPosition;

        UIController.DeactivateAll();
        BallInPlay = true;
        ThrowBallFromStart(0f);
    }


    private void UpdatePlayVsComputer()
    {
        var playerController = Computer.GetComponent<PlayerController>();
        var computerController = Computer.GetComponent<ComputerController>();
        if (_settingController.PlayVsComputer)
        {
            if (playerController.TouchObject != Computer.transform)
            {
                computerController.enabled = true;
                playerController.TouchObject.gameObject.SetActive(false);
                playerController.TouchObject = Computer.transform;
                playerController.enabled = false;
            }
        }
        else
        {
            Computer.GetComponent<ComputerController>().enabled = false;
            playerController.enabled = true;
            playerController.TouchObject = ComputerTouchPoint.transform;
            playerController.TouchObject.gameObject.SetActive(true);
        }
    }

    public void PauseAndShow(int panel)
    {
        BallInPlay = false;
        UIController.Activate(panel);
    }

    public void ResumeGame()
    {
        UpdatePlayVsComputer();
        BallInPlay = true;;
        UIController.DeactivateAll();
    }

    public void RestartGame()
    {
        PlayerWin = false;

        StartGame();
    }

    private void ResetScore()
    {
        PlayerScore = 0;
        ComputerScore = 0;
        UpdateScores();
    }

    private void GameOver()
    {
        PlaySound(SoundGameOver);
        PlayerWin = (PlayerScore >= MaxScore);
        UIController.Activate((int)UIController.UIPanels.GameOver);
        Ball.velocity = Vector2.zero;

        _settingController.PlayCount++;

        if (_settingController.PlayCount % AdByGameOver == 0)
        {
            _adController.ShowInsterstitial();
        }
    }

    #endregion

    public void GoalFrom(bool player, bool computer)
    {
        BallInPlay = false;
        var goalTextPrefab = Instantiate(GoalText);

        var goalTextPrefabAudioSource = goalTextPrefab.GetComponent<AudioSource>();
        goalTextPrefab.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (_settingController.PlayVsComputer)
        {
            if(computer) {
                PlaySound(SoundOoh);
            }
            else
            {
                PlaySound(SoundsGoal);
            }
        }
        else
        {
            if (computer)
            {
                goalTextPrefab.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            PlaySound(SoundsGoal);
        }


        if (player)
        {
            PlayerScore++;
            PlayAnimations(PlayerGoalLights);
        }
        else if (computer)
        {
            ComputerScore++;
            PlayAnimations(ComputerGoalLights);
        }

        UpdateScores();

        if (ComputerScore >= MaxScore || PlayerScore >= MaxScore)
        {
            GameOver();
        }
        else
        {
            ThrowBallFromStart(2.2f);
        }
    }

    private void PlaySound(AudioClip[] audioClips)
    {
        if (audioClips != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, audioClips.Length - 1);
            var audioSource = GetComponent<AudioSource>();
            PlaySound(audioClips[randomIndex]);
        }
    }

    private void PlaySound(AudioClip audioClip)
    {
        //_audioSource.clip = audioClip;
        _audioSource.PlayOneShot(audioClip);
    }

    private void UpdateScores()
    {
        PlayerScoreController.Set(PlayerScore);
        ComputerScoreController.Set(ComputerScore);
    }

    private void PlayAnimations(Animator[] lights)
    {
        if(lights != null) {
            foreach (var item in lights)
            {
                item.enabled = true;
                item.Play(null);
            }
        }
    }

    private void StopAnimations(Animator[] lights)
    {
        if (lights != null)
        {
            foreach (var item in lights)
            {
                item.enabled = false;
                //item.StopPlayback();
            }
        }
    }

    #region ThrowBall
    public void ThrowBallFromStart(float timeToThrow)
    {
        Ball.gameObject.GetComponent<TrailRenderer>().enabled = false;

        Ball.velocity = Vector2.zero;
        Ball.transform.position = _ballStartupPosition;

        Invoke("ThrowBall", timeToThrow);
    }

    void ThrowBall()
    {
        var getReady = Instantiate(GetReady);
        getReady.transform.position = Vector2.zero;

        StartCoroutine("ThrowBallInternal", 1f);
    }

    IEnumerator ThrowBallInternal(float delay) {
        yield return new WaitForSeconds(delay);

        var gameStartedAt = _gameStartedAt;

        while (UIController.IsActiveAny())
        {
            yield return new WaitForSeconds(0.3f);
        }

        if (gameStartedAt.Equals(_gameStartedAt))
        {
            PlaySound(SoundThrow);

            Ball.gameObject.GetComponent<TrailRenderer>().enabled = true;

            var randomVector = new Vector2(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f));
            if (Math.Abs(randomVector.x) < 1f)
                randomVector.x = (randomVector.x > 0) ? 1f : -1f;
            if (Math.Abs(randomVector.y) < 1f)
                randomVector.y = (randomVector.y > 0) ? 1f : -1f;


            Ball.AddForce(randomVector, ForceMode2D.Impulse);

            BallInPlay = true;
        }
    }
    #endregion
}