using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{ 
	private T[] Items;
	private int CurrentItemCount;

	public int Count
	{
		get
		{
			return CurrentItemCount;
		}
	}

	public Heap(int _maxHeapSize)
	{
		Items = new T[_maxHeapSize];
	}

	public void Add(T _item)
	{
		_item.HeapIndex = CurrentItemCount;
		Items[CurrentItemCount] = _item;
		SortUp(_item);
		CurrentItemCount++;
	}

	public T RemoveFirst()
	{
		T FirstItem = Items[0];
		CurrentItemCount--;
		Items[0] = Items[CurrentItemCount];
		Items[0].HeapIndex = 0;
		SortDown(Items[0]);

		return FirstItem;
	}

	public void UpdateItem(T _item)
	{
		SortUp(_item);
	}

	public bool Contains(T _item)
	{
		return Equals(Items[_item.HeapIndex], _item);
	}

	private void SortDown(T _item)
	{
		while(true)
		{
			int ChildIndexLeft = _item.HeapIndex * 2 + 1;
			int ChildIndexRight = _item.HeapIndex * 2 + 2;
			int SwapIndex = 0;

			if(ChildIndexLeft < CurrentItemCount)
			{
				SwapIndex = ChildIndexLeft;

				if(ChildIndexRight < CurrentItemCount)
				{
					if(Items[ChildIndexLeft].CompareTo(Items[ChildIndexRight]) < 0)
					{
						SwapIndex = ChildIndexRight;
					}
				}

				if(_item.CompareTo(Items[SwapIndex]) < 0)
				{
					Swap(_item, Items[SwapIndex]);
				}
				else
				{
					return;
				}
			}
			else
			{
				return;
			}
		}
	}

	private void SortUp(T _item)
	{
		int ParentIndex = (_item.HeapIndex - 1) / 2;

		while(true)
		{
			T ParentItem = Items[ParentIndex];

			if(_item.CompareTo(ParentItem) > 0)
			{
				Swap(_item, ParentItem);
			}
			else
			{
				break;
			}

			ParentIndex = (_item.HeapIndex - 1) / 2;
		}
	}

	private void Swap(T _itemA, T _itemB)
	{
		Items[_itemA.HeapIndex] = _itemB;
		Items[_itemB.HeapIndex] = _itemA;

		int ItemAIndex = _itemA.HeapIndex;

		_itemA.HeapIndex = _itemB.HeapIndex;
		_itemB.HeapIndex = ItemAIndex;

	}
}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex
	{
		get;
		set;
	}
}
