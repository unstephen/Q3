using System;
using UnityEngine;

namespace GamePlay
{
    public abstract class UGuiFormClone : UGuiForm
    {
        private static Type catch_type = null;

        private static UGuiForm catch_object = null;

        public UGuiFormClone()
        {
            if (UGuiFormClone.catch_type != null && UGuiFormClone.catch_type == base.GetType())
            {
                UGuiFormClone.catch_object = this;
            }
        }

        private static void SetCatch(Type type)
        {
            UGuiFormClone.catch_type = type;
        }

        private static UGuiForm FetchCatch()
        {
            UGuiForm result = UGuiFormClone.catch_object;
            UGuiFormClone.catch_type = null;
            UGuiFormClone.catch_object = null;
            return result;
        }

        public UGuiForm Clone()
        {
            UGuiFormClone.SetCatch(base.GetType());
            GameObject gameObject = UGuiFormClone.AddChild(base.gameObject.transform.parent.gameObject, base.gameObject);
            UGuiForm result;
            if (gameObject == null)
            {
                result = null;
            }
            else
            {
                UGuiForm UGuiForm = UGuiFormClone.FetchCatch();
                if (UGuiForm != null)
                {
                    UGuiForm.DoInitIfDont();
                    result = UGuiForm;
                }
                else
                {
                    UGuiForm UGuiForm2 = gameObject.GetComponent(base.GetType()) as UGuiForm;
                    if (UGuiForm2 != null)
                    {
                        Debug.Log("***** Clone GetComponent called");
                        UGuiForm2.DoInitIfDont();
                        result = UGuiForm2;
                    }
                    else
                    {
                        UGuiForm2 = (gameObject.AddComponent(base.GetType()) as UGuiForm);
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