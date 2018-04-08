using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayerControl : MonoBehaviour
{
    public GameObject Bullet, StartBullet, Bomb;  //Объект "ракета" и ее начальное положение
    public float acceleration;              //Скорость перемещения игрока по x

    public int rocketsAmmo = 50;
    public int bombsAmmo = 10;

    [HideInInspector]
    public static MyPlayerControl Player;

    [HideInInspector]
    public bool facingRight = true;         //Переменная для проверки направления движения игрока 
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int scoreLimit = 10;

    private Vector3 dir = new Vector3(0, 0, 0);
    private bool jump = false;              //Переменная для определения, находится ли игрок в прыжке/падении
    private bool grounded = false;
    private bool aboveEnemy = false;
    private Transform groundCheck;
    private Animator animator;

    private GameManagerScript GameManager;

    private void Awake()
    {
        Player = this;
    }

    private void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        animator = GetComponent<Animator>();
        score = 0;
        groundCheck = transform.Find("groundCheck");
    }

    private void Update()
    {
        if (Time.timeScale < 1f)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.ShowMenuPanel();

        animator.SetBool("IsMoving", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0.1f);

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        aboveEnemy = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Enemies"));

        //При нажатии клавиши "Выстрел", играется соответствующий звук и создается снаряд в том направлении, в которое смотрит игрок
        if (Input.GetButtonDown("Fire1") && rocketsAmmo != 0)
        {
            rocketsAmmo -= 1;
            GetComponent<AudioSource>().Play();
            Instantiate(Bullet, StartBullet.transform.position, StartBullet.transform.rotation);
            animator.SetTrigger("Shot");
        }

        if (Input.GetButtonDown("Fire2") && bombsAmmo != 0)
        {
            bombsAmmo -= 1;
            Instantiate(Bomb, transform.position, transform.rotation);
        }

        //Если нажат "Прыжок" и вертикальное движение игрока меньше подобранной вручную величины (игрок не находится в падении), происходит прыжок
        if (Input.GetButtonDown("Jump") && (grounded || aboveEnemy))
        {
            jump = true;
            animator.SetTrigger("Jump");
        }
    }

    //Движение игрока при нажатии соответствующих клавиш. Также при смене направления движения вызывается метод поворота Flip()
    private void FixedUpdate()
    {
        if (Time.timeScale < 1f)
            return;

        if (score >= scoreLimit)
            StartCoroutine("NextLevel");

        float h = Input.GetAxis("Horizontal");
        if (h > 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(acceleration, GetComponent<Rigidbody2D>().velocity.y);
            if (!facingRight)
                Flip();
        }
        else if (h < 0) 
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-acceleration, GetComponent<Rigidbody2D>().velocity.y);
            if (facingRight)
                Flip();
        }

        //Если определено нажаите клавиши прыжка, игрок получает вертикальный импульс вверх
        if (jump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1000f));
            jump = false;
        }
    }

    //Метод, инвертирующий множитель масштаба по x для объекта игрока. Таким образом достигается эффект поворота на 180.
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
    }

    public void PlusScore(int num)
    {
        score += num;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }
}
