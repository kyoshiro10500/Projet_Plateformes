using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{

    [Header("Engine")]
    [SerializeField]
    private float gravity;

    [SerializeField]
    private float gravity_wall;

    [Header("Player")]
    [SerializeField]
    private float speed_horizontal;

    [SerializeField]
    private float speed_horizontal_dash;

    [SerializeField]
    private float jump_height;

    [SerializeField]
    private float time_stay_up;

    private float gravity_to_use;
    private float speed_to_use;
    private bool jump_on = false;
    private float jump_origin_y;
    private float jump_height_reached = 0;
    private float number_jump = 0f;

    private RaycastHit2D ray_collide_down_l;
    private RaycastHit2D ray_collide_down_r;
    private RaycastHit2D ray_collide_up_l;
    private RaycastHit2D ray_collide_up_r;
    private RaycastHit2D ray_collide_left_u;
    private RaycastHit2D ray_collide_left_d;
    private RaycastHit2D ray_collide_right_u;
    private RaycastHit2D ray_collide_right_d;

    private float sprite_width;
    private float sprite_height;

    // Use this for initialization
    void Start()
    {
        sprite_width = this.GetComponent<Renderer>().bounds.size[0];
        sprite_height = this.GetComponent<Renderer>().bounds.size[1];
        speed_to_use = speed_horizontal;
        gravity_to_use = gravity;
    }

    public void Set_Jump(bool toActivate)
    {
        if (toActivate && number_jump < 2f)
        {
            jump_on = true;
            jump_origin_y = this.transform.position.y;
            jump_height_reached = 0;
            number_jump++;
        }
        else
        {
            jump_on = false;
        }
    }

    public void Set_Dash(bool toActivate)
    {
        if (toActivate)
        {
            speed_to_use = speed_horizontal_dash;
        }
        else
        {
            speed_to_use = speed_horizontal;
        }
    }

    public void Horizontal_Move(float horizontal_axis)
    {

        if ((horizontal_axis > 0 && ray_collide_right_u.collider != null && Mathf.Abs(ray_collide_right_u.point.x - transform.position.x) >= (sprite_width / 2))
            && (horizontal_axis > 0 && ray_collide_right_d.collider != null && Mathf.Abs(ray_collide_right_d.point.x - transform.position.x) >= (sprite_width / 2)))
        {
            if (ray_collide_right_d.point.x <= ray_collide_right_u.point.x)
            {
                if (transform.position.x + sprite_width / 2 + horizontal_axis * Time.deltaTime * speed_to_use > ray_collide_right_d.point.x)
                {
                    this.transform.Translate(new Vector3(ray_collide_right_d.point.x - sprite_height / 2 - transform.position.x, 0, 0));
                    gravity_to_use = gravity_wall;
                }
                else
                {
                    this.transform.Translate(new Vector3(Time.deltaTime * horizontal_axis * speed_to_use, 0, 0));
                    gravity_to_use = gravity;
                }
            }
            else
            {
                if (transform.position.x + sprite_width / 2 + horizontal_axis * Time.deltaTime * speed_to_use > ray_collide_right_u.point.x)
                {
                    this.transform.Translate(new Vector3(ray_collide_right_u.point.x - sprite_height / 2 - transform.position.x, 0, 0));
                    gravity_to_use = gravity_wall;
                }
                else
                {
                    this.transform.Translate(new Vector3(Time.deltaTime * horizontal_axis * speed_to_use, 0, 0));
                    gravity_to_use = gravity;
                }
            }
        }

        if ((horizontal_axis < 0 && ray_collide_left_u.collider != null && Mathf.Abs(ray_collide_left_u.point.x - transform.position.x) >= (sprite_width / 2) )
            && (horizontal_axis < 0 && ray_collide_left_d.collider != null && Mathf.Abs(ray_collide_left_d.point.x - transform.position.x) >= (sprite_width / 2)))
        {
            if (ray_collide_left_d.point.x >= ray_collide_left_u.point.x)
            {
                if (transform.position.x - sprite_width / 2 + horizontal_axis * Time.deltaTime* speed_to_use < ray_collide_left_d.point.x)
                {
                    this.transform.Translate(new Vector3(ray_collide_left_d.point.x - transform.position.x + sprite_width / 2, 0, 0));
                    gravity_to_use = gravity_wall;
                }
                else
                {
                    this.transform.Translate(new Vector3(Time.deltaTime * horizontal_axis * speed_to_use, 0, 0));
                    gravity_to_use = gravity;
                }
            }
            else
            {
                if (transform.position.x - sprite_width / 2 + horizontal_axis * Time.deltaTime* speed_to_use < ray_collide_left_u.point.x)
                {
                    this.transform.Translate(new Vector3(ray_collide_left_u.point.x - transform.position.x + sprite_width / 2, 0, 0));
                    gravity_to_use = gravity_wall;
                }
                else
                {
                    this.transform.Translate(new Vector3(Time.deltaTime * horizontal_axis * speed_to_use, 0, 0));
                    gravity_to_use = gravity;
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;

        ray_collide_up_l = Physics2D.Raycast(new Vector3(pos[0] - (sprite_width / 2) +0.0001f , pos[1], pos[2]), Vector3.up);
        ray_collide_up_r = Physics2D.Raycast(new Vector3(pos[0] + (sprite_width / 2) -0.0001f , pos[1], pos[2]), Vector3.up);
        ray_collide_down_l = Physics2D.Raycast(new Vector3(pos[0] - (sprite_width / 2) + 0.0001f , pos[1], pos[2]), Vector3.down);
        ray_collide_down_r = Physics2D.Raycast(new Vector3(pos[0] + (sprite_width / 2) - 0.0001f , pos[1], pos[2]), Vector3.down);
        ray_collide_left_u = Physics2D.Raycast(new Vector3(pos[0], pos[1] + (sprite_height / 2) - 0.001f , pos[2]), Vector3.left);
        ray_collide_left_d = Physics2D.Raycast(new Vector3(pos[0], pos[1] - (sprite_height / 2) + 0.001f, pos[2]), Vector3.left);
        ray_collide_right_u = Physics2D.Raycast(new Vector3(pos[0], pos[1] + (sprite_height / 2) - 0.001f, pos[2]), Vector3.right);
        ray_collide_right_d = Physics2D.Raycast(new Vector3(pos[0], pos[1] - (sprite_height / 2) +0.001f , pos[2]), Vector3.right);
        Debug.DrawRay(new Vector3(pos[0] - (sprite_width / 2), pos[1], pos[2]), Vector3.up);
        Debug.DrawRay(new Vector3(pos[0] + (sprite_width / 2), pos[1], pos[2]), Vector3.up);
        Debug.DrawRay(new Vector3(pos[0] - (sprite_width / 2), pos[1], pos[2]), Vector3.down);
        Debug.DrawRay(new Vector3(pos[0] + (sprite_width / 2), pos[1], pos[2]), Vector3.down);
        Debug.DrawRay(new Vector3(pos[0], pos[1] + (sprite_height / 2), pos[2]), Vector3.left);
        Debug.DrawRay(new Vector3(pos[0], pos[1] - (sprite_height / 2), pos[2]), Vector3.left);
        Debug.DrawRay(new Vector3(pos[0], pos[1] + (sprite_height / 2), pos[2]), Vector3.right);
        Debug.DrawRay(new Vector3(pos[0], pos[1] - (sprite_height / 2), pos[2]), Vector3.right);
        if (!jump_on)
        {
            if ((ray_collide_down_l.collider != null && Mathf.Abs(ray_collide_down_l.point.y - transform.position.y) >= (sprite_height / 2))
                && (ray_collide_down_r.collider != null && Mathf.Abs(ray_collide_down_r.point.y - transform.position.y) >= (sprite_height / 2)))
            {
                if (ray_collide_down_l.point.y >= ray_collide_down_r.point.y)
                {
                    if (transform.position.y - sprite_height / 2 - Time.deltaTime * gravity_to_use < ray_collide_down_l.point.y)
                    {
                        this.transform.Translate(new Vector3(0, -(transform.position.y - ray_collide_down_l.point.y - sprite_height / 2), 0));
                        number_jump = 0f;
                    }
                    else
                    {
                        this.transform.Translate(new Vector3(0, -Time.deltaTime * gravity_to_use, 0));
                        if(number_jump != 1)
                        {
                            number_jump = 2;
                        }
                    }
                }
                else
                {
                    if (transform.position.y - sprite_height / 2 - Time.deltaTime * gravity_to_use < ray_collide_down_r.point.y)
                    {
                        this.transform.Translate(new Vector3(0,-(transform.position.y - ray_collide_down_r.point.y - sprite_height / 2), 0));
                        number_jump = 0f;
                    }
                    else
                    {
                        this.transform.Translate(new Vector3(0, -Time.deltaTime * gravity_to_use, 0));
                        if (number_jump != 1)
                        {
                            number_jump = 2;
                        }
                    }
                }
            }
            else
            {
                number_jump = 0f;
            }
        }
        else
        {
            if (this.transform.position.y < jump_origin_y + jump_height)
            {
                if ((ray_collide_up_l.collider != null && Mathf.Abs(ray_collide_up_l.point.y - transform.position.y) >= (sprite_height / 2))
                && (ray_collide_up_r.collider != null && Mathf.Abs(ray_collide_up_r.point.y - transform.position.y) >= (sprite_height / 2)))
                {
                    if (ray_collide_up_l.point.y <= ray_collide_up_r.point.y)
                    {
                        if (transform.position.y + sprite_height / 2 + Time.deltaTime * gravity > ray_collide_up_l.point.y)
                        {
                            this.transform.Translate(new Vector3(0, ray_collide_up_l.point.y - transform.position.y - sprite_height / 2, 0));
                            jump_on = false;
                        }
                        else
                        {
                            this.transform.Translate(new Vector3(0, Time.deltaTime * gravity, 0));
                        }
                    }
                    else
                    {
                        if (transform.position.y + sprite_height / 2 + Time.deltaTime * gravity > ray_collide_up_r.point.y)
                        {
                            this.transform.Translate(new Vector3(0, ray_collide_up_r.point.y - transform.position.y - sprite_height / 2, 0));
                            jump_on = false;
                        }
                        else
                        {
                            this.transform.Translate(new Vector3(0, Time.deltaTime * gravity, 0));
                        }
                    }
                }
                else
                {
                    jump_on = false;
                }
            }
            else
            {
                if (jump_height_reached == 0)
                {
                    jump_height_reached = 0.01f;
                }
                else
                {
                    if (jump_height_reached >= time_stay_up)
                    {
                        jump_on = false;
                    }
                    else
                    {
                        jump_height_reached += Time.deltaTime;
                    }
                }
            }
        }
    }

}
