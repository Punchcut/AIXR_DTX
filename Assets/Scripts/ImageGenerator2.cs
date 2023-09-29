using System;
using UnityEngine;
using OpenAI;
using OpenAI.Images;
using UnityEngine.XR.Interaction.Toolkit;


public class ImageGenerator2 : MonoBehaviour
{

    [Header("Inputs")]
    [SerializeField]
    [TextArea(5, 20)]
    private string prompt;

    [SerializeField]
    private ImageSize imageSize = ImageSize.Medium;

    [SerializeField] VoiceController2 voiceController;

    private Transform selectedImage;
    // Start is called before the first frame update

    public async void GenerateImage(string prompt, Transform transform, Action<Transform, Texture2D> callBack = null)
    {
        try
        {
            // connect to openAI API
            var api = new OpenAIClient();
            // wait till OpenAI send back generated image
            var results = await api.ImagesEndPoint.GenerateImageAsync(prompt, 1, imageSize);

            foreach (var result in results)
            {
                Debug.Log(result.Key);
                // invoking a callback function, if it exists, passing it the Transform object and the generated Texture2D image
                callBack?.Invoke(transform, result.Value);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void OnFrameHovered(HoverEnterEventArgs args)
    {
        Debug.Log(args.interactableObject.transform.name);
        selectedImage = args.interactableObject.transform;
    }

    public void OnButtonClicked()
    {
        voiceController.ActivateVoice(selectedImage);
    }
}
