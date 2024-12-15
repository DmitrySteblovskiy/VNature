using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
public class Video360Player : MonoBehaviour
{
    [Header("Video Settings")]
    [Tooltip("Видео клип для воспроизведения")]
    public VideoClip videoClip;

    [Tooltip("URL для стримингового видео (если не используется VideoClip)")]
    public string videoURL;

    [Header("Sphere Settings")]
    [Tooltip("Material сферы, на которую будет наложено видео")]
    public Material sphereMaterial;

    [Tooltip("Название свойства материала для текстуры видео (обычно _MainTex)")]
    public string materialProperty = "_MainTex";

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    void Start()
    {
        // Получаем компоненты VideoPlayer и AudioSource
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        // Настройка VideoPlayer
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
        }
        else if (!string.IsNullOrEmpty(videoURL))
        {
            videoPlayer.url = videoURL;
        }
        else
        {
            Debug.LogError("Необходимо указать либо VideoClip, либо VideoURL.");
            return;
        }

        // Настройка режима рендера
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = materialProperty;

        // Настройка аудио
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // Запуск видео
        videoPlayer.isLooping = true;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer vp)
    {
        vp.Play();
        audioSource.Play();
    }

    void OnValidate()
    {
        // В режиме редактирования, можно обновить материал
        if (sphereMaterial != null && videoPlayer != null)
        {
            videoPlayer.targetMaterialProperty = materialProperty;
        }
    }
}
