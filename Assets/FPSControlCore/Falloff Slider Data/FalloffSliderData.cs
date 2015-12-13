using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class FalloffSliderData : object, IEnumerable {

    public float distance;
    [SerializeField] List<FalloffSliderPoint> _points = new List<FalloffSliderPoint>();

    public FalloffSliderData() { }

    public FalloffSliderPoint[] ToArray()
    {
        return _points.ToArray();
    }

    public int Length { get { return _points.Count; } }

    public FalloffSliderPoint this[int index]
    {
        get
        {
            return _points[index];
        }
        set
        {
            _points[index] = value;
            Reorder();
        }
    }

    public int this[FalloffSliderPoint point]
    {
        get
        {
            return IndexOf(point);
        }
    }

    public IEnumerator GetEnumerator()
    {
        for(int i = 0; i < _points.Count; i++)
        {
            yield return _points[i];
        }
    }

    public int IndexOf(FalloffSliderPoint point)
    {
        return _points.IndexOf(point);
    }

    public void Add(FalloffSliderPoint point)
    {
        _points.Add(point);
		Reorder();
    }

    public void Remove(FalloffSliderPoint point)
    {
        _points.Remove(point);
		Reorder();
    }

    public void RemoveAt(int index)
    {
        _points.RemoveAt(index);
		Reorder();
    }
	
	void Reorder()
	{
        //Debug.LogWarning("Reordering points");
        _points = _points.OrderBy(point => point.location).ToList();
	}

}

[System.Serializable]
public class FalloffSliderPoint : object, System.IComparable
{
    float _location;
    public float location { get { return _location; } set { _location = value; } }
    float _value;
    public float value { get { return _value; } set { _value = value; } }

    public FalloffSliderPoint(float location, float value)
    {
        _location = Mathf.Clamp(location, 0F, 1F);
        _value = Mathf.Clamp(value, 0F, 1F);
    }

    public int CompareTo(object other)
    {
        FalloffSliderPoint p = (FalloffSliderPoint)other;
        if(p.location < _location) return -1;
        else if(p.location > _location) return 1;
        else if(p.location == _location) return 0;

        return 0;
    }

    public static implicit operator Vector2(FalloffSliderPoint p)
    {
        return new Vector2(p.location, p.value);
    }

    public static implicit operator bool(FalloffSliderPoint p)
    {
        return p != null;
    }
}