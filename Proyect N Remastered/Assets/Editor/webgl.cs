using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class webgl : Editor
{

    private void Awake()
    {
#if UNITY_WEBGL
        PlayerSettings.WebGL.emscriptenArgs = "-s WASM_MEM_MAX=512MB";
#endif
    }
}
