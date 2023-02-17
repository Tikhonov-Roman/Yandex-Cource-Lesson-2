using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeController : MonoBehaviour
{   
    [SerializeField]private float _swingForce;
    [SerializeField] private float _minForceAngle;
    [SerializeField] private float _maxForceAngle;
    private bool _isConnected = true;
    private Joint2D _playerJoint2d;
    private Rigidbody2D _rigidbody2d;
    private Collider2D _collider2d;

    
   
    
    private void Start()
    {
        _playerJoint2d = GetComponent<Joint2D>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
    }
    private void Update()
    {   
        if(Input.GetMouseButton(0))
        {
            DisconnectRope();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!_isConnected && collider.gameObject.tag == "RopeSegment")
        {
            ConnectRope(collider.attachedRigidbody);
        }
    }
    private void FixedUpdate()
    {   if(_isConnected)    
        {   
            if(_rigidbody2d.velocity.x > 0 && transform.eulerAngles.z < _maxForceAngle)
            {  
                _rigidbody2d.AddForce(transform.right * _swingForce );
            }
            else if(_rigidbody2d.velocity.x < 0 && transform.eulerAngles.z > _minForceAngle)
            {
                _rigidbody2d.AddForce(transform.right * -1 * _swingForce );
                
            }
        }
    }
    private void DisconnectRope()
    {
        _playerJoint2d.connectedBody = null;
        _playerJoint2d.enabled = false;
        _isConnected = false;
       
    }
    private void ConnectRope(Rigidbody2D ropeRigidbody)
    {

        _isConnected = true;
        RotateToTarget(ropeRigidbody.transform);
        MoveToSpriteBottom(ropeRigidbody.GetComponent<Collider2D>());
        _playerJoint2d.connectedBody = ropeRigidbody;
        _playerJoint2d.enabled = true;
        

    }
    private void RotateToTarget(Transform targetTransform)
    {   
        transform.eulerAngles = targetTransform.eulerAngles;
    }
    private void MoveToSpriteBottom(Collider2D targetCollider )
    {
        Vector3 spriteBottom = targetCollider.transform.up * - 1 *  targetCollider.bounds.size.y / 2 +  targetCollider.transform.position ;

        transform.position = spriteBottom - transform.up * _collider2d.bounds.size.y / 2;
    }



    
}
