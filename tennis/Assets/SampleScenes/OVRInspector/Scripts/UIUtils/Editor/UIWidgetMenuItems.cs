/************************************************************************************

Copyright   :   Copyright 2017 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.4.1 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

https://developer.oculus.com/licenses/sdk-3.4.1


Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/
using UnityEngine;
using UnityEditor;

public class UIWidgetMenuItems : MonoBehaviour
{

    [MenuItem ("GameObject/UI/Sample Framework Slider")]
    static void CreateSlider()
    {
		CreateUIWidget("Assets/SampleScenes/OVRInspector/Prefabs/SliderWithLabels.prefab", "New Sample Framework Slider"); 
    }

    [MenuItem ("GameObject/UI/Sample Framework Toggle Box")]
    static void CreateToggle()
    {
		CreateUIWidget("Assets/SampleScenes/OVRInspector/Prefabs/ToggleWithLabels.prefab", "New Sample Framework Toggle"); 
    }

    [MenuItem ("GameObject/UI/Sample Framework Button")]
    static void CreateButton()
    {
		CreateUIWidget("Assets/SampleScenes/OVRInspector/Resources/Prefabs/Button.prefab", "New Sample Framework Button"); 
    }

    static void CreateUIWidget(string path, string objectName)
    {
        GameObject selection = Selection.activeGameObject;
        GameObject widgetAsset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        GameObject widgetInstantiated = Instantiate(widgetAsset) as GameObject;
        widgetInstantiated.name = objectName;
        if (selection != null)
        {
            widgetInstantiated.transform.SetParent(selection.transform, false);
        }
    }
}
