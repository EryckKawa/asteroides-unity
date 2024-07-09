using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScaleChanger : MonoBehaviour
{
	[SerializeField] private Vector3 scaleIncrement = new Vector3(0.1f, 0.1f, 0.1f);
	[SerializeField] private float interval = 0.6f;

	private Vector3 initialScale;
	private Coroutine scalingCoroutine;

	private void Start()
	{
		initialScale = transform.localScale;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			// Inicia a corrotina de aumento de escala
			if (scalingCoroutine == null)
			{
				scalingCoroutine = StartCoroutine(ScaleUpRoutine());
			}
		}
		else
		{
			// Para a corrotina e reinicia a escala
			if (scalingCoroutine != null)
			{
				StopCoroutine(scalingCoroutine);
				scalingCoroutine = null;
			}
			ResetScale();
		}
	}

	private IEnumerator ScaleUpRoutine()
	{
		while (true)
		{
			Vector3 newScale = transform.localScale + scaleIncrement;
			if (newScale.y >= 0.5f)
			{
				newScale.y = 0.5f;
				transform.localScale = newScale;
				yield break; // Sai da corrotina quando o limite é atingido
			}
			transform.localScale = newScale;
			yield return new WaitForSeconds(interval);
		}
	}

	private void ResetScale()
	{
		transform.localScale = initialScale;
	}
}
