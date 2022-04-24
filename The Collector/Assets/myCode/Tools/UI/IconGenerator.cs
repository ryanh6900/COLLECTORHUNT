using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class IconGenerator : MonoBehaviour
{
    public Camera camera;
    public string pathFolder = "myTextures/Icons";
    public List<GameObject> sceneObjects;
    public List<ItemObject> itemObjects;
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    [ContextMenu("Screenshot")]
    private void ProcessScreenshots()
    {
        StartCoroutine(Screenshot());
    }

    private IEnumerator Screenshot()
    {
        for (int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject obj = sceneObjects[i];
            ItemObject itemObjectData = itemObjects[i];
            obj.gameObject.SetActive(true);
            yield return null; //returning null or any other value Unity doesnt recognize will schedule coroutine for next frame and run again. 
            TakeScreenshot($"{Application.dataPath}/{pathFolder}/{itemObjectData.name}_Icon.png");
            yield return null;
            obj.gameObject.SetActive(false);

            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/{pathFolder}/{itemObjectData.name}_Icon.png");
            if (s != null)
            {
                itemObjectData.iconImage = s;
                EditorUtility.SetDirty(itemObjectData);
            }
            yield return null;
        }
    }

    void TakeScreenshot(string fullPath)
    {
        if (camera == null) camera = GetComponent<Camera>();

        RenderTexture renderTexture = new RenderTexture(1500, 1500, 24);
        camera.targetTexture = renderTexture;
        Texture2D screenShot = new Texture2D(1500,1500, TextureFormat.RGBA32, false);
        camera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, 1500, 1500), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;

        if (Application.isEditor)
            DestroyImmediate(renderTexture);
        else
            Destroy(renderTexture);

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
#endif
