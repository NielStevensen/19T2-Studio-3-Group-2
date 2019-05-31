using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; //This needs to be included to give the code access to the post process stack.

public class GreyScale : MonoBehaviour
{
    ColorGrading ColourGrading = null; //ColorGrading is an effect of the post processand the variable needs to be initialised to null.
    PostProcessVolume PostProcess; //This is a post processing object that is filled with the post process component on the game object.

    public bool Greyscale = false; //This is the bool that turns the post process on or off.

    [Range(-100, 100)]
    public float Saturation; //This is the float variable that gives controls the saturation value of the post process stack. It has a range of -100 to 100.

    [Range(-10, 10)]
    public float Contrast; //This is the float variable that gives controls the contrast value of the post process stack. It has a range of -10 to 10.

    [Range(-5, 5)]
    public float Exposure; //This is the float variable that gives controls the contrast value of the post process stack. It has a range of -5 to 5.

    void Start ()
    {
        PostProcess = GetComponent<PostProcessVolume>(); //This gets the post process volume componentand places it in the PostProcess variable.
        PostProcess.profile.TryGetSettings(out ColourGrading); //This  gets the ColorGrading effect and places it in the ColourGrading variable.
    }
	
	void Update ()
    {
        /*If the GreyScale bool is true then the ColorGrading effect is turned on. While it is turned on any changes to the Saturation, Contrast, or Exposure slider on the gameobject will be directly reflected in the post process stack.*/
        /*If the GreyScale bool is false the ColorGrading effect is turned off and the screen returns to normal.                                                                                                                            */
		if(Greyscale == true)
        {
            ColourGrading.enabled.value = true;
            ColourGrading.saturation.value = Saturation;
            ColourGrading.contrast.value = Contrast;
            ColourGrading.postExposure.value = Exposure;
        }
        else
        {
            ColourGrading.enabled.value = false;
        }
	}
}
