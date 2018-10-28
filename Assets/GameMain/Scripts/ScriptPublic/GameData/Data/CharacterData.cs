using UnityEngine;
using UnityEditor;
using UniRx;

public enum CharacterAttrType
{

}
public class CharacterData
{
    public ReactiveProperty<int> Level;
    public void InitFormRemote(int id)
    {

    }

    public CharacterData Clone()
    {
        CharacterData ret = new CharacterData();
        return ret;
    }
    public void RefreshCasheValue()
    {

    }

    public void Reset()
    {

    }
}