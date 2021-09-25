using UnityEngine;
 
 [ExecuteAlways]
 public class ClipBoxReal : MonoBehaviour {
     void Update() {
         Shader.SetGlobalMatrix("_WorldToBoxReal", transform.worldToLocalMatrix);
     }
 }