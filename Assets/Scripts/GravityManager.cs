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

    [SerializeField]
    private float time_stay_up;

    [Header("Player")]
    [SerializeField]
    private Vector3 spawn_position;

    [SerializeField]
    private float speed_horizontal;

    [SerializeField]
    private float speed_horizontal_dash;

    [SerializeField]
    private float jump_height;


    [SerializeField]
    private float distance_glue_wall;

    [SerializeField]
    private float acceleration_dash;

    [SerializeField]
    private float time_wall_jump_effect;

    [SerializeField]
    private float impulsion_wall_jump;

    private bool toStayUp = false;
    private float gravity_to_use;
    private float speed_to_use;
    private bool jump_on = false;
    private float jump_origin_y;
    private float jump_height_reached = 0;
    private float number_jump = 0f;
    private float time_up = 0;
    private float wall_jump_speed = 0;
    private float Chrono_wall = 0;
    private bool toDoWallJump = false;
    public bool ToDoWallJump
    {
        get
        {
            return toDoWallJump;
        }
    }
    private bool isAgainstRightWall = false;
    private float time_to_use = 0f;

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
        this.transform.position = spawn_position;
        time_up = 0;
        sprite_width = this.GetComponent<Renderer>().bounds.size[0];
        sprite_height = this.GetComponent<Renderer>().bounds.size[1];
        speed_to_use = speed_horizontal;
        gravity_to_use = gravity;
    }

    public void Set_Jump(bool toActivate)
    {
        if (toActivate && number_jump < 2f)
        {
            if (gravity_to_use == gravity || (gravity_to_use == gravity_wall && ((ray_collide_down_l.collider != null && Mathf.Abs(transform.position.y - ray_collide_down_l.point.y) <= sprite_width / 2)
                                                                                || (ray_collide_down_r.collider != null && Mathf.Abs(transform.position.y - ray_collide_down_r.point.y) <= sprite_width / 2))))
            {
                number_jump++;
            }
            else
            {
                toDoWallJump = true;
                Chrono_wall = 0;
            }
            jump_on = true;
            toStayUp = false;
            jump_origin_y = this.transform.position.y;
            jump_height_reached = 0;

        }
        else
        {
            if (this.transform.position.y != jump_origin_y)
            {
                toStayUp = true;
            }
            jump_on = false;
        }
    }

    public void Set_Dash(bool toActivate)
    {
        if (!(verif_collision(ray_collide_down_l, (sprite_height / 2), false) && verif_collision(ray_collide_down_r, (sprite_height / 2), false)))
        {
            if (toActivate)
            {
                speed_to_use += Time.deltaTime * acceleration_dash;
                if (speed_to_use > speed_horizontal_dash)
                {
                    speed_to_use = speed_horizontal_dash;
                }
            }
            else
            {
                speed_to_use -= 3 * Time.deltaTime * acceleration_dash;
                if (speed_to_use < speed_horizontal)
                {
                    speed_to_use = speed_horizontal;
                }
            }
        }
        else
        {
            if (!toActivate)
            {
                speed_to_use -= 3 * Time.deltaTime * acceleration_dash;
                if (speed_to_use < speed_horizontal)
                {
                    speed_to_use = speed_horizontal;
                }
            }
        }

        if (gravity_to_use == gravity_wall && toActivate)
        {
            speed_to_use = speed_horizontal_dash;
        }
    }
    public void horizontal_movement_regarding_ray(RaycastHit2D ray, float total_speed, float isRight)
    {
        float horizontal_speed = 0;
        if (ray.collider.gameObject.GetComponent<VerticalPlateforme>() != null)
        {
            horizontal_speed = ray.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale;
        }
        bool verif = false;
        if (isRight == 1)
        {
            verif = (transform.position.x + sprite_width / 2 + total_speed * Time.deltaTime > ray.point.x - horizontal_speed * Time.deltaTime);
        }
        else
        {
            verif = (transform.position.x + isRight * sprite_width / 2 + total_speed * Time.deltaTime < ray.point.x + horizontal_speed * Time.deltaTime);
        }
        if (verif)
        {

            this.transform.Translate(new Vector3(ray.point.x - isRight * sprite_width / 2 - transform.position.x + horizontal_speed * Time.deltaTime, 0, 0));
        }
        else
        {
            this.transform.Translate(new Vector3(Time.deltaTime * total_speed, 0, 0));
        }
    }

    public bool verif_collision(RaycastHit2D ray, float condition_distance, bool selonX)
    {
        if (selonX)
        {
            return (ray.collider != null && Mathf.Abs(ray.point.x - transform.position.x) > condition_distance);
        }
        else
        {
            return (ray.collider != null && Mathf.Abs(ray.point.y - transform.position.y) > condition_distance);
        }

    }

    public void make_movement_horizontal(float total_speed)
    {
        if ((total_speed > 0 && verif_collision(ray_collide_right_u, (sprite_width / 2), true)) && (total_speed > 0 && verif_collision(ray_collide_right_d, (sprite_width / 2), true)))
        {
            if (ray_collide_right_d.point.x <= ray_collide_right_u.point.x)
            {
                horizontal_movement_regarding_ray(ray_collide_right_d, total_speed, 1);
            }
            else
            {
                horizontal_movement_regarding_ray(ray_collide_right_u, total_speed, 1);
            }
        }

        if ((total_speed < 0 && verif_collision(ray_collide_left_u, (sprite_width / 2), true)) && (total_speed < 0 && verif_collision(ray_collide_left_d, (sprite_width / 2), true)))
        {
            if (ray_collide_left_d.point.x >= ray_collide_left_u.point.x)
            {
                horizontal_movement_regarding_ray(ray_collide_left_d, total_speed, -1);
            }
            else
            {
                horizontal_movement_regarding_ray(ray_collide_left_u, total_speed, -1);
            }
        }
    }

    public void Horizontal_Move(float horizontal_axis)
    {
        make_movement_horizontal(horizontal_axis * speed_to_use);
    }

    public void GoThroughPlatformDown()
    {
        if (ray_collide_down_l.collider.gameObject.GetComponent<PropertyPlatform>() != null && Mathf.Abs(transform.position.y - ray_collide_down_l.point.y) < sprite_width / 2 &&
            ray_collide_down_r.collider.gameObject.GetComponent<PropertyPlatform>() != null && Mathf.Abs(transform.position.y - ray_collide_down_r.point.y) < sprite_width / 2)
        {
            if (ray_collide_down_l.collider.gameObject.GetComponent<PropertyPlatform>().IsGoThrough && ray_collide_down_r.collider.gameObject.GetComponent<PropertyPlatform>().IsGoThrough)
            {
                transform.position = new Vector3(transform.position.x, ray_collide_down_l.collider.gameObject.transform.position.y - ray_collide_down_r.collider.GetComponent<Renderer>().bounds.size[1] / 2 - sprite_height / 2, transform.position.z);
            }
        }
    }

    public void calculateRay()
    {
        Vector3 pos = this.transform.position;
        ray_collide_up_l = Physics2D.Raycast(new Vector3(pos[0] - (sprite_width / 2) + 0.0001f, pos[1], pos[2]), Vector3.up);
        ray_collide_up_r = Physics2D.Raycast(new Vector3(pos[0] + (sprite_width / 2) - 0.0001f, pos[1], pos[2]), Vector3.up);
        ray_collide_down_l = Physics2D.Raycast(new Vector3(pos[0] - (sprite_width / 2) + 0.0001f, pos[1], pos[2]), Vector3.down);
        ray_collide_down_r = Physics2D.Raycast(new Vector3(pos[0] + (sprite_width / 2) - 0.0001f, pos[1], pos[2]), Vector3.down);
        ray_collide_left_u = Physics2D.Raycast(new Vector3(pos[0], pos[1] + (sprite_height / 2) - 0.001f, pos[2]), Vector3.left);
        ray_collide_left_d = Physics2D.Raycast(new Vector3(pos[0], pos[1] - (sprite_height / 2) + 0.001f, pos[2]), Vector3.left);
        ray_collide_right_u = Physics2D.Raycast(new Vector3(pos[0], pos[1] + (sprite_height / 2) - 0.001f, pos[2]), Vector3.right);
        ray_collide_right_d = Physics2D.Raycast(new Vector3(pos[0], pos[1] - (sprite_height / 2) + 0.001f, pos[2]), Vector3.right);
    }
    
    // Update is called once per frame
    private void LateUpdate()
    {
        calculateRay();
    }

    void Update()
    {
        calculateRay();
        if ((ray_collide_left_u.collider != null && Mathf.Abs(ray_collide_left_u.point.x - transform.position.x) <= (sprite_width / 2) + distance_glue_wall)
               || (ray_collide_left_d.collider != null && Mathf.Abs(ray_collide_left_d.point.x - transform.position.x) <= (sprite_width / 2) + distance_glue_wall))
        {
            if ((ray_collide_left_u.collider != null && ray_collide_left_u.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer && Mathf.Abs(ray_collide_left_u.point.x - transform.position.x) <= (sprite_width / 2))
                || (ray_collide_left_d.collider != null && ray_collide_left_d.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer) && Mathf.Abs(ray_collide_left_d.point.x - transform.position.x) <= (sprite_width / 2))
            {
                this.transform.position = spawn_position;
            }
            else
            {
                isAgainstRightWall = true;
                gravity_to_use = gravity_wall;
                if (Chrono_wall > 0.05)
                {
                    toDoWallJump = false;
                }
                number_jump = 1;
            }
        }
        else
        {
            if ((ray_collide_right_u.collider != null && Mathf.Abs(ray_collide_right_u.point.x - transform.position.x) <= (sprite_width / 2) + distance_glue_wall)
                || (ray_collide_right_d.collider != null && Mathf.Abs(ray_collide_right_d.point.x - transform.position.x) <= (sprite_width / 2) + distance_glue_wall))
            {
                if ((ray_collide_right_u.collider != null && ray_collide_right_u.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer && Mathf.Abs(ray_collide_right_u.point.x - transform.position.x) <= (sprite_width / 2))
                || (ray_collide_right_d.collider != null && ray_collide_right_d.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer) && Mathf.Abs(ray_collide_right_d.point.x - transform.position.x) <= (sprite_width / 2))
                {
                    this.transform.position = spawn_position;
                }
                else
                {
                    isAgainstRightWall = false;
                    gravity_to_use = gravity_wall;
                    if (Chrono_wall > 0.05)
                    {
                        toDoWallJump = false;
                    }
                    number_jump = 1;
                }
            }
            else
            {
                gravity_to_use = gravity;
            }
        }

        if (!jump_on && !toStayUp)
        {
            if (verif_collision(ray_collide_down_l, (sprite_height / 2), false) && verif_collision(ray_collide_down_r, (sprite_height / 2), false))
            {
                if (ray_collide_down_l.point.y >= ray_collide_down_r.point.y)
                {
                    if (transform.position.y - sprite_height / 2 - Time.deltaTime * gravity_to_use < ray_collide_down_l.point.y)
                    {
                        if (ray_collide_down_l.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer)
                        {
                            this.transform.position = spawn_position;
                        }
                        else
                        {
                            this.transform.Translate(new Vector3(0, -(transform.position.y - ray_collide_down_l.point.y - sprite_height / 2), 0));
                            number_jump = 0f;
                        }
                    }
                    else
                    {
                        this.transform.Translate(new Vector3(0, -Time.deltaTime * gravity_to_use, 0));
                        if (number_jump < 1)
                        {
                            number_jump = 1;
                        }
                    }
                }
                else
                {
                    if (transform.position.y - sprite_height / 2 - Time.deltaTime * gravity_to_use < ray_collide_down_r.point.y)
                    {
                        if (ray_collide_down_r.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer)
                        {
                            this.transform.position = spawn_position;
                        }
                        else
                        {
                            this.transform.Translate(new Vector3(0, -(transform.position.y - ray_collide_down_r.point.y - sprite_height / 2), 0));
                            number_jump = 0f;
                        }
                    }
                    else
                    {
                        this.transform.Translate(new Vector3(0, -Time.deltaTime * gravity_to_use, 0));
                        if (number_jump < 1)
                        {
                            number_jump = 1;
                        }
                    }
                }
            }
            else
            {
                number_jump = 0f;
                if (ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>() != null && ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>() != null)
                {
                        this.transform.position = new Vector3(transform.position.x,
                                               (ray_collide_down_r.collider.gameObject.transform.position.y + sprite_height / 2 + ray_collide_down_r.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 + +Time.deltaTime * ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale),
                                                transform.position.z);
                    make_movement_horizontal(ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);
                }
                else if (ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.y - ray_collide_down_r.point.y) > sprite_height / 2))
                {
                        this.transform.position = new Vector3(transform.position.x,
                                               (ray_collide_down_l.collider.gameObject.transform.position.y + sprite_height / 2 + ray_collide_down_l.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 + Time.deltaTime * ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale),
                                                transform.position.z);

                    make_movement_horizontal(ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);

                }
                else if (ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.y - ray_collide_down_l.point.y) <= sprite_height / 2) && ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale > 0)
                {
                        this.transform.position = new Vector3(transform.position.x,
                                               (ray_collide_down_l.collider.gameObject.transform.position.y + sprite_height / 2 + ray_collide_down_l.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 +Time.deltaTime * ray_collide_down_l.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale),
                                                transform.position.z);
                }
                else if (ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.y - ray_collide_down_l.point.y) > sprite_height / 2))
                {
                        this.transform.position = new Vector3(transform.position.x,
                                               (ray_collide_down_r.collider.gameObject.transform.position.y + +sprite_height / 2 + ray_collide_down_r.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 + Time.deltaTime * ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale),
                                                transform.position.z);
                    make_movement_horizontal(ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);
                }
                else if (ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.y - ray_collide_down_r.point.y) <= sprite_height / 2) && ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale > 0)
                {
                        this.transform.position = new Vector3(transform.position.x,
                                           (ray_collide_down_r.collider.gameObject.transform.position.y + sprite_height / 2 + ray_collide_down_r.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 + Time.deltaTime * ray_collide_down_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale),
                                            transform.position.z);
                }
            }
        }
        else
        {
            if (this.transform.position.y < jump_origin_y + jump_height && !toStayUp)
            {
                if (verif_collision(ray_collide_up_l, (sprite_height / 2), false) && verif_collision(ray_collide_up_r, (sprite_height / 2), false))
                {
                    if (ray_collide_up_l.point.y <= ray_collide_up_r.point.y)
                    {
                        if (transform.position.y + sprite_height / 2 + Time.deltaTime * gravity > ray_collide_up_l.point.y)
                        {
                            if (!ray_collide_up_l.collider.gameObject.GetComponent<PropertyPlatform>().IsGoThrough)
                            {
                                if (ray_collide_up_l.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer)
                                {
                                    this.transform.position = spawn_position;
                                }
                                else
                                {
                                    this.transform.Translate(new Vector3(0, ray_collide_up_l.point.y - transform.position.y - sprite_height / 2, 0));
                                    toStayUp = true;
                                    jump_on = false;
                                }
                            }
                            else
                            {
                                if (ray_collide_up_l.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer)
                                {
                                    this.transform.position = spawn_position;
                                }
                                else
                                {
                                    this.transform.position = new Vector3(this.transform.position.x,
                                                              ray_collide_up_l.collider.gameObject.transform.position.y + sprite_height / 2 + ray_collide_up_l.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2
                                                              , this.transform.position.z);
                                }

                            }
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
                            if (!ray_collide_up_r.collider.gameObject.GetComponent<PropertyPlatform>().IsGoThrough)
                            {
                                if (ray_collide_up_r.collider.gameObject.GetComponent<PropertyPlatform>().IsKillPlayer)
                                {
                                    this.transform.position = spawn_position;
                                }
                                else
                                {
                                    this.transform.Translate(new Vector3(0, ray_collide_up_r.point.y - transform.position.y - sprite_height / 2, 0));
                                    jump_on = false;
                                    toStayUp = true;
                                }
                            }
                            else
                            {
                                this.transform.position = new Vector3(this.transform.position.x,
                                                              ray_collide_up_r.collider.gameObject.transform.position.y + sprite_height / 2 + ray_collide_up_r.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2
                                                              , this.transform.position.z);
                                jump_on = false;
                            }
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
                    toStayUp = true;
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
                        toStayUp = false;
                    }
                    else
                    {
                        jump_height_reached += Time.deltaTime;
                    }
                }
            }
        }


       

        if (ray_collide_right_d.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.x - ray_collide_right_d.point.x) <= sprite_width / 2 + 0.02f))
        {
            if (ray_collide_right_d.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale < 0)
            {
                make_movement_horizontal(ray_collide_right_d.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);
            }
        }
        else if (ray_collide_right_u.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.x - ray_collide_right_u.point.x) <= sprite_width / 2 + 0.02f))
        {
            if (ray_collide_right_u.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale < 0)
            {
                make_movement_horizontal(ray_collide_right_u.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);
            }
        }


        if (ray_collide_left_d.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.x - ray_collide_left_d.point.x) <= sprite_width / 2 + 0.02f))
        {
            if (ray_collide_left_d.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale > 0)
            {
                make_movement_horizontal(ray_collide_left_d.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);
            }
        }
        else if (ray_collide_left_u.collider.gameObject.GetComponent<VerticalPlateforme>() != null && (Mathf.Abs(transform.position.x - ray_collide_left_u.point.x) <= sprite_width / 2 + 0.02f))
        {
            if (ray_collide_left_u.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale > 0)
            {
                make_movement_horizontal(ray_collide_left_u.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_horizontale);
            }
        }

        if (ray_collide_up_l.collider.gameObject.GetComponent<VerticalPlateforme>() != null && Mathf.Abs(transform.position.y - ray_collide_up_l.point.y) <= sprite_height / 2 && ray_collide_up_l.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale < 0)
        {
            transform.position = new Vector3(transform.position.x, ray_collide_up_l.collider.gameObject.transform.position.y - ray_collide_up_l.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 - sprite_width / 2 - 0.1f, transform.position.z);
        }
        else if (ray_collide_up_r.collider.gameObject.GetComponent<VerticalPlateforme>() != null && Mathf.Abs(transform.position.y - ray_collide_up_r.point.y) <= sprite_height / 2 && ray_collide_up_r.collider.gameObject.GetComponent<VerticalPlateforme>().Vitesse_verticale < 0)
        {
            transform.position = new Vector3(transform.position.x, ray_collide_up_r.collider.gameObject.transform.position.y - ray_collide_up_r.collider.gameObject.GetComponent<Renderer>().bounds.size[1] / 2 - sprite_width / 2 - 0.1f, transform.position.z);
        }

        if (toDoWallJump)
        {
            if (Chrono_wall == 0)
            {
                time_to_use = time_wall_jump_effect;
                if (speed_to_use == speed_horizontal_dash)
                {
                    time_to_use /= 2;
                }
            }

            if (Chrono_wall < time_to_use)
            {
                Chrono_wall += Time.deltaTime;
                if (isAgainstRightWall)
                {
                    make_movement_horizontal(impulsion_wall_jump * speed_to_use);
                }
                else
                {
                    make_movement_horizontal(-impulsion_wall_jump * speed_to_use);
                }
            }
            else
            {
                Chrono_wall = 0;
                toDoWallJump = false;
            }
        }
    }
}
