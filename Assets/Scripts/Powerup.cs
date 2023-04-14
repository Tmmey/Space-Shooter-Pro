using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    //0 - TripleShot
    //1 - Speed
    //2 - Shield
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;

                    case 1:
                        player.SpeedActive();
                        break;

                    case 2:
                        player.ShieldActive();
                        break;

                    default:
                        break;
                }
                AudioSource.PlayClipAtPoint(_clip, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
