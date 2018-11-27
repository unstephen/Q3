using System;
using UnityEngine;

namespace GamePlay
{
    public abstract class UGuiComponentClone : UGuiComponent
    {
        private static Type catch_type = null;

        private static UGuiComponent catch_object = null;

        public UGuiComponentClone()
        {
            if (UGuiComponentClone.catch_type != null && UGuiComponentClone.catch_type == base.GetType())
            {
                UGuiComponentClone.catch_object = this;
            }
        }

        private static void SetCatch(Type type)
        {
            UGuiComponentClone.catch_type = type;
        }

        private static UGuiComponent FetchCatch()
        {
            UGuiComponent result = UGuiComponentClone.catch_object;
            UGuiComponentClone.catch_type = null;
            UGuiComponentClone.catch_object = null;
            return result;
        }

        public UGuiComponent Clone()
        {
            UGuiComponentClone.SetCatch(base.GetType());
            GameObject gameObject = UGuiComponentClone.AddChild(base.gameObject.transform.parent.gameObject, base.gameObject);
            UGuiComponent result;
            if (gameObject == null)
            {
                result = null;
            }
            else
            {
                UGuiComponent UGuiForm = UGuiComponentClone.FetchCatch();
                if (UGuiForm != null)
                {
                    UGuiForm.DoInitIfDont();
                    result = UGuiForm;
                }
                else
                {
                    UGuiComponent UGuiForm2 = gameObject.GetComponent(base.GetType()) as UGuiComponent;
                    if (UGuiForm2 != null)
                    {
                        Debug.Log("***** Clone GetComponent called");
                        UGuiForm2.DoInitIfDont();
                        result = UGuiForm2;
                    }
                    else
                    {
                        UGuiForm2 = (gameObject.AddComponent(base.GetType()) as UGuiComponent);
                        if (UGuiForm2 != null)
                        {
                            UGuiForm2.DoInitIfDont();
                        }
                        result = UGuiForm2;
                    }
                }
            }
            return result;
        }

        public static GameObject AddChild(GameObject parent, GameObject prefab)
        {
            GameObject gameObject = Instantiate<GameObject>(prefab);
            if (gameObject != null && parent != null)
            {
                Transform transform = gameObject.transform;
                transform.SetParent(parent.transform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                gameObject.layer = parent.layer;
            }
            return gameObject;
        }
    }
}