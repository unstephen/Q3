using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelSerachClub : UGuiComponent
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();
        link.SetEvent("Create", UIEventType.Click, OnCreateClub);
    }

    private void OnCreateClub(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role != null)
        {
            role.curClubId.SetValueAndForceNotify(-1);
        }
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

    }
}
