using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//private bool movable = true;
	public float panSpeed = 20f;
	public float panBordThickness = 10f;

	public float scrollSpeed = 1f;
	public float minY = 30f;
	public float maxY = 50f;

	public Transform centrePoint;
	public bool canMove = true;
	
	// Update is called once per frame
	void Update () {

		if(GameManager.gameOver)
		{
			this.enabled = false;
			return;
		}
		
		if(Input.GetKeyUp(KeyCode.O))
		{
			if(canMove)
				canMove = false;
			else
				canMove = true;
		}

		if(canMove)
		{
			if(Input.mousePosition.y >= Screen.height - panBordThickness)
			{
				transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
			}
			if(Input.mousePosition.y <= panBordThickness)
			{
				transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
			}
			if(Input.mousePosition.x >= Screen.width - panBordThickness)
			{
				transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
			}
			if(Input.mousePosition.x <= panBordThickness)
			{
				transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
			}
		}
		
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		Vector3 pos = transform.position;

		pos.z = Mathf.Clamp(pos.z, -30, 0);
		pos.x = Mathf.Clamp(pos.x, -5, 5);

		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		transform.position = pos;
	}
}
