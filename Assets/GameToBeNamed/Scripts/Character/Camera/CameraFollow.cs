using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using UnityEngine;

namespace GameToBeNamed.Character
{
	public class CameraFollow : MonoBehaviour {

	private Character2D target;
	[Header("Camera settings")]
	[SerializeField] private float m_verticalOffset;
	[SerializeField] private float  m_lookAheadDstX;
	[SerializeField] private float m_lookSmoothTimeX;
	[SerializeField] private float m_verticalSmoothTime;
	[SerializeField] private Vector2 m_focusAreaSize;
	[SerializeField] private Vector3 m_screenShakeForcePosition;
	[SerializeField] private Vector3 m_screenShakeForceRotation;

	FocusArea focusArea;

	private float currentLookAheadX;
	private float targetLookAheadX;
	private float lookAheadDirX;
	private float smoothLookVelocityX;
	private float smoothVelocityY;

	bool lookAheadStopped;
	
	private void Awake(){
		GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterChangeClass>(OnCharacterChangeClass);
		GameManager.Instance.GlobalDispatcher.Subscribe<OnCameraScreenshake>(OnCameraScreenshake);
		GameManager.Instance.GlobalDispatcher.Subscribe<OnCameraConfigureOffset>(OnCameraLookPosition);
	}

	private void OnCameraLookPosition(OnCameraConfigureOffset ev) {

		if (ev.OffsetOrientation == 0) {
			m_verticalOffset = ev.OriginalOffsetValue;
		}
		else if (ev.OffsetOrientation == 1) {
			m_verticalOffset = ev.OffsetUpValue;
		}
		else if (ev.OffsetOrientation == 2) {
			m_verticalOffset = ev.OffsetDownValue;
		}
	}


	private void OnCameraScreenshake(OnCameraScreenshake ev) {
		Time.timeScale = 0.6f;
		transform.DOShakeRotation(ev.ShakeDuration, m_screenShakeForceRotation, ev.Shakeforce,5);
		transform.DOShakePosition(ev.ShakeDuration, m_screenShakeForcePosition, ev.Shakeforce, 5).OnComplete(() => { Time.timeScale = 1; });
	}

	private void OnCharacterChangeClass(OnCharacterChangeClass ev){
		target = ev.CurrentCharacter;
		focusArea = new FocusArea (target.Controller2D.collider.bounds, m_focusAreaSize);
	}
	
	private void FixedUpdate() {
		
		if (target == null) {
			return;
		}
		focusArea.Update (target.Controller2D.collider.bounds);
		
		Vector2 focusPosition = focusArea.centre + Vector2.up * m_verticalOffset;

		if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign (focusArea.velocity.x);
			if (Mathf.Sign(target.Velocity.x) == Mathf.Sign(focusArea.velocity.x) && target.Velocity.x != 0) {
				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirX * m_lookAheadDstX;
			}
			else {
				if (!lookAheadStopped) {
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * m_lookAheadDstX - currentLookAheadX)/4f;
				}
			}
		}
		
		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, m_lookSmoothTimeX);

		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, m_verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;
		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.centre, m_focusAreaSize);
	}

	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;
		float left,right;
		float top,bottom;


		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}
   }
}