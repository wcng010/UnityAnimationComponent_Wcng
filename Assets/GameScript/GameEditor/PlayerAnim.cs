using UnityEditor;
using UnityEngine;
[ExecuteAlways]
public class PlayerAnim : MonoBehaviour
{
    float perTime;
    [SerializeField]Animator animator;
    private void Awake()
    {
        Debug.Log(123);
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Debug.Log(1231);
        perTime = (float)EditorApplication.timeSinceStartup;//从编辑器运行开始计算的时间
        EditorApplication.update += CustomUpdate;
    }
    
    public void CustomUpdate()
    {
        float _nextTime = (float)EditorApplication.timeSinceStartup - perTime;
        perTime = (float)EditorApplication.timeSinceStartup;
        animator.Update(_nextTime);
    }
 
    private void OnDestroy()
    {
        Debug.Log(214124);
        EditorApplication.update -= CustomUpdate;
    }
}
