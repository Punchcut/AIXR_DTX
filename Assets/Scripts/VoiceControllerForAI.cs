using Oculus.Voice;
using UnityEngine;
using TMPro;

public class VoiceControllerForAI : MonoBehaviour
{

    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [SerializeField] ImageGenerator imageGenerator;

    private Transform objectToActOnWithVoice;

    private bool appVoiceActive;

    private void Awake()
    {
        // Listen to the event when the complete transcription is recieved
        appVoiceExperience.TranscriptionEvents.OnFullTranscription.AddListener((transcript) =>
        {
            imageGenerator.GenerateImage(transcript, objectToActOnWithVoice, (transform, texture) =>
            {
                // set hover object texture
                transform.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            });
            // set hover object text
            var tmp = objectToActOnWithVoice.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = transcript;
        });

        // Listen to the event when a partial transcription is recieved
        appVoiceExperience.TranscriptionEvents.OnPartialTranscription.AddListener((transcript) =>
        {
            // set hover object text
            var tmp = objectToActOnWithVoice.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = transcript;
        });

        // Listen to the event when a voice request has created
        appVoiceExperience.VoiceEvents.OnRequestCreated.AddListener((request) =>
        {
            appVoiceActive = true;
            Debug.Log("OnRequestCreated Activated");
        });

        // Listen to the event when a voice request has completed
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(() =>
        {
            appVoiceActive = false;
            Debug.Log("OnRequestCompleted Deactivated");
        });
    }

    public void ActivateVoice(Transform selectedTransform)
    {
        objectToActOnWithVoice = selectedTransform;
        appVoiceExperience.Activate();
    }
}
