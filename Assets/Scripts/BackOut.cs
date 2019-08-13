using UnityEngine;
using UnityEngine.SceneManagement;

public class BackOut : MonoBehaviour
{
	[Tooltip("Name of the menu scene.")]
	public string menu;

	//Return to the menu
	public void OnBackOut()
	{
		SceneManager.LoadScene(menu);
	}
}
