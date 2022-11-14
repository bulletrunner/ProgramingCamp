using Assets;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public static class S
{
    public class IDLE : EntityStates.IState
    {
        private enemy_dsrunr_ctrl Obj;
        public IDLE(enemy_dsrunr_ctrl obj)
        {
            Obj = obj;
        }

        public void OnEnter() { }

        public void OnLeave() { }

        public void OnUpdate()
        {
            Vector3 vec = Obj.gameObject.transform.localScale;
            Obj.animator.SetBool("move", true);
            if (Obj.rigidbody.velocity.x >= 0.001f)
                vec.x = math.abs(vec.x);
            else if (Obj.rigidbody.velocity.x <= -0.001f)
                vec.x = -math.abs(vec.x);
            else
                Obj.animator.SetBool("move", false);

            Obj.gameObject.transform.localScale = vec;
        }
    }

    public class MOVE : EntityStates.IState
    {
        private enemy_dsrunr_ctrl Obj;
        public MOVE(enemy_dsrunr_ctrl obj)
        {
            Obj = obj;
        }

        public void OnEnter() { }

        public void OnLeave() { }

        public void OnUpdate() 
        {

        }

    }

    public class LOCKED : EntityStates.IState
    {
        private enemy_dsrunr_ctrl Obj;
        private GameObject Target;
        public LOCKED(enemy_dsrunr_ctrl obj,GameObject tar)
        {
            Obj = obj;
            Target = tar;
        }

        public void OnEnter() { }

        public void OnLeave() { }

        public void OnUpdate()
        {
            Vector3 vec = Target.transform.position - Obj.gameObject.transform.position;
            
            if (math.abs(vec.x) >= 1.3f)
            {
                int d = (int)(vec.x / math.abs(vec).x);
                Vector3 v = Obj.rigidbody.velocity;
                v.x = 2.7f * d;
                Obj.rigidbody.velocity = v;

                vec = Obj.gameObject.transform.localScale;
                vec.x = d*math.abs(vec.x);
                Obj.gameObject.transform.localScale = vec;

                Obj.animator.SetBool("move", true);
            }
            else
            {
                Obj.animator.SetBool("move", false);
            }
        }

    }


}



public class enemy_dsrunr_ctrl : MonoBehaviour
{
    public EntityStates.IState EState;
    public Rigidbody2D rigidbody;
    public Animator animator;

    private Vector3 last_pos;



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EState = new S.IDLE(this);
        last_pos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        EState.OnUpdate();



    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "player")
        {
            EState = new S.LOCKED(this, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            EState = new S.IDLE(this);
        }
    }


}
