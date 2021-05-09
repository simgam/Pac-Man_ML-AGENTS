using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class movimentoPACMAN : MonoBehaviour
{
    public Text punteggio, vite, powerUp;
    public float speed;
    private Rigidbody rigidBody;
    private static int count;
    bool power = false;
    private static int life, countEat, countSfereEat;

    public Transform ghost1, ghost2, ghost3, ghost4;
    float countdownPowerUp, countdownReset;

    bool reset = false;

    // Start is called before the first frame update
    void Start()
    {
        countdownPowerUp = 0f;
        countdownReset = 7.0f;
        count = 0;
        life = 3;
        countEat = 0;
        countSfereEat = 240;
        rigidBody = GetComponent<Rigidbody>();
        punteggio.text = "Punteggio  = " + count;
        vite.text = "Vite rimaste = " + life;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

    }

    // Update is called once per frame
    void Update()
    {

        if (reset) { StartCoroutine(ResetLevel()); }
        else
        {
            if (power) { StartCoroutine(PowerEat()); }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
            }

            Vector3 move = new Vector3(h, 0.0f, v);

            rigidBody.AddForce(move * speed);

            if (transform.position.x > 9.4f)
            {
                transform.position = new Vector3(-8.5f, 0.084f, 0.69f);
            }

            if (transform.position.x < -8.9f)
            {
                transform.position = new Vector3(9.0f, 0.084f, 0.69f);
            }

            Vector3 distance1 = this.transform.position - ghost1.position;
            Vector3 distance2 = this.transform.position - ghost2.position;
            Vector3 distance3 = this.transform.position - ghost3.position;
            Vector3 distance4 = this.transform.position - ghost4.position;

            if (distance1.magnitude < 0.8f)
            {
                Collision(ghost1.GetComponent<EnemyAi>());
            }
            if (distance2.magnitude < 0.8f)
            {
                Collision(ghost2.GetComponent<EnemyAi>());
            }
            if (distance3.magnitude < 0.8f)
            {
                Collision(ghost3.GetComponent<EnemyAi>());
            }
            if (distance4.magnitude < 0.8f)
            {
                Collision(ghost4.GetComponent<EnemyAi>());
            }
        }
    }

    private void Collision(EnemyAi ghost)
    {
        if (!ghost.GetComponent<EnemyAi>().dead)
        {
            if (power && ghost.GetComponent<EnemyAi>().escape)
            {
                countEat++;
                count += countEat * 200;
                punteggio.text = "Punteggio  = " + count;
            }
            else
            {
                gameOver();
            }
        }         
    }

    public void gameOver()
    {
        life--;
        if (life > 0)
        {
            vite.text = "Vite rimaste = " + life;
            this.transform.position = new Vector3(0f, 0f, -20.5f);
            reset = true;
            countdownPowerUp = 0f;
        }
        else
        {
            life = 3;
            count = 0;
            countSfereEat = 240;
            SceneManager.LoadScene("Game Over");
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bonus")
        {
            Destroy(col.gameObject);
            count += 100;
            punteggio.text = "Punteggio  = " + count;
            if (countSfereEat == 1)
            {
                SceneManager.LoadScene("Vittoria");
            }
            else { countSfereEat--; }
        }

        if (col.gameObject.tag == "cherry")
        {
            Destroy(col.gameObject);
            count += 1000;
            punteggio.text = "Punteggio  = " + count;
        }        

        if (col.gameObject.tag == "power")
        {
            Debug.Log("trigger");
            Destroy(col.gameObject);
            power = true;
            countEat = 0;
            countdownPowerUp += 10;
        }
    }
    
    public IEnumerator PowerEat()
    {
        if (countdownPowerUp >= 0)
        {     
            countdownPowerUp -= Time.deltaTime;
            powerUp.text = ""+countdownPowerUp;
        }
        else
        {            
            power = false;
            powerUp.text = "" + 0;
        }
        yield return null;
    }

    public IEnumerator ResetLevel()
    {
        if (countdownReset >= 0)
        {
            countdownReset -= Time.deltaTime;
        }
        else
        {
            reset = false;
            countdownReset = 7.0f;
            this.transform.position = new Vector3(0f, 0f, -6.5f);
        }
        yield return null;
    }
}