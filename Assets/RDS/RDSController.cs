using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RDSController : MonoBehaviour
{
    [SerializeField] CustomRenderTexture _texture;
    [SerializeField] Material _surfaceMat;
    //前のフレームのrenderTextureの状態を保存して
    //1対した変化がなければリセットする
    Color[] buf = new Color[0];
    //時間でもリセット
    int random_frame = 0;
    int current_frame = 0;
    
    //random color
    float hueMin = 0;
    float hueMax = 1.0f;
    float saturationMin = 0.5f;
    float saturationMax = 1.0f;

    void Start(){
        random_frame = Random.Range(120, 300);
    }

    
    void Update()
    {
        if (current_frame >= random_frame || IsNotChangedImage()){
            //Update render texture and change parameters
            _texture.Initialize();

            //UpdateMat
            _texture.material.SetFloat("_Du", Random.Range(0, 1.0f));
            _texture.material.SetFloat("_Dv", Random.Range(0, 1.0f));
            _texture.material.SetFloat("_Feed", Random.Range(0, 0.1f));
            _texture.material.SetFloat("_Kill", Random.Range(0, 0.1f));

            //InitMat
            _texture.initializationMaterial.SetFloat("_Seed", Random.Range(0, 1.0f));

            //Surface
            _surfaceMat.SetColor("_Color1", Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 0, 1.0f, 1.0f, 1.0f));
            
            //Time control
            random_frame = Random.Range(120, 300);
            current_frame = 0;
        } else {
            current_frame += 1;
        }
    }

    bool IsNotChangedImage()
    {
        var currentBuf = GetCurrentPixels();
        if (buf.SequenceEqual(currentBuf)) {
            return true;
        } else {
            buf = currentBuf;
            return false;
        }
    }

    Color[] GetCurrentPixels() 
    {
        var currentRT = RenderTexture.active;
        RenderTexture.active = _texture;

        var texture = new Texture2D(_texture.width, _texture.height);
        texture.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
        texture.Apply();

        var colors = texture.GetPixels();
        
        RenderTexture.active = currentRT;

        return colors;
    }
}
