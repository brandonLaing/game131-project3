using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RevolvingList<T>
{
    private List<T> _list;
    private int _currentIndex = 0;
    private int currentIndex
    {
        get
        {
            return _currentIndex;
        }
        set
        {
            _currentIndex = value;
            if (_currentIndex > _list.Count)
                _currentIndex = 0;
        }
    }

    public RevolvingList(List<T> newList)
    {
        if (newList == null || newList.Count < 2)
            throw new Exception("Revolving list needs to have at least 2 points");
    
        _list = newList;
    }

    public T Next
    {
        get
        {
            return _list[currentIndex++];
        }
    }

    public T Start
    {
        get
        {
            return _list[0];
        }
    }
}