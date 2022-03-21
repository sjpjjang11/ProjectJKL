using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text;

public static class Utility
{
	public static void Activate(GameObject _obj, bool _activation)
	{
		if (IsActive(_obj) == _activation)
		{
			return;
		}

		_obj.SetActive(_activation);
	}

	public static void Activate(Behaviour _obj, bool _activation)
	{
		if (IsActive(_obj) == _activation)
		{
			return;
		}

		_obj.enabled = _activation;
	}

	public static void Activate(Renderer _obj, bool _activation)
	{
		if (IsActive(_obj) == _activation)
		{
			return;
		}

		_obj.enabled = _activation;
	}

	public static void Activate(Collider _obj, bool _activation)
	{
		if (IsActive(_obj) == _activation)
		{
			return;
		}

		_obj.enabled = _activation;
	}

	public static bool IsActive(GameObject _obj)
	{
		return _obj.activeSelf;
	}

	public static bool IsActive(Behaviour _obj)
	{
		return _obj.isActiveAndEnabled;
	}

	public static bool IsActive(Renderer _obj)
	{
		return _obj.isVisible;
	}

	public static bool IsActive(Collider _obj)
	{
		return _obj.enabled;
	}

	public static float Distance(Vector3 _from, Vector3 _to)
	{
		Vector2 From = new Vector2(_from.x, _from.z);
		Vector2 To = new Vector2(_to.x, _to.z);

		float Distance = Vector2.Distance(From, To);

		return (float)Math.Round(Distance, 2);
	}

	public static Vector3 RelativePosition(Vector3 _from, Vector3 _to)
	{
		return _to - _from;
	}

	public static bool CheckInside(Vector3 _center, Vector3 _point, float _radius)
	{
		return Vector3.Distance(_center, _point) < _radius;
	}

	public static Vector3 ClosestPoint(Vector3 _from, Collider _target, Vector3 _targetPosition, Quaternion _targetRotation)
	{
		MeshCollider TargetMeshCollider = GetComponentNoAlloc<MeshCollider>(_target.gameObject);

		Vector3 ClosestPoint = Vector3.zero;

		if (TargetMeshCollider != null)
		{
			TargetMeshCollider.convex = true;
		}

		ClosestPoint = Physics.ClosestPoint(_from, _target, _targetPosition, _targetRotation);

		if (TargetMeshCollider != null)
		{
			TargetMeshCollider.convex = false;
		}

		return ClosestPoint;
	}

	public static string GetThousandCommaText(int data)
	{
		if (data.CompareTo(0) == 0)
		{
			return data.ToString();
		}

		return string.Format("{0:#,###}", data);
	}

	/// <summary>
	/// 전체값의 몇 퍼센트는 얼마인가 계산
	/// </summary>
	/// <param name="_totalValue"></param>
	/// <param name="_percent"></param>
	/// <returns></returns>
	public static float GetPercentOfTotalValue(float _totalValue, float _percent)
	{
		return _totalValue * _percent * 0.01f;
	}

	/// <summary>
	/// 전체값에서 일부값은 몇 퍼센트인가 계산
	/// </summary>
	/// <param name="_totalValue">전체값</param>
	/// <param name="_someValue">일부값</param>
	/// <param name="ExcessPermission"></param>
	/// <returns></returns>
	public static float GetSomeValueOfTotalValue(float _totalValue, float _someValue, bool ExcessPermission = true)
	{
		float result = _someValue / _totalValue * 100.0f;

		if (!ExcessPermission)
		{
			if (result > 100.0f)
			{
				result = 100.0f;
			}
		}

		return result;
	}

	public static Vector2 GetSomeValueOfTotalValue(Vector2 _totalValue, Vector2 _someValue, bool ExcessPermission = true)
	{
		Vector2 result = new Vector2(_someValue.x / _totalValue.x * 100.0f, _someValue.y / _totalValue.y * 100.0f);

		if (!ExcessPermission)
		{
			if (result.x > 100.0f)
			{
				result.x = 100.0f;
			}

			if (result.y > 100.0f)
			{
				result.y = 100.0f;
			}
		}

		return result;
	}

