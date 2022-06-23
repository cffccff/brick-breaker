using UnityEngine;
using MarchingBytes;
using System.Collections;
using System.Collections.Generic;

public class Example : MonoBehaviour {
	
	//public string poolName;
	List<GameObject> goList = new List<GameObject>();
    public void Start()
    {
		//GameObject go1 = EasyObjectPool.instance.GetObjectFromPool("Model2", Vector3.zero, Quaternion.identity);
		//GameObject go2 = EasyObjectPool.instance.GetObjectFromPool("Model2", Vector3.zero, Quaternion.identity);
		//GameObject go3 = EasyObjectPool.instance.GetObjectFromPool("Model2", Vector3.zero, Quaternion.identity);
		//GameObject go4 = EasyObjectPool.instance.GetObjectFromPool("Model2", Vector3.zero, Quaternion.identity);
	}
    public void CreateFromPoolAction() {
		GameObject go = EasyObjectPool.instance.GetObjectFromPool("Model2",new Vector3(10,10,0),Quaternion.identity);
		if(go) {
			goList.Add(go);
		}
	}

	public void ReturnToPoolAction() {
		foreach(GameObject go in goList) {
			EasyObjectPool.instance.ReturnObjectToPool(go);
		}
		goList.Clear();
	}
}
