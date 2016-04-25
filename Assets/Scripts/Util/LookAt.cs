using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public Transform m_target;
	
	//DEBUG propose
	void Update () {
        transform.LookAt(m_target);
	}
}
