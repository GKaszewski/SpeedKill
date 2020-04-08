using UnityEngine;
using System.Collections;
using Photon.Pun;
/*
 * Credit To : Scott Sewell, developer at KinematicSoup
 * http://www.kinematicsoup.com/news/2016/8/9/rrypp5tkubynjwxhxjzd42s3o034o8
 * /
 
/*
 * Used to allow a later script execution order for FixedUpdate than in GameplayTransform.
 * It is critical this script runs after all other scripts that modify a transform from FixedUpdate.
 */
public class NetworkInterpolatedTransformUpdater : MonoBehaviourPunCallbacks
{
    private NetworkInterpolatedTransform m_interpolatedTransform;
    
	void Awake()
    {
        if (!photonView.IsMine) return;
        m_interpolatedTransform = GetComponent<NetworkInterpolatedTransform>();
    }
	
	void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        m_interpolatedTransform.LateFixedUpdate();
    }
}
