using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour {

    [ContextMenu("Save")]
	public void Save ()
    {
       
        NavController.c.SaveNav();
    }
}
