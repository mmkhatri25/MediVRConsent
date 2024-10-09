using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using UnityEngine.UI;

public class ApiController : MonoBehaviour
{

    public static Action<RawImage, string> GetUrlTextureAction;
    public const string BASE_URL = "https://medixr.link/api/v1/get_categories";
    
    private void OnEnable()
    {
        GetUrlTextureAction += GetTextureFromUrl;
    }
    private void OnDisable()
    {
        GetUrlTextureAction -= GetTextureFromUrl;

    }

    void Start()
    {
        Debug.Log("Abc..");

        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        Debug.Log("1 Abc..");

        using (UnityWebRequest request = UnityWebRequest.Get(BASE_URL))
        {
            Debug.Log("waiting..");

            yield return request.SendWebRequest();
            Debug.Log("2 Abc..");

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                Debug.Log("3 Abc..");

            }
            else // Success
            {
                ApiDataSave.JsonVideoData = string.Empty;
                ApiDataSave.JsonVideoData = request.downloadHandler.text;
                Debug.Log(request.downloadHandler.text);
                VideoManager.CreateCatAction?.Invoke();
                Debug.Log("4 Abc..");

            }
        }
    }

    void GetTextureFromUrl(RawImage tex, string textUrl)
    {

        StartCoroutine(GetTexture(tex, textUrl));
    }

    IEnumerator GetTexture(RawImage tex, string textUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(textUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture texturl = ((DownloadHandlerTexture)request.downloadHandler).texture;
            tex.texture = texturl;
            Debug.Log(request.downloadHandler.text);
        }
    }
}


