using Game.Scripts.Players;
using UnityEngine;

public class EndLevelObject : MonoBehaviour
{
    [SerializeField] private string nameNextScene;

    private int _amountPlayersIn = 0;
    private float _fadeOutTime = 1.2f;
    private BlackScreenController BlackScreenController => BlackScreenController.I;
    private LevelManager _levelManager => LevelManager.I;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_levelManager._levelState == LevelState.END) // Make sure the level hasn't been considered finished already by some death or something
            {
                return;
            }
            
            _levelManager.LevelComplete();
            
            PlayerMovement[] players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
            RopeVisual.I.RopeFadeOut(_fadeOutTime);
            foreach (PlayerMovement player in players)
            {
                StartCoroutine(player.GoToEndLevelObject(transform.position, this, _fadeOutTime));
            }
        }
    }

    public void PlayerEntered()
    {
        _amountPlayersIn++;
        if (_amountPlayersIn == 2)
        {
            BlackScreenController.FadeOutScene(nameNextScene);
        }
    }
}
