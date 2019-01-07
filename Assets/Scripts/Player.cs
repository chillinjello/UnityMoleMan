using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public float restartLevelDelay = 1f;

    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Rock>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T> (int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //this section is for when  the mole hits an object

        //squirrel

        //sunflower

        //trap
    }

    protected override void OnCantMove <T> (T component)
    {
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