	public static Vector3 LocalToGlobalTransVector(Transform tr, Vector3 v)
	{
		tr.localPosition = v;

		return tr.position;
	}

	public static Vector3 GlobalToLocalTransVector(Transform tr, Vector3 v)
	{
		tr.position = v;

		return tr.localPosition;
	}

	public static Vector3 RotationToEuler(Transform tr)
	{
		return tr.eulerAngles;
	}

	public static Vector3 RotationToEuler(Transform tr, Quaternion que)
	{
		tr.rotation = que;

		return tr.eulerAngles;
	}

	public static float GetNegativeAngleValue(float _angle)
	{
		float Result;

		Result = (_angle > 180) ? _angle - 360 : _angle;

		return Result;
	}

	public static Vector3 NormalizeAngle(Vector3 _angle)
	{
		Vector3 Result = _angle;

		if (Result.x > 180)
		{
			Result.x -= 360;
		}
		else if (Result.x < -180)
		{
			Result.x += 360;
		}

		if (Result.y > 180)
		{
			Result.y -= 360;
		}
		else if (Result.y < -180)
		{
			Result.y += 360;
		}

		if (Result.z > 180)
		{
			Result.z -= 360;
		}
		else if (Result.z < -180)
		{
			Result.z += 360;
		}

		return Result;
	}

	public static Vector3 GetAxisRaw(Vector3 _direction)
	{
		float X = 0.0f;
		float Z = 0.0f;

		if (Mathf.Sign(_direction.x) == 1)
		{
			if (_direction.x == 0.0f)
			{
				X = 0.0f;
			}
			else
			{
				X = 1.0f;
			}
		}
		else
		{
			X = -1.0f;
		}

		if (Mathf.Sign(_direction.z) == 1)
		{
			if (_direction.z == 0.0f)
			{
				Z = 0.0f;
			}
			else
			{
				Z = 1.0f;
			}
		}
		else
		{
			Z = -1.0f;
		}

		return new Vector3(X, _direction.y, Z);
	}

	public static bool CheckNecessity(Transform _transform)
	{
		bool IsNecessity = false;

		if (Camera.main.WorldToViewportPoint(_transform.position).y < 0.0f)
		{
			IsNecessity = true;
		}

		return IsNecessity;
	}

