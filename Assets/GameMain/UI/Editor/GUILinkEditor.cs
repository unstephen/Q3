using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;


[CustomEditor(typeof(GUILink))]
public class UILinkEditor : Editor
{
    [MenuItem("GameObject/AddGameObjectToLink &n", false, 10000)]
    private static void AddGameObjectToLink()
    {
        UnityEngine.GameObject select = Selection.activeGameObject;
        if (select == null)
            return;
        if (!select.activeInHierarchy)
        {
            Debug.LogError(string.Format("【GUILink】自动添加对象失败：请先激活需要添加的对象和GUILink对象", select.name), select);
            return;
        }

        GUILink link = FindInParents<GUILink>(select.transform);
        if (link == null)
        {
            Debug.LogError(string.Format("【GUILink】自动添加对象失败：{0}父节点无GUILink组件", select.name), select);
            return;
        }

        if (link.gameObject == select && select.transform.parent != null)
            link = FindInParents<GUILink>(select.transform.parent);
        if (link == null)
        {
            Debug.LogError(string.Format("【GUILink】自动添加对象失败：{0}父节点无GUILink组件", select.name), select);
            return;
        }
        link.ReBuildLinkMap();
        if (link.Get(select.name) == null)
        {
            GUILink.UILink item = new GUILink.UILink();
            item.Name = select.name;
            item.LinkObj = select;
            link.Links.Add(item);
            Debug.Log(string.Format("【GUILink】自动添加对象成功：{0}  GUILink:{1}", select.name, link.name), link);
        }
        else
            Debug.LogError(string.Format("【GUILink】自动添加对象失败：已经存在重复名字{0}的对象  GUILink:{1}", select.name, link.name), link);
    }

    string errostr = "Erro！！！Link丢失物件，检查";
    public override void OnInspectorGUI()
    {
        GUILink link = target as GUILink;
        GUI.changed = false;
        if (link.Links != null)
        {
            RegisterUndo("GUILink Change", link);
            for (int i = 0; i < link.Links.Count; i++)
            {
                GUILink.UILink uilink = link.Links[i];
                GameObject linkobj = uilink.LinkObj;

                if (!linkobj)
                {
                    uilink.Name = errostr;
                    continue;
                }
                if (linkobj)
                {
                    if (string.IsNullOrEmpty(uilink.Name) || uilink.Name == errostr)
                        uilink.Name = linkobj.name;
                    if (!uilink.component || uilink.component.gameObject != linkobj.gameObject)
                        uilink.component = linkobj.gameObject.GetComponent<MonoBehaviour>();
                }
            }
            EditorUtility.SetDirty(link);
        }
        base.OnInspectorGUI();
    }

    static public void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            UnityEditor.Undo.RecordObjects(objects, name);

            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                EditorUtility.SetDirty(obj);
            }
        }
    }

    static public T FindInParents<T>(Transform trans) where T : Component
    {
        if (trans == null) 
            return null;
        return trans.GetComponentInParent<T>();
    }
}