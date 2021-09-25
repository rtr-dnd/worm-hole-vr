using UnityEngine;
 
 [ExecuteAlways]
 public class ClipBoxVirtual : MonoBehaviour {
     void Update() {
         Shader.SetGlobalMatrix("_WorldToBoxVirtual", transform.worldToLocalMatrix);
     }
 }