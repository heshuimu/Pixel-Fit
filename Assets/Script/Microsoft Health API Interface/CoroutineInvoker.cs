using UnityEngine;
using System.Collections;

public class CoroutineInvoker : MonoBehaviour {
	
	public IEnumerator coroutine;
	
	public void InvokeCoroutine()
	{
		StartCoroutine(Invoker());
	}
	
	IEnumerator Invoker()
	{
		yield return StartCoroutine(coroutine);
		Destroy(gameObject);
	}
}