	public static uint TimeStamp()
	{
		TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));

		return (uint)timeSpan.TotalSeconds;
	}

	public static void InstantiateField(object _obj, Type _type, bool _isSetFieldName)
	{
		// 필드 저장할 List 인스턴스 생성
		List<FieldInfo> FieldList = new List<FieldInfo>();

		object Obj;

		// 필드 얻어옴
		FieldList.AddRange(_type.GetFields((BindingFlags.Public | BindingFlags.Instance)));

		if (FieldList == null || FieldList.Count == 0)
		{
			return;
		}

		for (int i = 0; i < FieldList.Count; i++)
		{
			// 필드의 해당 타입과 이름으로 인스턴스 생성
			if (_isSetFieldName)
			{
				Obj = Activator.CreateInstance(FieldList[i].FieldType, FieldList[i].Name);
			}
			else
			{
				Obj = Activator.CreateInstance(FieldList[i].FieldType);
			}

			// 생성한 인스턴스를 필드에 값으로 설정
			FieldList[i].SetValue(_obj, Obj);
		}
	}

	public static T To<T>(object _value)
	{
		return (T)Convert.ChangeType(_value, typeof(T));
	}

	public static T EnumParse<T>(string _string)
	{
		return (T)Enum.Parse(typeof(T), _string);
	}

	public static string DetermineObjectPoolName(string _prefix, string _owner = "")
	{
		return ("[" + _prefix + "]" + _owner);
	}

	static List<Component> m_ListComponentCache = new List<Component>();

	public static T GetComponentNoAlloc<T>(GameObject _obj) where T : Component
	{
		_obj.GetComponents(typeof(T), m_ListComponentCache);
		var component = m_ListComponentCache.Count > 0 ? m_ListComponentCache[0] : null;
		m_ListComponentCache.Clear();
		return component as T;
	}

	public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict,
										   TKey oldKey, TKey newKey)
	{
		if (!dict.TryGetValue(oldKey, out TValue value))
			return false;

		dict.Remove(oldKey);  // do not change order
		if (dict.ContainsKey(newKey))
		{
			UnityEngine.Debug.LogError("oldKey : " + oldKey + "  " + "newKey : " + newKey);
		}
		dict.Add(newKey, value);  // or dict.Add(newKey, value) depending on ur comfort
		return true;
	}

	public static Type GetCallerType(int _index)
	{
		return new StackTrace().GetFrame(_index).GetMethod().DeclaringType;
	}

	/*public static Vector3 MoveBifurcation(Vector3 dir, float bifurcation, float max)
	{
		float x = 0.0f;
		float z = 0.0f;

		if(dir.x != 0.0f && Mathf.Abs(dir.x) >= 0.1f)
		{
			if (Mathf.Sign(dir.x) == 1)
			{
				if (dir.x <= bifurcation)
				{
					x = bifurcation;
				}
				else
				{
					x = max;
				}
			}
			else
			{
				if (dir.x >= -bifurcation)
				{
					x = -bifurcation;
				}
				else
				{
					x = -max;
				}
			}
		}

		if(dir.z != 0.0f && Mathf.Abs(dir.z) >= 0.1f)
		{
			if (Mathf.Sign(dir.z) == 1)
			{
				if (dir.z <= bifurcation)
				{
					z = bifurcation;
				}
				else
				{
					z = max;
				}
			}
			else
			{
				if (dir.z >= -bifurcation)
				{
					z = -bifurcation;
				}
				else
				{
					z = -max;
				}
			}
		}
		
		dir.x = x;
		dir.z = z;

		//Debug.Log(dir);
		return dir;
	}*/

	public static void DestroyScene(string _sceneName)
	{
		if (SceneManager.GetSceneByName(_sceneName).isLoaded)
		{
			SceneManager.UnloadSceneAsync(_sceneName);
		}
	}

	public static void SetStretchAnchorsValue(RectTransform rect, float parentX = 0, float parentY = 0)
	{
		RectTransform parentRect;

		Vector2 parentSize;
		Vector2 tempAnchorMax;
		Vector2 tempAnchorMin;
		Vector2 mySize;
		Vector2 myPos;

		float anchorMaxX;
		float anchorMaxY;

		float anchorMinX;
		float anchorMinY;

		float maxBlank;
		float minBlank;

		float blankPersent = 0.0f;
		float anchorsBlank;

		parentSize = new Vector2(parentX, parentY);

		if (parentSize == Vector2.zero)
		{
			parentRect = rect.parent.GetComponent<RectTransform>();

			tempAnchorMax = parentRect.anchorMax;
			tempAnchorMin = parentRect.anchorMin;

			//parentRect.anchorMax = new Vector2(0.5f, 0.5f);
			//parentRect.anchorMin = new Vector2(0.5f, 0.5f);

			//parentSize = parentRect.sizeDelta;
			//parentSize = new Vector2(parentRect.rect.width, parentRect.rect.height);

			//parentRect.anchorMax = tempAnchorMax;
			//parentRect.anchorMin = tempAnchorMin;

			parentSize = new Vector2(rect.parent.GetComponent<RectTransform>().rect.width, rect.parent.GetComponent<RectTransform>().rect.height);
			//Debug.Log("Parent Width : " + rect.parent.GetComponent<RectTransform>().rect.width + " " + "Parent height : " + rect.parent.GetComponent<RectTransform>().rect.height);
		}

		//parentSize = new Vector2(rect.parent.GetComponent<RectTransform>().rect.width, rect.parent.GetComponent<RectTransform>().rect.height);
		mySize = new Vector2(rect.rect.width, rect.rect.height);
		myPos = rect.localPosition;

		//max x, min x 세팅
		if (Math.Sign(myPos.x).CompareTo(1) == 0) //위치값이 +
		{
			//Debug.Log("Sign = 1");
			//max쪽의 여백 구하기
			//위치값이 플러스이므로 max쪽(Right, Top)의 여백이 그만큼 줄어들기 때문에 위치값을 빼준다.
			maxBlank = ((parentSize.x - mySize.x) / 2) - myPos.x;
			//Debug.Log("maxBlank : " + parentSize + " - " + mySize + " / " + 2 + " - " + setPos + " = " + maxBlank);
			//만약 max쪽의 여백이 마이너스(-)라면 이미 max의 최대치(1)이므로 계산하는 의미가 없음.          
			if (Mathf.Sign(maxBlank).CompareTo(-1) == 0)
			{
				//그대로 1 저장
				anchorMaxX = 1;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = maxBlank / parentSize.x * 100.0f;
				//Debug.Log("Max blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Max anchorsBlank : " + anchorsBlank);
				//max의 최대치(1)에서 anchorsBlank값을 빼주면 자신과 부모의 
				//여백 중에서 max쪽(Right, Top)의 anchors값을 얻을 수 있다.
				anchorMaxX = 1 - anchorsBlank;
				//Debug.Log("anchorsMax : " + anchorsMax);
			}

			//min쪽의 여백 구하기
			//위치값이 플러스이므로 min쪽(Left, Bottom)의 여백이 그만큼 늘어나기 때문에 위치값을 더해준다.
			minBlank = ((parentSize.x - mySize.x) / 2) + myPos.x;
			//Debug.Log("minBlank : " + minBlank);

			if (Mathf.Sign(minBlank).CompareTo(-1) == 0)
			{
				//그대로 0 저장
				anchorMinX = 0;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = minBlank / parentSize.x * 100.0f;
				//Debug.Log("Min blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Min anchorsBlank : " + anchorsBlank);
				//min의 최소치(0)에서 anchorsBlank값을 더해주면 자신과 부모의 
				//여백 중에서 min쪽(Left, Bottom)의 anchors값을 얻을 수 있다.
				anchorMinX = anchorsBlank;
				//Debug.Log("anchorsMin : " + anchorsMin);
			}
		}
		else if (Math.Sign(myPos.x).CompareTo(-1) == 0) //위치값이 -
		{
			//Debug.Log("Sign = -1");
			//max쪽의 여백 구하기
			//위치값이 마이너스이므로 max쪽(Right, Top)의 여백이 그만큼 늘어나기 때문에 위치값을 더해준다.
			maxBlank = ((parentSize.x - mySize.x) / 2) + myPos.x;

			//만약 max쪽의 여백이 마이너스(-)라면 이미 max의 최대치(1)이므로 계산하는 의미가 없음.          
			if (Mathf.Sign(maxBlank).CompareTo(-1) == 0)
			{
				//그대로 1 저장
				anchorMaxX = 1;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = maxBlank / parentSize.x * 100.0f;
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//max의 최대치(1)에서 anchorsBlank값을 빼주면 자신과 부모의 
				//여백 중에서 max쪽(Right, Top)의 anchors값을 얻을 수 있다.
				anchorMaxX = 1 - anchorsBlank;
			}

			//min쪽의 여백 구하기
			//위치값이 마이너스이므로 min쪽(Left, Bottom)의 여백이 그만큼 줄어들기 때문에 위치값을 빼준다.
			minBlank = ((parentSize.x - mySize.x) / 2) - myPos.x;

			//만약 min쪽의 여백이 마이너스(-)라면 이미 min의 최소치(0)이므로 계산하는 의미가 없음.           
			if (Mathf.Sign(minBlank).CompareTo(-1) == 0)
			{
				//그대로 0 저장
				anchorMinX = 0;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = minBlank / parentSize.x * 100.0f;
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//min의 최소치(0)에서 anchorsBlank값을 더해주면 자신과 부모의 
				//여백 중에서 min쪽(Left, Bottom)의 anchors값을 얻을 수 있다.
				anchorMinX = anchorsBlank;
			}
		}
		else //위치값이 0
		{
			//Debug.Log("Sign = 0");
			maxBlank = minBlank = ((parentSize.x - mySize.x) / 2);
			//Debug.Log("Blank : " + maxBlank + " " + minBlank);
			if (Mathf.Sign(maxBlank).CompareTo(-1) == 0)
			{
				//그대로 1 저장
				anchorMaxX = 1;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = maxBlank / parentSize.x * 100.0f;
				//Debug.Log("Max blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Max anchorsBlank : " + anchorsBlank);
				//max의 최대치(1)에서 anchorsBlank값을 빼주면 자신과 부모의 
				//여백 중에서 max쪽(Right, Top)의 anchors값을 얻을 수 있다.
				anchorMaxX = 1 - anchorsBlank;
				//Debug.Log("anchorsMax : " + anchorsMax);
			}

			//만약 min쪽의 여백이 마이너스(-)라면 이미 min의 최소치(0)이므로 계산하는 의미가 없음.           
			if (Mathf.Sign(minBlank).CompareTo(-1) == 0)
			{
				//그대로 0 저장
				anchorMinX = 0;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = minBlank / parentSize.x * 100.0f;
				//Debug.Log("Min blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Min anchorsBlank : " + anchorsBlank);
				//min의 최소치(0)에서 anchorsBlank값을 더해주면 자신과 부모의 
				//여백 중에서 min쪽(Left, Bottom)의 anchors값을 얻을 수 있다.
				anchorMinX = anchorsBlank;
				//Debug.Log("anchorsMin : " + anchorsMin);
			}
		}

		//////////////////////////////////////////////////////////////////////////////

		//max y, min y 세팅
		if (Math.Sign(myPos.y).CompareTo(1) == 0) //위치값이 +
		{
			//Debug.Log("Sign = 1");
			//max쪽의 여백 구하기
			//위치값이 플러스이므로 max쪽(Right, Top)의 여백이 그만큼 줄어들기 때문에 위치값을 빼준다.
			maxBlank = ((parentSize.y - mySize.y) / 2) - myPos.y;
			//Debug.Log("maxBlank : " + parentSize + " - " + mySize + " / " + 2 + " - " + setPos + " = " + maxBlank);
			//만약 max쪽의 여백이 마이너스(-)라면 이미 max의 최대치(1)이므로 계산하는 의미가 없음.          
			if (Mathf.Sign(maxBlank).CompareTo(-1) == 0)
			{
				//그대로 1 저장
				anchorMaxY = 1;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = maxBlank / parentSize.y * 100.0f;
				//Debug.Log("Max blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Max anchorsBlank : " + anchorsBlank);
				//max의 최대치(1)에서 anchorsBlank값을 빼주면 자신과 부모의 
				//여백 중에서 max쪽(Right, Top)의 anchors값을 얻을 수 있다.
				anchorMaxY = 1 - anchorsBlank;
				//Debug.Log("anchorsMax : " + anchorsMax);
			}

			//min쪽의 여백 구하기
			//위치값이 플러스이므로 min쪽(Left, Bottom)의 여백이 그만큼 늘어나기 때문에 위치값을 더해준다.
			minBlank = ((parentSize.y - mySize.y) / 2) + myPos.y;
			//Debug.Log("minBlank : " + minBlank);

			if (Mathf.Sign(minBlank).CompareTo(-1) == 0)
			{
				//그대로 0 저장
				anchorMinY = 0;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = minBlank / parentSize.y * 100.0f;
				//Debug.Log("Min blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Min anchorsBlank : " + anchorsBlank);
				//min의 최소치(0)에서 anchorsBlank값을 더해주면 자신과 부모의 
				//여백 중에서 min쪽(Left, Bottom)의 anchors값을 얻을 수 있다.
				anchorMinY = anchorsBlank;
				//Debug.Log("anchorsMin : " + anchorsMin);
			}
		}
		else if (Math.Sign(myPos.y).CompareTo(-1) == 0) //위치값이 -
		{
			//Debug.Log("Sign = -1");
			//max쪽의 여백 구하기
			//위치값이 마이너스이므로 max쪽(Right, Top)의 여백이 그만큼 늘어나기 때문에 위치값을 더해준다.
			maxBlank = ((parentSize.y - mySize.y) / 2) + Mathf.Abs(myPos.y);

			//만약 max쪽의 여백이 마이너스(-)라면 이미 max의 최대치(1)이므로 계산하는 의미가 없음.          
			if (Mathf.Sign(maxBlank).CompareTo(-1) == 0)
			{
				//그대로 1 저장
				anchorMaxY = 1;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = maxBlank / parentSize.y * 100.0f;
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//max의 최대치(1)에서 anchorsBlank값을 빼주면 자신과 부모의 
				//여백 중에서 max쪽(Right, Top)의 anchors값을 얻을 수 있다.
				anchorMaxY = 1 - anchorsBlank;
			}

			//min쪽의 여백 구하기
			//위치값이 마이너스이므로 min쪽(Left, Bottom)의 여백이 그만큼 줄어들기 때문에 위치값을 빼준다.
			minBlank = ((parentSize.y - mySize.y) / 2) - Mathf.Abs(myPos.y);

			//만약 min쪽의 여백이 마이너스(-)라면 이미 min의 최소치(0)이므로 계산하는 의미가 없음.           
			if (Mathf.Sign(minBlank).CompareTo(-1) == 0)
			{
				//그대로 0 저장
				anchorMinY = 0;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = minBlank / parentSize.y * 100.0f;
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//min의 최소치(0)에서 anchorsBlank값을 더해주면 자신과 부모의 
				//여백 중에서 min쪽(Left, Bottom)의 anchors값을 얻을 수 있다.
				anchorMinY = anchorsBlank;
			}
		}
		else //위치값이 0
		{
			//Debug.Log("Sign = 0");
			maxBlank = minBlank = ((parentSize.y - mySize.y) / 2);
			//Debug.Log("Blank : " + maxBlank + " " + minBlank);
			if (Mathf.Sign(maxBlank).CompareTo(-1) == 0)
			{
				//그대로 1 저장
				anchorMaxY = 1;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = maxBlank / parentSize.y * 100.0f;
				//Debug.Log("Max blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Max anchorsBlank : " + anchorsBlank);
				//max의 최대치(1)에서 anchorsBlank값을 빼주면 자신과 부모의 
				//여백 중에서 max쪽(Right, Top)의 anchors값을 얻을 수 있다.
				anchorMaxY = 1 - anchorsBlank;
				//Debug.Log("anchorsMax : " + anchorsMax);
			}

			//만약 min쪽의 여백이 마이너스(-)라면 이미 min의 최소치(0)이므로 계산하는 의미가 없음.           
			if (Mathf.Sign(minBlank).CompareTo(-1) == 0)
			{
				//그대로 0 저장
				anchorMinY = 0;
			}
			else
			{
				//여백값의 퍼센트값을 구함
				blankPersent = minBlank / parentSize.y * 100.0f;
				//Debug.Log("Min blankPersent : " + blankPersent);
				//1에서 blankPersent(%)가 얼마를 차지하는지
				anchorsBlank = 1 * blankPersent / 100.0f;
				//Debug.Log("Min anchorsBlank : " + anchorsBlank);
				//min의 최소치(0)에서 anchorsBlank값을 더해주면 자신과 부모의 
				//여백 중에서 min쪽(Left, Bottom)의 anchors값을 얻을 수 있다.
				anchorMinY = anchorsBlank;
				//Debug.Log("anchorsMin : " + anchorsMin);
			}
		}

		rect.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
		rect.anchorMin = new Vector2(anchorMinX, anchorMinY);
	}


	/// <summary>
	/// 인자 값으로 받은 스트링 Array 값을 StringBuilder 로 문장으로 만들어 리턴
	/// </summary>
	public static string CreateStringBuilderStr(string[] _strArray)
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < _strArray.Length; i++)
		{
			sb.Append(_strArray[i]);
		}

		return sb.ToString();
	}

	/// <summary>
	/// 인자 값으로 받은 int를 bool 치환 
	/// </summary>
	public static bool GetBool(int _isbool)
	{
		bool b = true;

		if (_isbool.Equals(1))
		{
			b = true;
		}
		else if (_isbool.Equals(0))
		{
			b = false;
		}

		return b;
	}

	/// <summary>
	/// 인자 값으로 받은 스트링을 bool 치환 
	/// </summary>
	public static bool GetBool(string _isbool)
	{
		bool b = true;

		if (_isbool.Equals("TRUE"))
		{
			b = true;
		}
		else if (_isbool.Equals("FALSE"))
		{
			b = false;
		}

		return b;
	}
}

