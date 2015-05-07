using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour {

	public float speed = 0.4f;
	private Vector2 dest = Vector2.zero;
	private DirectionType direction;
	private Vector2 touchOrigin;
	private bool hasTouched;

	// Use this for initialization
	void Start () {
		dest = new Vector2 (transform.position.x, transform.position.y);
		hasTouched = false;
	}

	void Update()
	{
//		#if  UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
//		//		 Check for Input if not moving
//		if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
//		{
//			direction = DirectionType.up;
//		}
//		else if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
//		{
//			direction = DirectionType.down;
//		}
//		else if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
//		{
//			direction = DirectionType.left;
//		}
//		else if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
//		{
//			direction = DirectionType.right;
//		}
//		#else
		if(Input.touchCount > 0)
		{
			Touch myTouch = Input.touches[0];
			
			if(myTouch.phase == TouchPhase.Began)
			{
				touchOrigin = myTouch.position;
				hasTouched = true;
			} else if (myTouch.phase == TouchPhase.Ended && hasTouched) //TODO check this x > =0;
			{
				Vector2 touchEnd = myTouch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				hasTouched = false;
				if(Mathf.Abs(x) > Mathf.Abs(y))
				{
					var horizontal = x > 0 ? 1 : -1;
					if(horizontal == 1)
					{
						direction = DirectionType.right;
					}else{
					
						direction = DirectionType.left;
					}
				}
				else
				{
					var vertical = y > 0 ? 1 : -1;
					if(vertical == 1)
					{
						direction = DirectionType.up;
					}else{
						direction = DirectionType.down;
					}
				}
			}
		}
//		#endif
	}
	
	// Update is called once per frame
	void FixedUpdate () {


		// Move closer to Destination
		Vector2 p = Vector2.MoveTowards (transform.position, dest, speed);
		GetComponent<Rigidbody2D> ().MovePosition (p);

		switch (direction) {
		case DirectionType.up:
			dest = (Vector2)transform.position + Vector2.up;
			break;
		case DirectionType.down:
			dest = (Vector2)transform.position - Vector2.up;
			break;
		case DirectionType.left:
			dest = (Vector2)transform.position - Vector2.right;
			break;
		case DirectionType.right:
			dest = (Vector2)transform.position + Vector2.right;
			break;
		}


		// Animation Parameters
		Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator> ().SetFloat ("DirX", dir.x);
		GetComponent<Animator> ().SetFloat ("DirY", dir.y);
	
	}

	private bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}
}

public enum DirectionType{
	up,
	down,
	left,
	right
}
