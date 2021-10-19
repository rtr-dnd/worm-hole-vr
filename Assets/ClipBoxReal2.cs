using UnityEngine;

[ExecuteAlways]
public class ClipBoxReal2 : MonoBehaviour
{
  void Update()
  {
    Shader.SetGlobalMatrix("_WorldToBoxReal2", transform.worldToLocalMatrix);
  }
}