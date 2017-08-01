using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMsg;
public class MainRunTime : MonoBehaviour {

    MsgCenterEngine mce;
    bool m_preInit = false;
    // Use this for initialization
    void Start () {
       

    }
	
	// Update is called once per frame
	void Update () {
		if(m_preInit == false)
        {
            m_preInit = true;
            PreLoad();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            Msg m = Msg.Create((int)MsgID.MoveCamera, "value", 125);
            EngineManager.SendMsg(m);
        }
	}

    private void OnDestroy()
    {
        EngineManager.Stop();
    }

    private void PreLoad()
    {
        mce = new MsgCenterEngine();
        EngineManager.Start();
    }
}
